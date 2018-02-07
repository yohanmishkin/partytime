using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;
using JsonApiDotNetCore.Builders;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Serialization;
using JsonApiDotNetCore.Services;
using Moq;
using Newtonsoft.Json.Serialization;
using Partytime;
using Saule;

namespace Benchmarks
{
    public class SerializerShowdown
    {
        [Benchmark]
        public string JsonApiDotNetCore() =>
        
            _json_api_dotnet_serializer.Serialize(
                new IdentifiableEwok { Age = 1, Name = "Wicket" }
            );

        
        [Benchmark]
        public string Saule() =>
        
            _saule.Serialize(
                new Ewok { Age = 1, Name = "Wicket" }, new Uri("http://api.com/api/ewoks")
            ).ToString();

        
        [Benchmark]
        public string Partytime() =>

            _partytime.Serialize(
                new Ewok { Age = 1, Name = "Wicket" }
            );

        private readonly JsonApiSerializer _json_api_dotnet_serializer;
        private readonly JsonApiSerializer<EwokResource> _saule;
        private readonly Serializer _partytime;

        public SerializerShowdown()
        {
            var contextGraphBuilder = new ContextGraphBuilder();
            contextGraphBuilder.AddResource<IdentifiableEwok>("Ewok");
            var contextGraph = contextGraphBuilder.Build();

            var jsonApiContextMock = new Mock<IJsonApiContext>();
            jsonApiContextMock.SetupAllProperties();
            jsonApiContextMock.Setup(m => m.ContextGraph).Returns(contextGraph);
            jsonApiContextMock.Setup(m => m.AttributesToUpdate).Returns(new Dictionary<AttrAttribute, object>());

            var jsonApiOptions = new JsonApiOptions();
            jsonApiOptions.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonApiContextMock.Setup(m => m.Options).Returns(jsonApiOptions);

            var documentBuilder = new DocumentBuilder(jsonApiContextMock.Object);

            _json_api_dotnet_serializer = new JsonApiSerializer(jsonApiContextMock.Object, documentBuilder);
            _saule = new JsonApiSerializer<EwokResource>();
            _partytime = new Serializer();
        }
    }

    internal class EwokResource : ApiResource
    {
        public EwokResource()
        {
            OfType("Ewok");
            Attribute(nameof(Ewok.Name));
            Attribute(nameof(Ewok.Age));
        }
    }

    internal class IdentifiableEwok : Identifiable
    {
        public int Id { get; set; }
        [Attr("Age")]
        public int Age { get; set; }
        [Attr("Name")]
        public string Name { get; set; }
    }

    internal class Ewok
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
    }
}
