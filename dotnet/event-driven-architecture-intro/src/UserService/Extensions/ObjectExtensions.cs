using System.Text;
using Newtonsoft.Json;

namespace UserService.Extensions;

public static class ObjectExtensions
{
    public static byte[] ToBytes(this object o) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o));
}