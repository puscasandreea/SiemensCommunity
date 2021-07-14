﻿using AutoMapper;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Adapters
{
    public class ProductDTOAdapter
    {
        private readonly IMapper _productAdapter;

        public ProductDTOAdapter()
        {
            var config = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTO, Data.Models.ProductDTO>();
                config.CreateMap<Data.Models.ProductDTO, ProductDTO>();
            });

            _productAdapter = config.CreateMapper();
        }

        public Data.Models.ProductDTO Adapt(ProductDTO product)
        {
            return _productAdapter.Map<ProductDTO, Data.Models.ProductDTO>(product);
        }

        public ProductDTO Adapt(Data.Models.ProductDTO product)
        {
            return _productAdapter.Map<Data.Models.ProductDTO, ProductDTO>(product);
        }

        public List<ProductDTO> AdaptList(List<Data.Models.ProductDTO> products)
        {
            return _productAdapter.Map<List<Data.Models.ProductDTO>, List<ProductDTO>>(products);
        }
    }
}
