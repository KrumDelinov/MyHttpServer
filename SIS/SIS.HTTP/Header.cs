
namespace SIS.HTTP
{
    public class Header
    {
        //for gitHub
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
