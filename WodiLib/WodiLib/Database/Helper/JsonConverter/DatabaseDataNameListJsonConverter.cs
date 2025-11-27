// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataNameListJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseDataNameListJsonConverter))]
    public partial interface IDatabaseDataNameListSettings
    {
    }

    [JsonConverter(typeof(DatabaseDataNameListJsonConverter))]
    public partial record DatabaseDataNameListSettings
    {
    }

    [JsonConverter(typeof(DatabaseDataNameListJsonConverter))]
    public partial class DatabaseDataNameList
    {
    }

    [JsonConverter(typeof(DatabaseDataNameListJsonConverter))]
    public partial class FixedDatabaseDataNameList
    {
    }

    [JsonConverter(typeof(DatabaseDataNameListJsonConverter))]
    public partial class ReadOnlyDatabaseDataNameList
    {
    }

    /// <summary>
    ///     <see cref="IDatabaseDataNameListSettings"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseDataNameListJsonConverter : JsonConverter<IDatabaseDataNameListSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseDataNameListSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseDataNameListSettings Read(
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

        private List<DataName> ReadItems(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var items = new List<DataName>();
            var itemConverter = new DataNameJsonConverter();

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

        private DatabaseDataNameListSettings CreateSettings(List<DataName> items)
        {
            return new DatabaseDataNameListSettings(items);
        }

        private IDatabaseDataNameListSettings WrapSettings(
            DatabaseDataNameListSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseDataNameList))
            {
                return new DatabaseDataNameList(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseDataNameList))
            {
                return (FixedDatabaseDataNameList)new DatabaseDataNameList(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseDataNameList))
            {
                return (ReadOnlyDatabaseDataNameList)new DatabaseDataNameList(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseDataNameListSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartArray();

            WriteItems(writer, value, options);

            writer.WriteEndArray();
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseDataNameListSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DataNameJsonConverter();

            foreach (var item in value.Settings)
            {
                itemConverter.Write(writer, item, options);
            }
        }

        #endregion
    }
}
