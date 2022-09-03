using System.Text;
using Newtonsoft.Json;

namespace PostService.Extensions;

public static class ByteReadOnlyMemoryExtensions
{
    public static T? ToModel<T>(this ReadOnlyMemory<byte> memory)
    {
        var body = memory.ToArray();
        var message = Encoding.UTF8.GetString(body);
        return JsonConvert.DeserializeObject<T>(message);
    }
}