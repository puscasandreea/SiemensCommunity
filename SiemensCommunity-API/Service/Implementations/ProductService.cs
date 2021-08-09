﻿using AutoMapper;
using Common;
using Data.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Service.Adapters;
using Service.Contracts;
using Service.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPhotoService _photoService;
        private readonly IPhotoRepository _photoRepository;
        private readonly ProductAdapter _productAdapter = new ProductAdapter();
        private readonly ProductDTOAdapter _productDTOAdapter = new ProductDTOAdapter();
        private readonly TokenDetailsAdapter _optionDetailsDTOAdapter = new TokenDetailsAdapter();
        private readonly ProductAdapter _addProductAdapter = new ProductAdapter();
        private readonly PhotoAdapter _photoAdapter = new PhotoAdapter();
        private readonly ProductFormAdapter _productFormDTOAdapter = new ProductFormAdapter();
        private readonly UpdateProductAdapter _updateProductAdapter = new UpdateProductAdapter();
        private readonly ILogger _logger;

        public ProductService(IProductRepository productRepository, IPhotoService photoService, IPhotoRepository photoRepository, ILoggerFactory logger)
        {
            _productRepository = productRepository;
            _photoService = photoService;
            _photoRepository = photoRepository;
            _logger = logger.CreateLogger("ProductService");
        }

        public async Task<Product> AddAsync(AddProduct addProduct)
        {
            Data.Models.Product returnedProduct= new Data.Models.Product();

            var result = await _photoService.UploadPhotoAsync(addProduct.File);
            if (result == null || result.Error != null)
            {
                _logger.LogError(MyLogEvents.ErrorUploadItem, "Error uploading photo with errors " + result.Error);
                throw new NotImplementedException();
            }
            var image = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = false
            };
            try
            {
                var photoInDb = await _photoRepository.AddAsync(_photoAdapter.Adapt(image));
                var adaptedProduct = _productAdapter.AdaptAddProductToProduct(addProduct);
                adaptedProduct.Photo = photoInDb;
                adaptedProduct.PhotoId = photoInDb.Id;
                returnedProduct = await _productRepository.AddAsync(adaptedProduct);
                _logger.LogInformation(MyLogEvents.InsertItem, "Product successfully added");
            } catch (Exception ex)
            {
                _logger.LogError(MyLogEvents.InsertItem, "Error while inserting product wiht message " + ex.Message);
            }

            return _productAdapter.Adapt(returnedProduct);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            bool result = false;
            try
            {
                result = await _productRepository.DeleteByIdAsync(id);
                _logger.LogInformation(MyLogEvents.InsertItem, "Successful insertion of product with id={id}", id);
            }catch(Exception ex)
            {
                _logger.LogError(MyLogEvents.InsertItem, "Error while deleting item with id={id}, with error {eroror}", id, ex.Message);
            }
            return result;
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            IEnumerable<Data.Models.Product> returnedProducts = new List<Data.Models.Product>();
            try
            {
                returnedProducts = await _productRepository.GetAsync();
                _logger.LogInformation(MyLogEvents.ListItems, "Got {count} products", returnedProducts.Count());
            }catch(Exception ex)
            {
                _logger.LogError(MyLogEvents.ListItems, "Error while getting product with message " + ex.Message);
            }
            return _productAdapter.AdaptList(returnedProducts);
        }

        public async Task<ProductFormDTO> GetByIdAsync(int id)
        {
            Data.Models.ProductFormDTO returnedProduct = new Data.Models.ProductFormDTO();
            try
            {
                returnedProduct = await _productRepository.FindById(id);
                _logger.LogInformation(MyLogEvents.GetItem, "Getting item with id={id}", id);
            }
            catch(Exception ex)
            {
                _logger.LogError(MyLogEvents.GetItem, "Error while getting item with id={id}, with error {error}", id, ex.Message);
            }
            return _productFormDTOAdapter.Adapt(returnedProduct);
        }

        public async Task<Product> UpdateAsync(UpdateProductDTO product)
        {
            var image = new Photo();
            var photoInDb = await _photoRepository.FindByURL(product.ImageURL);
            var oldPhotoId = photoInDb.Id;
            if (product.File != null)
            {
                var result = await _photoService.UploadPhotoAsync(product.File);
                if (result.Error != null)
                {
                    _logger.LogError(MyLogEvents.UploadItem, "Error while uploading photo: {error}", result.Error);
                }
                image.Url = result.SecureUrl.AbsoluteUri;
                image.PublicId = result.PublicId;
                image.IsMain = false;
                photoInDb = await _photoRepository.AddAsync(_photoAdapter.Adapt(image));
            }
            product.PhotoId = photoInDb.Id;
            var productUpdated = await _productRepository.UpdateAsync(_updateProductAdapter.Adapt(product), product.Id);
            if(oldPhotoId != photoInDb.Id)
                await _photoRepository.DeleteByIdAsync(photoInDb.Id);

            return _productAdapter.Adapt(productUpdated);
        }

       /* public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            var returnedProducts = await _productRepository.GetAsync();
            var filtered = returnedProducts.Where(x => x.CategoryId == categoryId).ToList();

            return _productAdapter.AdaptList(filtered);
        }*/

        public async Task<List<ProductDTO>> GetFiltredProducts(int selectedCategory, int selectedOption)
        {
            var returnedProducts = await _productRepository.GetFiltredProducts(selectedCategory, selectedOption);
            return _productDTOAdapter.AdaptList(returnedProducts);
        }

        public async Task<List<ProductDTO>> GetUserProductsAsync(int userId, int? selectedCategoryId)
        {
            if (selectedCategoryId == null)
            {
                var products = await _productRepository.GetUserProductsAsync(userId);
                _logger.LogInformation(products.Count() + "products found of all categories, of user with id " + userId);
                return _productDTOAdapter.AdaptList(products);
            }
            else
            {
                var products = await _productRepository.GetUserProductsByCategoryAsync(userId, selectedCategoryId.Value);
                _logger.LogInformation(products.Count() + "products found of user with id " + userId + "category id " + selectedCategoryId);
                return _productDTOAdapter.AdaptList(products);
            }
        }

        public async Task<List<ProductDTO>> GetUserAvailableProductsAsync(int userId, int? selectedCategoryId)
        {

            if (selectedCategoryId == null)
            {
               var products = await _productRepository.GetUserAvailableProductsAsync(userId);
                _logger.LogInformation(products.Count() + "products found available of all categories, of user with id " + userId);
                _logger.LogInformation(products.Count() + "products found available of all categories, of user with id " + userId);
                return _productDTOAdapter.AdaptList(products);
            }
            else
            {
                var products = await _productRepository.GetUserAvailableProductsByCategoryAsync(userId, selectedCategoryId.Value);
                _logger.LogInformation(products.Count() + "products found of user with id " + userId + "category id " + selectedCategoryId);
                return _productDTOAdapter.AdaptList(products);
            }

        }

        public async Task<List<ProductDTO>> GetUserLendProductsAsync(int userId, int? selectedCategoryId)
        {

            if (selectedCategoryId == null)
            {
                var products = await _productRepository.GetUserLendProductsAsync(userId);
                _logger.LogInformation(products.Count() + "products found lend of all categories, of user with id " + userId);
                return _productDTOAdapter.AdaptList(products);
            }
            else
            {
                var products = await _productRepository.GetUserLendProductsByCategoryAsync(userId, selectedCategoryId.Value);
                _logger.LogInformation(products.Count() + "products found lend of user with id " + userId + "category id " + selectedCategoryId);
                return _productDTOAdapter.AdaptList(products);
            }

        }
    }
}
