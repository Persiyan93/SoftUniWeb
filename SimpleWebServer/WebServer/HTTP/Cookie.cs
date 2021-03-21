namespace HTTP
{
    public class Cookie
    {
        public string Name { get; set; }

        public string  Value { get; set; }
        public Cookie(string name ,string value)
        {
            this.Name = name;
            this.Value = value;
         
        }
    }
}