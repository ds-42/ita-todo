using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.BL.Extensions;

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
