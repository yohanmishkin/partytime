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

        [Fact(DisplayName = "Payload data")]
        public void SerializeObject()
        {
            var simpleObject = new Simple();

            var result = _serializer.Serialize(simpleObject);
            var payload = JObject.Parse(result);

            Assert.NotNull(payload["data"]);
        }
    }
}
