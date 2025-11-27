// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataRowJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseDataRowJsonConverter))]
    public partial interface IDatabaseDataRowSettings
    {
    }

    [JsonConverter(typeof(DatabaseDataRowJsonConverter))]
    public partial record DatabaseDataRowSettings
    {
    }

    [JsonConverter(typeof(DatabaseDataRowJsonConverter))]
    public partial class DatabaseDataRow
    {
    }

    [JsonConverter(typeof(DatabaseDataRowJsonConverter))]
    public partial class FixedDatabaseDataRow
    {
    }

    [JsonConverter(typeof(DatabaseDataRowJsonConverter))]
    public partial class ReadOnlyDatabaseDataRow
    {
    }

    /// <summary>
    ///     <see cref="IDatabaseDataRowSettings"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseDataRowJsonConverter : JsonConverter<IDatabaseDataRowSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseDataRowSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseDataRowSettings Read(
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

        private DatabaseDataRowSettings CreateSettings(List<DatabaseFieldValue> items)
        {
            return new DatabaseDataRowSettings(items);
        }

        private IDatabaseDataRowSettings WrapSettings(
            DatabaseDataRowSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseDataRow))
            {
                return new DatabaseDataRow(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseDataRow))
            {
                return (FixedDatabaseDataRow)new DatabaseDataRow(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseDataRow))
            {
                return (ReadOnlyDatabaseDataRow)new DatabaseDataRow(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseDataRowSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartArray();

            WriteItems(writer, value, options);

            writer.WriteEndArray();
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseDataRowSettings value,
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
