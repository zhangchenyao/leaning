using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public class JsonFilesTool
    {
        public JObject Read( string path)
        {
            try
            {
                using StreamReader file = System.IO.File.OpenText(path);
                using JsonTextReader reader = new(file);
                JObject o = (JObject)JToken.ReadFrom(reader);
                
                return o;
            }catch(Exception e)
            {
                return null;
            }
        }

        public bool Write(string content,string path)
        {
            File.WriteAllText(path, content, System.Text.Encoding.UTF8);
            return true;
        }

        public bool Write(JObject content, string path)
        {
            return Write(path, content.ToString());
        }

        public bool Write(object content, string path)
        {
            return  Write(path, JsonConvert.SerializeObject(content));
            
        }
    }
}
