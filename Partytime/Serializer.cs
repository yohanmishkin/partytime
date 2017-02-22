using Jil;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Partytime
{
    public class Serializer
    {
        public string Serialize(object data)
        {
            var hash = BuildPayload(data);
            return Jilify(hash);
        }

        private dynamic BuildPayload(object data)
        {
            dynamic documentHash = new ExpandoObject();
            documentHash.data = null;

            if (data != null)
            {
                var normalizedResource = NormalizeDocument(data); // resource and newIncludes
                documentHash.data = normalizedResource.data;
                if (HasProperty(normalizedResource, "included"))
                    documentHash.included = normalizedResource.included;
            }

            return documentHash;
        }

        private dynamic NormalizeDocument(object data, object included = null)
        {
            dynamic normalizedData = new ExpandoObject();
            if (data is IEnumerable)
                normalizedData.data = ((IEnumerable<dynamic>)data).Select(x => NormalizeResource(x));
            else
                normalizedData.data = NormalizeResource(data);

            return ExtractEmbeddedRecords(normalizedData);
        }

        private dynamic ExtractEmbeddedRecords(object data)
        {
            dynamic hash = data;

            if (!HasProperty(hash.data, "relationships"))
                return data;

            var relationships = hash.data.relationships;
            foreach (dynamic relationship in relationships)
            {
                foreach (dynamic item in relationship.Value)
                {
                    dynamic entries = (KeyValuePair<string, object>)item;
                    foreach (dynamic entry in entries.Value)
                    {
                        dynamic relationshipData = NormalizeResource(entry);
                        var hashDictionary = (IDictionary<string, object>)hash;
                        if (!hashDictionary.ContainsKey("included"))
                            hashDictionary.Add("included", new List<object>());

                        ((List<object>)hashDictionary["included"]).Add(relationshipData);
                    }
                }
            }


            //var type = entry.GetType();
            //var id = type.GetProperty("Id").GetValue(entry, null);

            //datas.Add(new Dictionary<string, object>
            //            {
            //                { "id", id },
            //                { "type", type.Name } // TODO: Dasherize()
            //            });

            return hash;
        }

        private dynamic NormalizeResource(object data)
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
                .Where(x => x.PropertyType.GetTypeInfo().IsPrimitive || x.PropertyType == typeof(string));

            foreach (var property in properties)
            {
                var key = property.Name.Dasherize();
                var value = property.GetValue(data, null);
                if (value != null)
                    attributes[key] = value;
            }

            return attributes;
        }

        private dynamic ExtractRelationships(object data)
        {
            dynamic relationships = new Dictionary<string, object>();

            var properties = data.GetType()
                .GetProperties()
                .Where(x => !x.PropertyType.GetTypeInfo().IsPrimitive && x.PropertyType != typeof(string));
            
            foreach (var property in properties)
            {
                var hasIdProperty = property.GetType()
                    .GetProperties()
                    .Any(x => x.Name.ToLower() == "id");

                var isList = property.GetValue(data, null) is IEnumerable;

                if (!hasIdProperty && !isList) // Convention!
                    continue;
                
                var key = property.Name.Dasherize();
                var value = property.GetValue(data, null);
                if (value != null)
                    relationships[key] = value;
            }

            dynamic extractedData = new Dictionary<string, object>();
            foreach (var relationshipItem in relationships)
            {
                var relationship = relationshipItem.Value;
                if (relationship is IEnumerable)
                {
                    var datas = new List<object>();
                    foreach (var entry in relationship)
                    {
                        datas.Add(entry);
                    }

                    extractedData[relationshipItem.Key] = new Dictionary<string, object> { { "data", datas } };
                }
                else
                {
                    dynamic relationshipData = new ExpandoObject();
                    relationshipData.id = 1;
                    string typeName = relationship.GetType().Name.ToString();
                    relationshipData.type = typeName.Dasherize();
                    extractedData[relationshipItem.Key] = new Dictionary<string, object> { { "data", relationshipData } };
                }
            }

            return extractedData;
        }

        private static string Jilify(object data)
        {
            using (var output = new StringWriter())
            {
                JSON.SerializeDynamic(data, output);
                return output.ToString();
            }
        }

        private bool HasProperty(object data, string propertyName)
        {
            if (!(data is IDictionary<string, object>))
                return false;

            var normalizedDictionary = (IDictionary<string, object>) data;
            return normalizedDictionary.ContainsKey(propertyName);
        }
    }
}