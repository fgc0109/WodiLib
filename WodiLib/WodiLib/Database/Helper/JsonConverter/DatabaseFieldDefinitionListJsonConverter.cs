// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldDefinitionListJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseFieldDefinitionListJsonConverter))]
    public partial interface IDatabaseFieldDefinitionListSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldDefinitionListJsonConverter))]
    public partial record DatabaseFieldDefinitionListSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldDefinitionListJsonConverter))]
    public partial class DatabaseFieldDefinitionList
    {
    }

    [JsonConverter(typeof(DatabaseFieldDefinitionListJsonConverter))]
    public partial class FixedDatabaseFieldDefinitionList
    {
    }

    [JsonConverter(typeof(DatabaseFieldDefinitionListJsonConverter))]
    public partial class ReadOnlyDatabaseFieldDefinitionList
    {
    }

    /// <summary>
    ///     <see cref="IDatabaseFieldDefinitionListSettings"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseFieldDefinitionListJsonConverter : JsonConverter<IDatabaseFieldDefinitionListSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseFieldDefinitionListSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseFieldDefinitionListSettings Read(
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

        private List<IDatabaseFieldDefinitionSettings> ReadItems(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var items = new List<IDatabaseFieldDefinitionSettings>();
            var itemConverter = new DatabaseFieldDefinitionJsonConverter();

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

        private DatabaseFieldDefinitionListSettings CreateSettings(List<IDatabaseFieldDefinitionSettings> items)
        {
            return new DatabaseFieldDefinitionListSettings(items);
        }

        private IDatabaseFieldDefinitionListSettings WrapSettings(
            DatabaseFieldDefinitionListSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseFieldDefinitionList))
            {
                return new DatabaseFieldDefinitionList(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseFieldDefinitionList))
            {
                return (FixedDatabaseFieldDefinitionList)new DatabaseFieldDefinitionList(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseFieldDefinitionList))
            {
                return (ReadOnlyDatabaseFieldDefinitionList)new DatabaseFieldDefinitionList(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseFieldDefinitionListSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartArray();

            WriteItems(writer, value, options);

            writer.WriteEndArray();
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseFieldDefinitionListSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DatabaseFieldDefinitionJsonConverter();

            foreach (var item in value.Settings)
            {
                itemConverter.Write(writer, item, options);
            }
        }

        #endregion
    }
}
