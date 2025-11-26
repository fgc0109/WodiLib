using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WodiLib.Cmn
{
    [JsonConverter(typeof(VariableAddressJsonConverter))]
    public abstract partial record VariableAddress
    {
    }

    /// <summary>
    ///     <see cref="VariableAddress"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class VariableAddressJsonConverter : JsonConverter<VariableAddress>
    {
        /// <inheritdoc/>
        public override VariableAddress Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            var value = reader.GetInt32()!;
            return VariableAddressFactory.Create(value);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, VariableAddress value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.RawValue);
        }
    }
}
