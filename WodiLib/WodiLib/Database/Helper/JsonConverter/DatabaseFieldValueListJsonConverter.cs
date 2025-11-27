// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldValueListJsonConverter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WodiLib.Database
{
    [JsonConverter(typeof(DatabaseFieldValueListJsonConverter))]
    public partial interface IDatabaseFieldValueListSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldValueListJsonConverter))]
    public partial record DatabaseFieldValueListSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldValueListJsonConverter))]
    public partial class DatabaseFieldValueList
    {
    }

    [JsonConverter(typeof(DatabaseFieldValueListJsonConverter))]
    public partial class FixedDatabaseFieldValueList
    {
    }

    [JsonConverter(typeof(DatabaseFieldValueListJsonConverter))]
    public partial class ReadOnlyDatabaseFieldValueList
    {
    }

    /// <summary>
    ///     <see cref="IDatabaseFieldValueListSettings"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseFieldValueListJsonConverter : JsonConverter<IDatabaseFieldValueListSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseFieldValueListSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseFieldValueListSettings Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            var items = ReadItems(ref reader, options, typeToConvert);
            var settings = CreateSettings(items);
            return WrapSettings(settings, typeToConvert);
        }

        private List<DatabaseFieldValue> ReadItems(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var items = new List<DatabaseFieldValue>();
            var itemConverter = new DatabaseFieldValueJsonConverter();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                var item = itemConverter.Read(ref reader, typeToConvert, options);
                if (item is null)
                {
                    throw new JsonException("null 要素は許可されていません。");
                }

                items.Add(item);
            }

            return items;
        }

        private DatabaseFieldValueListSettings CreateSettings(List<DatabaseFieldValue> items)
        {
            var fieldType = items.Count > 0
                ? items[0].Type
                : DatabaseFieldType.Int;
            return new DatabaseFieldValueListSettings(items)
            {
                FieldType = fieldType,
            };
        }

        private IDatabaseFieldValueListSettings WrapSettings(
            DatabaseFieldValueListSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseFieldValueList))
            {
                return new DatabaseFieldValueList(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseFieldValueList))
            {
                return (FixedDatabaseFieldValueList)new DatabaseFieldValueList(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseFieldValueList))
            {
                return (ReadOnlyDatabaseFieldValueList)new DatabaseFieldValueList(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseFieldValueListSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartArray();

            WriteItems(writer, value, options);

            writer.WriteEndArray();
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseFieldValueListSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DatabaseFieldValueJsonConverter();

            foreach (var item in value.Settings)
            {
                itemConverter.Write(writer, item, options);
            }
        }

        #endregion
    }
}
