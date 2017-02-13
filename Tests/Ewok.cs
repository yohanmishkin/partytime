namespace Tests
{
    public class Ewok
    {
        public Ewok()
        {
        }

        public int Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        //public Address Address { get; set; }
    }

    public class Address
    {
        public int Hut { get; set; }
        public string TreeVillage { get; set; }
    }
}