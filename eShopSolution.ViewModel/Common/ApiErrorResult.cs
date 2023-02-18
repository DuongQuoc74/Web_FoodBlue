﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Common
{
    public class ApiErrorResult <T> : ApiResult<T>
    {
       
        public ApiErrorResult()
        {
            
        }
        public ApiErrorResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
        
        public ApiErrorResult(string[] validationErrors)
        {
            IsSuccessed = false;
            ValidationErrors = validationErrors;
        }
    }
}