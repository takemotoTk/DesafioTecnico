using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace VaccinationCardManagement.Application.ExtensionMethods;

public static class MappingExtension
{    
    #region Serializer config
    private static JsonSerializerOptions JsonSerializerSettings()
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };
        return jsonSerializerOptions;
    }

    private static string SerializerObjectToJson(object obj, JsonSerializerOptions? serializerOptions = null)
    {
        var convertObjToJson = JsonSerializer.Serialize(obj, serializerOptions ?? JsonSerializerSettings());
        return convertObjToJson;
    }

    private static T DeserializerJsonToObject<T>(string obj, JsonSerializerOptions? serializerOptions = null)
    {
        var convertJsonToObj = JsonSerializer.Deserialize<T>(obj, serializerOptions ?? JsonSerializerSettings());
        return convertJsonToObj;
    }
    #endregion

    public static T Map<T>(this object obj)
    {
        object objCurrent = null;
        try
        {
            object response = null;
            if (obj != null)
            {
                var typeObj = typeof(T);
                var fullName = typeObj.FullName;
                Assembly assem = typeObj.Assembly;

                dynamic convertJsonToObj = null;
                var convertToJson = SerializerObjectToJson(obj);
                if (!string.IsNullOrEmpty(convertToJson))
                {
                    //objCurrent = assem.CreateInstance(fullName);
                    convertJsonToObj = DeserializerJsonToObject<T>(convertToJson);
                    if (convertJsonToObj != null)
                    {
                        objCurrent = convertJsonToObj;
                    }

                    response = (T)Convert.ChangeType(objCurrent, typeof(T));
                }
            }

            return (T)response;
        }
        catch (InvalidCastException)
        {
            return default(T);
        }
    }

    public static T MappingEntityLinq<T>(this IQueryable<object> obj)
    {
        return Map<T>(obj);
    }
}
