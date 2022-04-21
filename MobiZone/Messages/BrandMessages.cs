using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace ApiLayer.Messages
{
    public class BrandMessages : IMessages
    {
        private string jsonFile = @"D:\git\updatedMobizone\MobiZone\messages.json";
        public string Added { get; set; }
        public string AddedError { get; set; }
        public string Deleted { get; set; }
        public string DeltedError { get; set; }
        public string DuplicateData { get; set; }
        public string Null { get; set; }
        public string Updated { get; set; }
        public string UpdatedError { get; set; }
        public string ExceptionError { get; set; }
        public BrandMessages()
        {
            JArray objArray = new JArray();
            objArray = Read();
            foreach (var brand in objArray.ToList())
            {

                this.Added = JsonConvert(brand["BrandAdded"]);
                this.AddedError = JsonConvert(brand["ProductAddedError"]);
                this.Updated = JsonConvert(brand["BrandtUpdated"]);
                this.UpdatedError = JsonConvert(brand["BrandUpdatedError"]);
                this.Null = JsonConvert(brand["BrandNull"]);
                this.Deleted = JsonConvert(brand["BrandDeleted"]);
                this.DeltedError = JsonConvert(brand["BrandDeltedError"]);
                this.DuplicateData = JsonConvert(brand["BrandDuplicateData"]);
                this.ExceptionError = JsonConvert(brand["BrandException"]);
            }


        }
        private JArray Read()
        {
            string json = File.ReadAllText(jsonFile);
            var jObject = JObject.Parse(json);
            JArray userArray = (JArray)jObject["Users"];
            return userArray;
        }
        private string JsonConvert(object data)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return json;
        }
    }
}
