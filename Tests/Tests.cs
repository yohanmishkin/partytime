using Newtonsoft.Json.Linq;
using Partytime;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Tests
    {
        private Serializer _serializer;

        public Tests()
        {
            _serializer = new Serializer();
        }

        [Fact(DisplayName = "Serializes null")]
        public void SerializeNull()
        {
            var result = _serializer.Serialize(null);
            var payload = JObject.Parse(result);

            Assert.Equal(JTokenType.Null, payload["data"].Type);
        }

        [Fact(DisplayName = "Payload contains data")]
        public void SerializeObject()
        {
            var data = new Ewok();

            var result = _serializer.Serialize(data);
            var payload = JObject.Parse(result);

            Assert.NotNull(payload["data"]);
        }

        [Fact(DisplayName = "Resource is serialized with id and type")]
        public void ResourceHasIdAndType()
        {
            var data = new Ewok();

            var result = _serializer.Serialize(data);
            var payload = JObject.Parse(result);

            Assert.NotNull(payload["data"]["id"]);
            Assert.Equal("ewok", payload["data"]["type"].Value<string>());
        }

        [Fact(DisplayName = "Serializes attributes")]
        public void SerializesAttributes()
        {
            var data = new Ewok
            {
                Age = 1,
                Name = "Wicket"
            };

            var result = _serializer.Serialize(data);
            var payload = JObject.Parse(result);
            var attributes = payload["data"]["attributes"];

            Assert.Equal(data.Age, attributes["age"].Value<int>());
            Assert.Equal(data.Name, attributes["name"].Value<string>());
        }

        [Fact(DisplayName = "Serializes list")]
        public void SerializesList()
        {
            var data = new List<Ewok>
            {
                new Ewok { Id = 1 },
                new Ewok { Id = 2 },
                new Ewok { Id = 3 },
            };

            var result = _serializer.Serialize(data);
            var payload = JObject.Parse(result);

            Assert.Equal(3, payload["data"].Count());
        }
    }
}
