using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace ApiLayer.Messages
{
    public class CartMessages : IMessages
    {
        private IWebHostEnvironment _webHostEnvironment;
        string _jsonFile = null;

        public CartMessages(IWebHostEnvironment environment)
        {
            _webHostEnvironment = environment;
            string file = "messages.json";
            _jsonFile = Path.Combine(_webHostEnvironment.ContentRootPath, file);
            JArray objArray = new JArray();
            objArray = Read();

            foreach (var cartproduct in objArray.ToList())
            {
                this.Added = JsonConvert(cartproduct["ProductAdded"]);
                this.AddedError = JsonConvert(cartproduct["ProductAddedError"]);
                this.Updated = JsonConvert(cartproduct["ProductUpdated"]);
                this.UpdatedError = JsonConvert(cartproduct["ProductUpdatedError"]);
                this.ExceptionError = JsonConvert(cartproduct[" CartException"]);
                this.Deleted = JsonConvert(cartproduct["ProductDeleted"]);
                this.DeltedError = JsonConvert(cartproduct["ProductDeletedError"]);
               
            }


        }
        public string Added { get; set ; }
        public string AddedError { get; }
        public string Deleted { get ; set ; }
        public string DeltedError {get ; set ; }
        public string DuplicateData { get; set ; }
        public string Null { get; set ; }
        public string Updated { get ; set ; }
        public string UpdatedError { get ; set; }
        public string ExceptionError { get ; set; }
        string IMessages.AddedError { get ;set; }

        private JArray Read()
        {
            string json = File.ReadAllText(_jsonFile);
            var jObject = JObject.Parse(json);
            JArray userArray = (JArray)jObject["CartProduct"];
            return userArray;
        }

        private string JsonConvert(object data)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return json;
        }

    }
}
