using Jil;
using System;
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
            using (var output = new StringWriter())
            {
                JSON.Serialize(data, output);
                return output.ToString();
            }
        }
    }
}