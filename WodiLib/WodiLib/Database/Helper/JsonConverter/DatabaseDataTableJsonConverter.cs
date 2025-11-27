// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataTableJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseDataTableJsonConverter))]
    public partial interface IDatabaseDataTableSettings
    {
    }

    [JsonConverter(typeof(DatabaseDataTableJsonConverter))]
    public partial record DatabaseDataTableSettings
    {
    }

    [JsonConverter(typeof(DatabaseDataTableJsonConverter))]
    public partial class DatabaseDataTable
    {
    }

    [JsonConverter(typeof(DatabaseDataTableJsonConverter))]
    public partial class FixedDatabaseDataTable
    {
    }

    [JsonConverter(typeof(DatabaseDataTableJsonConverter))]
    public partial class ReadOnlyDatabaseDataTable
    {
    }

    /// <summary>
    ///     <see cref="IDatabaseDataTableSettings"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseDataTableJsonConverter : JsonConverter<IDatabaseDataTableSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseDataTableSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseDataTableSettings Read(
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

        private List<IDatabaseDataRowSettings> ReadItems(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var items = new List<IDatabaseDataRowSettings>();
            var itemConverter = new DatabaseDataRowJsonConverter();

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

        private DatabaseDataTableSettings CreateSettings(List<IDatabaseDataRowSettings> items)
        {
            return new DatabaseDataTableSettings(items);
        }

        private IDatabaseDataTableSettings WrapSettings(
            DatabaseDataTableSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseDataTable))
            {
                return new DatabaseDataTable(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseDataTable))
            {
                return (FixedDatabaseDataTable)new DatabaseDataTable(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseDataTable))
            {
                return (ReadOnlyDatabaseDataTable)new DatabaseDataTable(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseDataTableSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartArray();

            WriteItems(writer, value, options);

            writer.WriteEndArray();
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseDataTableSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DatabaseDataRowJsonConverter();

            foreach (var item in value.Settings)
            {
                itemConverter.Write(writer, item, options);
            }
        }

        #endregion
    }
}
