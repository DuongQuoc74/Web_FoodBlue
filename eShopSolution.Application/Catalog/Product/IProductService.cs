﻿using eShopSolution.ViewModel.Catalog.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Product
{
    public interface IProductService
    {
        Task<ProductVM> GetById (int id, string languageId);
    }
}
