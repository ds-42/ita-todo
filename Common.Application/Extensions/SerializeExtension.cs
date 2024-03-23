using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Application.Extensions;

public static class SerializeExtension
{
    public static string JsonSerialize(this object value)
    {
        return JsonSerializer.Serialize(value, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });
    }

}
