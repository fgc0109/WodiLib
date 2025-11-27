// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldMetadataListJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseFieldMetadataListJsonConverter))]
    public partial interface IDatabaseFieldMetadataListSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldMetadataListJsonConverter))]
    public partial record DatabaseFieldMetadataListSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldMetadataListJsonConverter))]
    public partial class DatabaseFieldMetadataList
    {
    }

    [JsonConverter(typeof(DatabaseFieldMetadataListJsonConverter))]
    public partial class FixedDatabaseFieldMetadataList
    {
    }

    [JsonConverter(typeof(DatabaseFieldMetadataListJsonConverter))]
    public partial class ReadOnlyDatabaseFieldMetadataList
    {
    }

    /// <summary>
    ///     <see cref="IDatabaseFieldMetadataListSettings"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseFieldMetadataListJsonConverter : JsonConverter<IDatabaseFieldMetadataListSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseFieldMetadataListSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseFieldMetadataListSettings Read(
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

        private List<IDatabaseFieldMetadataSettings> ReadItems(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var items = new List<IDatabaseFieldMetadataSettings>();
            var itemConverter = new DatabaseFieldMetadataJsonConverter();

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

        private DatabaseFieldMetadataListSettings CreateSettings(List<IDatabaseFieldMetadataSettings> items)
        {
            return new DatabaseFieldMetadataListSettings(items);
        }

        private IDatabaseFieldMetadataListSettings WrapSettings(
            DatabaseFieldMetadataListSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseFieldMetadataList))
            {
                return new DatabaseFieldMetadataList(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseFieldMetadataList))
            {
                return (FixedDatabaseFieldMetadataList)new DatabaseFieldMetadataList(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseFieldMetadataList))
            {
                return (ReadOnlyDatabaseFieldMetadataList)new DatabaseFieldMetadataList(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseFieldMetadataListSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartArray();

            WriteItems(writer, value, options);

            writer.WriteEndArray();
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseFieldMetadataListSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DatabaseFieldMetadataJsonConverter();

            foreach (var item in value.Settings)
            {
                itemConverter.Write(writer, item, options);
            }
        }

        #endregion
    }
}
