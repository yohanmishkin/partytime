using BenchmarkDotNet.Attributes;
using JsonApiDotNetCore.Serialization;

namespace Benchmarks
{
    public class SerializerShowdown
    {
        public SerializerShowdown()
        {
            JsonApiDotNetCore.Builders.IDocumentBuilder documentBuilder = null;
            JsonApiDotNetCore.Services.IJsonApiContext jsonApiContext = null;
            var json_api_dotnet_serializer = new JsonApiSerializer(jsonApiContext, documentBuilder);
        }

        [Benchmark]
        public string JsonApiDotNetCore() => string.Empty;
    }
}
