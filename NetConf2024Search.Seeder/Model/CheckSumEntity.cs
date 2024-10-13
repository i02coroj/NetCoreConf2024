using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace NetConf2024Search.Seeder.Model;

public abstract class CheckSumEntity
{
    public string GetChecksum()
    {
        var types = new Type[9]
        {
            typeof(string),
            typeof(bool),
            typeof(bool?),
            typeof(int),
            typeof(int?),
            typeof(Guid),
            typeof(Guid?),
            typeof(DateTimeOffset),
            typeof(DateTimeOffset?)
        };
        IEnumerable<PropertyInfo> source = from p in GetType().GetProperties()
                                           where types.Contains(p.PropertyType)
                                           select p;
        string s = string.Join(string.Empty, source.Select((PropertyInfo p) => p.GetValue(this)?.ToString() ?? string.Empty));
        byte[] inArray = SHA256.HashData(Encoding.UTF8.GetBytes(s));
        return Convert.ToBase64String(inArray);
    }
}
