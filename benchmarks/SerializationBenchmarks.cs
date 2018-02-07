using Newtonsoft.Json.Linq;
using Partytime;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using JsonApiDotNetCore.Serialization;

namespace Benchmarks
{
    public class SerializationBenchmarks
    {
        public void BenchmarkSerializers()
        {
            JsonApiDotNetCore.Builders.IDocumentBuilder documentBuilder = null;
            JsonApiDotNetCore.Services.IJsonApiContext jsonApiContext = null;
            var json_api_dotnet_serializer = new JsonApiSerializer(jsonApiContext, documentBuilder);

            
        }
    }
}
