using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLayer.Messages
{
    public class UserMessages : IMessages
    {
        private string jsonFile = @"D:\projects\Mobizone\MobiZone\messages.json";
        public string Added { get; set; }
        public string AddedError { get; set; }
        public string Deleted { get; set; }
        public string DeltedError { get; set; }
        public string DuplicateData { get; set; }
        public string Null { get; set; }
        public string Updated { get; set; }
        public string UpdatedError { get; set; }
        public string ExceptionError { get; set; }

        public UserMessages()
        {
            JArray objArray = new JArray();
            objArray = Read();
            foreach (var users in objArray.ToList())
            {
                this.Added = JsonConvert(users["UserAdded"]);
                this.AddedError = JsonConvert(users["UserAddedError"]);
                this.ExceptionError = JsonConvert(users["UserException"]);
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