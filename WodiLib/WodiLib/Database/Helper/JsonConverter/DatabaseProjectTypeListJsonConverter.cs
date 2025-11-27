// ========================================
// Project Name : WodiLib
// File Name    : DatabaseProjectTypeListJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseProjectTypeListJsonConverter))]
    public partial interface IDatabaseProjectTypeListSettings
    {
    }

    [JsonConverter(typeof(DatabaseProjectTypeListJsonConverter))]
    public partial record DatabaseProjectTypeListSettings
    {
    }

    [JsonConverter(typeof(DatabaseProjectTypeListJsonConverter))]
    public partial class DatabaseProjectTypeList
    {
    }

    [JsonConverter(typeof(DatabaseProjectTypeListJsonConverter))]
    public partial class FixedDatabaseProjectTypeList
    {
    }

    [JsonConverter(typeof(DatabaseProjectTypeListJsonConverter))]
    public partial class ReadOnlyDatabaseProjectTypeList
    {
    }

    /// <summary>
    ///     <see cref="DatabaseProjectTypeList"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseProjectTypeListJsonConverter : JsonConverter<IDatabaseProjectTypeListSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseProjectTypeListSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseProjectTypeListSettings Read(
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

        private List<IDatabaseProjectTypeSettings> ReadItems(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var items = new List<IDatabaseProjectTypeSettings>();
            var itemConverter = new DatabaseProjectTypeJsonConverter();

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

        private DatabaseProjectTypeListSettings CreateSettings(
            List<IDatabaseProjectTypeSettings> items
        )
        {
            return new DatabaseProjectTypeListSettings(items);
        }

        private IDatabaseProjectTypeListSettings WrapSettings(
            DatabaseProjectTypeListSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseProjectTypeList))
            {
                return new DatabaseProjectTypeList(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseProjectTypeList))
            {
                return (FixedDatabaseProjectTypeList)new DatabaseProjectTypeList(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseProjectTypeList))
            {
                return (ReadOnlyDatabaseProjectTypeList)new DatabaseProjectTypeList(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseProjectTypeListSettings value,
            JsonSerializerOptions options
        )
        {
            WriteItems(writer, value, options);
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseProjectTypeListSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DatabaseProjectTypeJsonConverter();

            writer.WriteStartArray();

            foreach (var item in value.Settings)
            {
                itemConverter.Write(writer, item, options);
            }

            writer.WriteEndArray();
        }

        #endregion
    }
}
