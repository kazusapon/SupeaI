using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utils.Models
{
    public class Utils
    {

        public static string Serializer<JSON>(JSON result)
        {
            string json =  JsonSerializer.Serialize(result);

            return json;
        }

        public static JSON DeSerializer<String, JSON>(string json, JSON j)
        {
            var obj = JsonSerializer.Deserialize<JSON>(json);

            return (JSON)obj;
        }
    }
}