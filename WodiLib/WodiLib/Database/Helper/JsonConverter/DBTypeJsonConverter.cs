// ========================================
// Project Name : WodiLib
// File Name    : DBTypeJsonConverter.cs
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
    [JsonConverter(typeof(DBTypeJsonConverter))]
    public partial interface IDBTypeSettings
    {
    }

    [JsonConverter(typeof(DBTypeJsonConverter))]
    public partial record DBTypeSettings
    {
    }

    [JsonConverter(typeof(DBTypeJsonConverter))]
    public partial class DBType
    {
    }

    [JsonConverter(typeof(DBTypeJsonConverter))]
    public partial class ReadOnlyDBType
    {
    }

    /// <summary>
    ///     <see cref="DBType"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DBTypeJsonConverter : JsonConverter<IDBTypeSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDBTypeSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDBTypeSettings Read(
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
                if (currentProperty == propertyNames.TypeMetadataTable)
                {
                    properties.TypeMetadataTable = new DatabaseTypeMetadataTableJsonConverter().Read(
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

        private DBTypeSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.TypeMetadataTable, propertyNames.TypeMetadataTable)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DBTypeSettings
            {
                TypeMetadataTable = properties.TypeMetadataTable,
            };
        }

        private IDBTypeSettings WrapSettings(
            DBTypeSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DBType))
            {
                return new DBType(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDBType))
            {
                return (ReadOnlyDBType)new DBType(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, IDBTypeSettings value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.TypeMetadataTable)));
            new DatabaseTypeMetadataTableJsonConverter().Write(writer, value.TypeMetadataTable, options);

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string TypeMetadataTable { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                TypeMetadataTable = options.GetConvertedPropertyName(nameof(DBType.TypeMetadataTable));
            }
        }

        private class PropertyValues
        {
            public IDatabaseTypeMetadataTableSettings? TypeMetadataTable { get; set; }
        }
    }
}
