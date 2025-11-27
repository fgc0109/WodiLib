// ========================================
// Project Name : WodiLib
// File Name    : DatabaseNamedDataRowJsonConverter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    [JsonConverter(typeof(DatabaseNamedDataRowJsonConverter))]
    public partial interface IDatabaseNamedDataRowSettings
    {
    }

    [JsonConverter(typeof(DatabaseNamedDataRowJsonConverter))]
    public partial record DatabaseNamedDataRowSettings
    {
    }

    [JsonConverter(typeof(DatabaseNamedDataRowJsonConverter))]
    public partial class DatabaseNamedDataRow
    {
    }

    [JsonConverter(typeof(DatabaseNamedDataRowJsonConverter))]
    public partial class FixedDatabaseNamedDataRow
    {
    }

    [JsonConverter(typeof(DatabaseNamedDataRowJsonConverter))]
    public partial class ReadOnlyDatabaseNamedDataRow
    {
    }

    /// <summary>
    ///     <see cref="IDatabaseNamedDataRowSettings"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseNamedDataRowJsonConverter : JsonConverter<IDatabaseNamedDataRowSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseNamedDataRowSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseNamedDataRowSettings Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var propertyNames = new PropertyNames(options);
            var properties = ReadProperties(ref reader, propertyNames, options, typeToConvert);

            var settings = CreateSettings(properties, propertyNames);
            return WrapSettings(settings, typeToConvert);
        }

        private PropertyValues ReadProperties(
            ref Utf8JsonReader reader,
            PropertyNames propertyNames,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var properties = new PropertyValues();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var propertyName = reader.GetString();
                if (propertyName is null)
                {
                    throw new JsonException();
                }

                reader.Read();

                var currentProperty = options.GetConvertedPropertyName(propertyName);
                if (currentProperty == propertyNames.DataName)
                {
                    properties.DataName = new DataNameJsonConverter().Read(
                        ref reader,
                        typeToConvert,
                        options
                    );
                }
                else if (currentProperty == propertyNames.Items)
                {
                    properties.Items = ReadItems(ref reader, options, typeToConvert);
                }
                else
                {
                    throw new JsonException($"予期しないプロパティです。(プロパティ名: {currentProperty})");
                }
            }

            return properties;
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

        private DatabaseNamedDataRowSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.DataName, propertyNames.DataName)
                || validator.IsNull(properties.Items, propertyNames.Items)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseNamedDataRowSettings(properties.Items)
            {
                DataName = properties.DataName,
            };
        }

        private IDatabaseNamedDataRowSettings WrapSettings(
            DatabaseNamedDataRowSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseNamedDataRow))
            {
                return new DatabaseNamedDataRow(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseNamedDataRow))
            {
                return (FixedDatabaseNamedDataRow)new DatabaseNamedDataRow(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseNamedDataRow))
            {
                return (ReadOnlyDatabaseNamedDataRow)new DatabaseNamedDataRow(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseNamedDataRowSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DataName)));
            new DataNameJsonConverter().Write(writer, value.DataName, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(ListConstant.JsonFieldNameItems));
            WriteItems(writer, value, options);

            writer.WriteEndObject();
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseNamedDataRowSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DatabaseFieldValueJsonConverter();

            writer.WriteStartArray();

            foreach (var item in value.Settings)
            {
                itemConverter.Write(writer, item, options);
            }

            writer.WriteEndArray();
        }

        #endregion

        private class PropertyNames
        {
            public string DataName { get; }
            public string Items { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                DataName = options.GetConvertedPropertyName(nameof(IDatabaseNamedDataRowSettings.DataName));
                Items = options.GetConvertedPropertyName(ListConstant.JsonFieldNameItems);
            }
        }

        private class PropertyValues
        {
            public DataName? DataName { get; set; }
            public List<DatabaseFieldValue>? Items { get; set; }
        }
    }
}
