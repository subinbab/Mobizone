using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;

namespace UILayer.Models
{
    public static class EnumConvertion
    {
        [JSInvokable]
        public static string EnumToString<T>()
            {
                var values = Enum.GetValues(typeof(T)).Cast<int>();
                var enumDictionary = values.ToDictionary(value => Enum.GetName(typeof(T), value));

                return JsonConvert.SerializeObject(enumDictionary);
            }
    }
}
