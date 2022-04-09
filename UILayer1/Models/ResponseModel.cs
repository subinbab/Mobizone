using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UILayer.Models
{
    
        public class ResponseModel<T>
        {
        public bool IsSuccess { get; set; }
        public int totalRecords { get; set; }
        public T result { get; set; }
        public string message { get; set; }
    }
   
}
