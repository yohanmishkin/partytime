using Jil;
using System.Dynamic;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Partytime
{
    public class Serializer
    {
        public Serializer()
        {
        }

        public string Serialize(object data)
        {
            dynamic documentHash = new ExpandoObject();
            documentHash.data = null;
            
            if (data != null)
                documentHash.data = NormalizeResource(data);

            return SerializeObject(documentHash);
        }

        private dynamic NormalizeResource(object data)
        {
            var typeName = data.GetType().Name;

            dynamic normalizedData = new ExpandoObject();
            if (data is IEnumerable)
                normalizedData = ((IEnumerable<dynamic>)data).Select(x => Normalize(x));
            else
                normalizedData = Normalize(data);

            return normalizedData;
        }

        private dynamic Normalize(object data)
        {
            dynamic stage = new ExpandoObject();

            stage.id = ExtractId(data);
            stage.type = ExtractType(data);
            stage.attributes = ExtractAttributes(data);
            stage.relationships = ExtractRelationships(data);

            return stage;
        }

        private dynamic ExtractId(object data)
        {
            return data.GetType().GetProperty("Id").GetValue(data, null);
        }

        private dynamic ExtractType(object data)
        {
            return data.GetType().Name.Dasherize(); // TODO: Purlarize?
        }

        private dynamic ExtractAttributes(object data)
        {
            dynamic attributes = new Dictionary<string, object>();

            var properties = data.GetType()
                .GetProperties()
                .Where(x => x.PropertyType.IsPrimitive || x.PropertyType == typeof(string));

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

        private dynamic ExtractRelationships(object data)
        {
            dynamic relationships = new Dictionary<string, object>();
            dynamic tempRelationships = new Dictionary<string, object>();

            var properties = data.GetType()
                .GetProperties()
                .Where(x => !x.PropertyType.IsPrimitive && x.PropertyType != typeof(string));

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    var key = property.Name.Dasherize();
                    var value = property.GetValue(data, null);
                    if (value != null)
                        tempRelationships[key] = value;
                }
            }

            foreach (var relationship in tempRelationships)
            {
                dynamic relationshipData = new ExpandoObject();
                relationshipData.id = 1;
                string typeName = relationship.Value.GetType().Name.ToString();
                relationshipData.type = typeName.Dasherize();
                relationships[relationship.Key] = new Dictionary<string, object> { { "data", relationshipData } };
            }

            return relationships;
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