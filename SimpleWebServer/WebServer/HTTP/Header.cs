namespace HTTP
{
    /// <summary>
    /// Represet an Http Header information ,consisting of <c>Name</c> and <c>Value</c>
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Initializes a new<see cref="Header"/> class.
        /// </summary>
        /// <param name="name">Header name</param>
        /// <param name="value">Header value</param>
        public Header(string name, string value)
        {
            this.Name = name;
            this.Value = value;

        }
        public string Name { get; set; }

        public string Value { get; set; }

       
        public override string ToString()
        {
            return $"{this.Name}: {this.Value}";    
        }
    }
}