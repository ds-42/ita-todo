using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Users.Services.Utils;

public class Utils
{
    public static string SerializeObject(object value) 
    {
        return JsonSerializer.Serialize(value, new JsonSerializerOptions() 
        { 
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });
    }

}
