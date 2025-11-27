// ========================================
// Project Name : WodiLib
// File Name    : DatabaseValueCaseListJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseValueCaseListJsonConverter))]
    public partial interface IDatabaseValueCaseListSettings
    {
    }

    [JsonConverter(typeof(DatabaseValueCaseListJsonConverter))]
    public partial record DatabaseValueCaseListSettings
    {
    }

    [JsonConverter(typeof(DatabaseValueCaseListJsonConverter))]
    public partial class DatabaseValueCaseList
    {
    }

    [JsonConverter(typeof(DatabaseValueCaseListJsonConverter))]
    public partial class FixedDatabaseValueCaseList
    {
    }

    [JsonConverter(typeof(DatabaseValueCaseListJsonConverter))]
    public partial class ReadOnlyDatabaseValueCaseList
    {
    }

    /// <summary>
    ///     <see cref="DatabaseValueCaseList"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseValueCaseListJsonConverter : JsonConverter<IDatabaseValueCaseListSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseValueCaseListSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseValueCaseListSettings Read(
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

        private List<DatabaseValueCase> ReadItems(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var items = new List<DatabaseValueCase>();
            var itemConverter = new DatabaseValueCaseJsonConverter();

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

        private DatabaseValueCaseListSettings CreateSettings(
            List<DatabaseValueCase> items
        )
        {
            return new DatabaseValueCaseListSettings(items);
        }

        private IDatabaseValueCaseListSettings WrapSettings(
            DatabaseValueCaseListSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseValueCaseList))
            {
                return new DatabaseValueCaseList(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseValueCaseList))
            {
                return (FixedDatabaseValueCaseList)new DatabaseValueCaseList(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseValueCaseList))
            {
                return (ReadOnlyDatabaseValueCaseList)new DatabaseValueCaseList(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseValueCaseListSettings value,
            JsonSerializerOptions options
        )
        {
            WriteItems(writer, value, options);
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseValueCaseListSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DatabaseValueCaseJsonConverter();

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
