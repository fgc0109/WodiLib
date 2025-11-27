// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeTableListJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseTypeTableListJsonConverter))]
    public partial interface IDatabaseTypeTableListSettings
    {
    }

    [JsonConverter(typeof(DatabaseTypeTableListJsonConverter))]
    public partial record DatabaseTypeTableListSettings
    {
    }

    [JsonConverter(typeof(DatabaseTypeTableListJsonConverter))]
    public partial class DatabaseTypeTableList
    {
    }

    [JsonConverter(typeof(DatabaseTypeTableListJsonConverter))]
    public partial class FixedDatabaseTypeTableList
    {
    }

    [JsonConverter(typeof(DatabaseTypeTableListJsonConverter))]
    public partial class ReadOnlyDatabaseTypeTableList
    {
    }

    /// <summary>
    ///     <see cref="DatabaseTypeTableList"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseTypeTableListJsonConverter : JsonConverter<IDatabaseTypeTableListSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseTypeTableListSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseTypeTableListSettings Read(
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

        private List<IDatabaseTypeTableSettings> ReadItems(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var items = new List<IDatabaseTypeTableSettings>();
            var itemConverter = new DatabaseTypeTableJsonConverter();

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

        private DatabaseTypeTableListSettings CreateSettings(
            List<IDatabaseTypeTableSettings> items
        )
        {
            return new DatabaseTypeTableListSettings(items);
        }

        private IDatabaseTypeTableListSettings WrapSettings(
            DatabaseTypeTableListSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseTypeTableList))
            {
                return new DatabaseTypeTableList(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseTypeTableList))
            {
                return (FixedDatabaseTypeTableList)new DatabaseTypeTableList(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseTypeTableList))
            {
                return (ReadOnlyDatabaseTypeTableList)new DatabaseTypeTableList(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseTypeTableListSettings value,
            JsonSerializerOptions options
        )
        {
            WriteItems(writer, value, options);
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseTypeTableListSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DatabaseTypeTableJsonConverter();

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
