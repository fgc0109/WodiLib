// ========================================
// Project Name : WodiLib
// File Name    : DBDataJsonConverter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    [JsonConverter(typeof(DBDataJsonConverter))]
    public partial interface IDBDataSettings
    {
    }

    [JsonConverter(typeof(DBDataJsonConverter))]
    public partial record DBDataSettings
    {
    }

    [JsonConverter(typeof(DBDataJsonConverter))]
    public partial class DBData
    {
    }

    [JsonConverter(typeof(DBDataJsonConverter))]
    public partial class ReadOnlyDBData
    {
    }

    /// <summary>
    ///     <see cref="DBData"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DBDataJsonConverter : JsonConverter<IDBDataSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDBDataSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDBDataSettings Read(
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
                if (currentProperty == propertyNames.DataTable)
                {
                    properties.DataTable = new DatabaseNamedDataTableJsonConverter().Read(
                        ref reader,
                        typeToConvert,
                        options
                    );
                }
                else
                {
                    throw new JsonException($"予期しないプロパティです。(プロパティ名: {currentProperty})");
                }
            }

            return properties;
        }

        private DBDataSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.DataTable, propertyNames.DataTable)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DBDataSettings
            {
                DataTable = properties.DataTable,
            };
        }

        private IDBDataSettings WrapSettings(
            DBDataSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DBData))
            {
                return new DBData(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDBData))
            {
                return (ReadOnlyDBData)new DBData(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, IDBDataSettings value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DataTable)));
            new DatabaseNamedDataTableJsonConverter().Write(writer, value.DataTable, options);

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string DataTable { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                DataTable = options.GetConvertedPropertyName(nameof(DBData.DataTable));
            }
        }

        private class PropertyValues
        {
            public IDatabaseNamedDataTableSettings? DataTable { get; set; }
        }
    }
}
