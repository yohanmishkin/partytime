using Jil;
using System.Dynamic;
using System.IO;
using System;
using System.Collections.Generic;

namespace Partytime
{
    public class Serializer
    {
        public Serializer()
        {
        }

        public string Serialize(object data)
        {
            dynamic stage = new ExpandoObject();
            if (data == null)
            {
                stage.data = null;
                data = stage;
                return SerializeObject(data);
            }
            else
                stage.data = NormalizeResource(data);

            return SerializeObject(stage);
        }

        private dynamic NormalizeResource(object data)
        {
            var typeName = data.GetType().Name;

            dynamic normalizedData = new ExpandoObject();
            normalizedData = Normalize(data);

            return normalizedData;
        }

        private dynamic Normalize(object data)
        {
            dynamic stage = new ExpandoObject();

            stage.attributes = ExtractAttributes(data);

            return stage;
        }

        private dynamic ExtractAttributes(object data)
        {
            dynamic attributes = new Dictionary<string, object>();

            var properties = data.GetType().GetProperties();
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    var key = property.Name.Dasherize();
                    var value = property.GetValue(data, null);
                    if (value != null)
                        attributes[key] = value;
                }
            }

            return attributes;
        }

        private static string SerializeObject(object data)
        {
            using (var output = new StringWriter())
            {
                JSON.SerializeDynamic(data, output);
                return output.ToString();
            }
        }
    }
}