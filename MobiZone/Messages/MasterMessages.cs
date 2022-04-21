using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace ApiLayer.Messages
{
    public class MasterMessages : IMessages
    {
        private IWebHostEnvironment _webHostEnvironment;
        string _jsonFile = null;
        public MasterMessages(IWebHostEnvironment environment)
        {
            _webHostEnvironment = environment;

            string file = "messages.json";
            _jsonFile = Path.Combine(_webHostEnvironment.ContentRootPath, file);
            JArray objArray = new JArray();
            objArray = Read();
            foreach (var product in objArray.ToList())
            {
                this.Added = JsonConvert(product["MasterAdded"]);
                this.AddedError = JsonConvert(product["MasterAddedError"]);
                this.Updated = JsonConvert(product["MasterUpdated"]);
                this.UpdatedError = JsonConvert(product["MasterUpdatedError"]);
                this.Null = JsonConvert(product["MasterNull"]);
                this.Deleted = JsonConvert(product["MasterDeleted"]);
                this.DeltedError = JsonConvert(product["MasterDeltedError"]);
                this.DuplicateData = JsonConvert(product["MasterDuplicateData"]);
                this.ExceptionError = JsonConvert(product["MasterException"]);
            }
        }

        public string Added { get; set ; }
        public string AddedError { get ; set ; }
        public string Deleted { get ; set ; }
        public string DeltedError { get ; set ; }
        public string DuplicateData { get ; set; }
        public string Null { get; set ; }
        public string Updated { get ; set; }
        public string UpdatedError { get ; set; }
        public string ExceptionError { get; set; }

    private JArray Read()
    {
        string json = File.ReadAllText(_jsonFile);
        var jObject = JObject.Parse(json);
        JArray productArray = (JArray)jObject["Master"];
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
