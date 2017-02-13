namespace Tests
{
    class Parent
    {
        public int Id = 1;
        public Kid Child { get; set; }
    }

    class Kid
    {
        public int Id = 2;
        public Parent Parent { get; set; }
        public GrandKid Child { get; set; }
    }

    class GrandKid
    {
        public int Id = 3;
        public Kid Parent { get; set; }
    }
}
