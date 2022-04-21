using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLayer.Models
{
    public class ResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public int totalRecords { get; set; }
        public T result { get; set; }
        public string message { get; set; }

        public void AddResponse(bool IsSuccess, int totalRecords, T result, string message)
        {
            this.IsSuccess = IsSuccess;
            this.result = result;
            this.message = message;
            this.totalRecords = totalRecords;
        }

    }
}
