// ========================================
// Project Name : WodiLib
// File Name    : DatabaseNamedDataTableJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseNamedDataTableJsonConverter))]
    public partial interface IDatabaseNamedDataTableSettings
    {
    }

    [JsonConverter(typeof(DatabaseNamedDataTableJsonConverter))]
    public partial record DatabaseNamedDataTableSettings
    {
    }

    [JsonConverter(typeof(DatabaseNamedDataTableJsonConverter))]
    public partial class DatabaseNamedDataTable
    {
    }

    [JsonConverter(typeof(DatabaseNamedDataTableJsonConverter))]
    public partial class FixedDatabaseNamedDataTable
    {
    }

    [JsonConverter(typeof(DatabaseNamedDataTableJsonConverter))]
    public partial class ReadOnlyDatabaseNamedDataTable
    {
    }

    /// <summary>
    ///     <see cref="IDatabaseNamedDataTableSettings"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseNamedDataTableJsonConverter : JsonConverter<IDatabaseNamedDataTableSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseNamedDataTableSettings).IsAssignableFrom(typeToConvert);
        }

        /// <inheritdoc/>
        public override IDatabaseNamedDataTableSettings Read(
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

        private List<IDatabaseNamedDataRowSettings> ReadItems(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var items = new List<IDatabaseNamedDataRowSettings>();
            var itemConverter = new DatabaseNamedDataRowJsonConverter();

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

        private DatabaseNamedDataTableSettings CreateSettings(List<IDatabaseNamedDataRowSettings> items)
        {
            return new DatabaseNamedDataTableSettings(items);
        }

        private IDatabaseNamedDataTableSettings WrapSettings(
            DatabaseNamedDataTableSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseNamedDataTable))
            {
                return new DatabaseNamedDataTable(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseNamedDataTable))
            {
                return (FixedDatabaseNamedDataTable)new DatabaseNamedDataTable(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseNamedDataTable))
            {
                return (ReadOnlyDatabaseNamedDataTable)new DatabaseNamedDataTable(settings);
            }

            return settings;
        }

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseNamedDataTableSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartArray();

            WriteItems(writer, value, options);

            writer.WriteEndArray();
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseNamedDataTableSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DatabaseNamedDataRowJsonConverter();

            foreach (var item in value.Settings)
            {
                itemConverter.Write(writer, item, options);
            }
        }
    }
}
