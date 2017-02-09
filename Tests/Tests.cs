using Newtonsoft.Json.Linq;
using Partytime;
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
            var data = new Simple();

            var result = _serializer.Serialize(data);
            var payload = JObject.Parse(result);

            Assert.NotNull(payload["data"]);
        }

        [Fact(DisplayName = "Serializes attributes")]
        public void SerializesAttributes()
        {
            var data = new Simple
            {
                IntProperty = 1,
                StringProperty = "string"
            };

            var result = _serializer.Serialize(data);
            var payload = JObject.Parse(result);
            var attributes = payload["data"]["attributes"];

            Assert.Equal(data.IntProperty, attributes["int-property"].Value<int>());
            Assert.Equal(data.StringProperty, attributes["string-property"].Value<string>());
        }
    }
}
