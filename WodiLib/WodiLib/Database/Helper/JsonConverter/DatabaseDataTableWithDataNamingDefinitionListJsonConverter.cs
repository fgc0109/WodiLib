// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataTableWithDataNamingDefinitionListJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseDataTableWithDataNamingDefinitionListJsonConverter))]
    public partial interface IDatabaseDataTableWithDataNamingDefinitionListSettings
    {
    }

    [JsonConverter(typeof(DatabaseDataTableWithDataNamingDefinitionListJsonConverter))]
    public partial record DatabaseDataTableWithDataNamingDefinitionListSettings
    {
    }

    [JsonConverter(typeof(DatabaseDataTableWithDataNamingDefinitionListJsonConverter))]
    public partial class DatabaseDataTableWithDataNamingDefinitionList
    {
    }

    [JsonConverter(typeof(DatabaseDataTableWithDataNamingDefinitionListJsonConverter))]
    public partial class FixedDatabaseDataTableWithDataNamingDefinitionList
    {
    }

    [JsonConverter(typeof(DatabaseDataTableWithDataNamingDefinitionListJsonConverter))]
    public partial class ReadOnlyDatabaseDataTableWithDataNamingDefinitionList
    {
    }

    /// <summary>
    ///     <see cref="IDatabaseDataTableWithDataNamingDefinitionListSettings"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseDataTableWithDataNamingDefinitionListJsonConverter :
        JsonConverter<IDatabaseDataTableWithDataNamingDefinitionListSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseDataTableWithDataNamingDefinitionListSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseDataTableWithDataNamingDefinitionListSettings Read(
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

        private List<IDatabaseDataTableWithDataNamingDefinitionSettings> ReadItems(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var items = new List<IDatabaseDataTableWithDataNamingDefinitionSettings>();
            var itemConverter = new DatabaseDataTableWithDataNamingDefinitionJsonConverter();

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

        private DatabaseDataTableWithDataNamingDefinitionListSettings CreateSettings(
            List<IDatabaseDataTableWithDataNamingDefinitionSettings> items
        )
        {
            return new DatabaseDataTableWithDataNamingDefinitionListSettings(items);
        }

        private IDatabaseDataTableWithDataNamingDefinitionListSettings WrapSettings(
            DatabaseDataTableWithDataNamingDefinitionListSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseDataTableWithDataNamingDefinitionList))
            {
                return new DatabaseDataTableWithDataNamingDefinitionList(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseDataTableWithDataNamingDefinitionList))
            {
                return (FixedDatabaseDataTableWithDataNamingDefinitionList)
                    new DatabaseDataTableWithDataNamingDefinitionList(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseDataTableWithDataNamingDefinitionList))
            {
                return (ReadOnlyDatabaseDataTableWithDataNamingDefinitionList)
                    new DatabaseDataTableWithDataNamingDefinitionList(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseDataTableWithDataNamingDefinitionListSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartArray();

            WriteItems(writer, value, options);

            writer.WriteEndArray();
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseDataTableWithDataNamingDefinitionListSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DatabaseDataTableWithDataNamingDefinitionJsonConverter();

            foreach (var item in value.Settings)
            {
                itemConverter.Write(writer, item, options);
            }
        }

        #endregion
    }
}
