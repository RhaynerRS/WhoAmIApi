using System.Reflection;

namespace Richter.WhoAmIApi.CrossCutting.Extensions
{
    public static class TypeExtensions
    {
        public static bool TryGetCustomAttribute<TAttribute>(this Type type, out TAttribute attribute) where TAttribute : Attribute
        {
            attribute = (TAttribute)type.GetCustomAttributes(typeof(TAttribute)).FirstOrDefault()!;
            return attribute != null;
        }
    }
}