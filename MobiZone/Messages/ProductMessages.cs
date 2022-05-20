using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace ApiLayer.Messages
{
    public class ProductMessages : IMessages
    {
        private IWebHostEnvironment _webHostEnvironment;
        string _jsonFile = null;
        public ProductMessages(IWebHostEnvironment environment)
        {
            _webHostEnvironment = environment;

            string file = "messages.json";
            _jsonFile = Path.Combine(_webHostEnvironment.ContentRootPath, file);
            JArray objArray = new JArray();
            objArray = Read();
            foreach (var product in objArray.ToList())
            {
                this.Added = JsonConvert(product["ProductAdded"]);
                this.AddedError = JsonConvert(product["ProductAddedError"]);
                this.Updated = JsonConvert(product["ProductUpdated"]);
                this.UpdatedError = JsonConvert(product["ProductUpdatedError"]);
                this.Null = JsonConvert(product["ProductNull"]);
                this.Deleted = JsonConvert(product["ProductDeleted"]);
                this.DeltedError = JsonConvert(product["ProductDeltedError"]);
                this.DuplicateData = JsonConvert(product["ProductDuplicateData"]);
                this.ExceptionError = JsonConvert(product["ProductException"]);
            }
        }


        public string Added { get; set; }
        public string Updated { get; set; }
        public string Deleted { get; set; }
        public string Null { get; set; }
        public string AddedError { get; set; }
        public string UpdatedError { get; set; }
        public string DeltedError { get; set; }
  
        public string DuplicateData { get; set; }
        public string ExceptionError { get; set; }
        private JArray Read()
        {
            string json = File.ReadAllText(_jsonFile);
            var jObject = JObject.Parse(json);
            JArray productArray = (JArray)jObject["Products"];
            return productArray;
        }
        private string JsonConvert(object data)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            /*var onlyLetters = new String(json.Where(c => Char.IsLetter(c)).ToArray()||c=> Char.IsWhiteSpace(c));*/
            return json;
        }

    }
}
