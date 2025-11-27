// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldValueJsonConverter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WodiLib.Database
{
    [JsonConverter(typeof(DatabaseFieldValueJsonConverter))]
    public partial class DatabaseFieldValue
    {
    }

    /// <summary>
    ///     <see cref="DatabaseFieldValue"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseFieldValueJsonConverter : JsonConverter<DatabaseFieldValue>
    {
        /// <inheritdoc/>
        public override DatabaseFieldValue? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => new DatabaseFieldValue(reader.GetInt32()),
                JsonTokenType.String => new DatabaseFieldValue(reader.GetString()!),
                JsonTokenType.Null => null,
                _ => throw new JsonException(),
            };
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DatabaseFieldValue? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            if (value.Type == DatabaseFieldType.Int)
            {
                writer.WriteNumberValue(value.IntValue.RawValue);
            }
            else
            {
                writer.WriteStringValue(value.StringValue.RawValue);
            }
        }
    }
}
