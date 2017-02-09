using Jil;
using System.Dynamic;
using System.IO;

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
            }
            else
                stage.data = data;

            return SerializeObject(stage);
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