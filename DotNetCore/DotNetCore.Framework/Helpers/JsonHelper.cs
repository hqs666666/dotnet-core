
using System.Text;
using Newtonsoft.Json;

namespace DotNetCore.FrameWork.Helpers
{
    public class JsonHelper
    {
        public static byte[] ObjectToBytes(object item)
        {
            var lJsonString = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(lJsonString);
        }

        public static T BytesToObject<T>(byte[] serializedObject)
        {
            if (null == serializedObject)
                return default(T);

            var lJsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(lJsonString);
        }

        public static string ObjectToJson<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T JsonToObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
