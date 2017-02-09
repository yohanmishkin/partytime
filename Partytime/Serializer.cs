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
            if (data == null)
            {
                dynamic nullData = new ExpandoObject();
                nullData.data = null;
                data = nullData;
            }

            using (var output = new StringWriter())
            {
                JSON.SerializeDynamic(data, output);
                return output.ToString();
            }
        }
    }
}