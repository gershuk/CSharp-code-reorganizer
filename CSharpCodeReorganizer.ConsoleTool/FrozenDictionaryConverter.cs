using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharpCodeReorganizer.ConsoleTool;

public class FrozenDictionaryConverter<TKey, TValue> : JsonConverter<FrozenDictionary<TKey, TValue>> where TKey : notnull
{
    public override FrozenDictionary<TKey, TValue> Read(ref Utf8JsonReader reader,
                                                        Type typeToConvert,
                                                        JsonSerializerOptions options)
    {
        var dictionary = JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(ref reader, options);
        return dictionary!.ToFrozenDictionary();
    }

    public override void Write(Utf8JsonWriter writer,
                               FrozenDictionary<TKey, TValue> value,
                               JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
