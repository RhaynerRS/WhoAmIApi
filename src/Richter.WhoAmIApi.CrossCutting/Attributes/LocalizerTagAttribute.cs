namespace Richter.WhoAmIApi.CrossCutting.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LocalizerTagAttribute(string tag) : Attribute
    {
        public string Tag { get; set; } = tag;
        public string[] ArgumentKeys { get; set; } = [];

        public bool HasArguments => ArgumentKeys.Length != 0;

        public LocalizerTagAttribute(string tag, params string[] argumentKeys) : this(tag)
        {
            ArgumentKeys = argumentKeys;
        }
    }
}