using Partytime;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact(DisplayName = "Serialize simple object")]
        public void SerializeObject()
        {
            var serializer = new Serializer();
            var simpleObject = new Simple();

            var json = serializer.Serialize(simpleObject);

            Assert.NotNull(json);
        }
    }
}
