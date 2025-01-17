﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> AddAsync(T entity);
        //public Task<int> Delete(T entity);
        public Task<bool> DeleteByIdAsync(int id);
        //public Task<int> DeleteAll(IEnumerable<T> entities);
        public Task<IEnumerable<T>> GetAsync();
        //public Task<T> FindById(int id);
    }
}
