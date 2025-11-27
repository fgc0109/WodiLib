// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldMetadataJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseFieldMetadataJsonConverter))]
    public partial interface IDatabaseFieldMetadataSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldMetadataJsonConverter))]
    public partial record DatabaseFieldMetadataSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldMetadataJsonConverter))]
    public partial class DatabaseFieldMetadata
    {
    }

    [JsonConverter(typeof(DatabaseFieldMetadataJsonConverter))]
    public partial class ReadOnlyDatabaseFieldMetadata
    {
    }

    /// <summary>
    ///     <see cref="DatabaseFieldMetadata"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseFieldMetadataJsonConverter : JsonConverter<IDatabaseFieldMetadataSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseFieldMetadataSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseFieldMetadataSettings Read(
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
                if (currentProperty == propertyNames.FieldName)
                {
                    properties.FieldName = new FieldNameJsonConverter().Read(
                        ref reader,
                        typeToConvert,
                        options
                    );
                }
                else if (currentProperty == propertyNames.SpecialSettingDefinition)
                {
                    properties.SpecialSettingDefinition =
                        new DatabaseFieldSpecialSettingDefinitionJsonConverter().Read(
                            ref reader,
                            typeToConvert,
                            options
                        );
                }
                else if (currentProperty == propertyNames.FieldMemo)
                {
                    properties.FieldMemo = new FieldMemoJsonConverter().Read(
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

        private DatabaseFieldMetadataSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.FieldName, propertyNames.FieldName)
                || validator.IsNull(properties.SpecialSettingDefinition, propertyNames.SpecialSettingDefinition)
                || validator.IsNull(properties.FieldMemo, propertyNames.FieldMemo)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseFieldMetadataSettings
            {
                FieldName = properties.FieldName,
                SpecialSettingDefinition = properties.SpecialSettingDefinition,
                FieldMemo = properties.FieldMemo,
            };
        }

        private IDatabaseFieldMetadataSettings WrapSettings(
            DatabaseFieldMetadataSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseFieldMetadata))
            {
                return new DatabaseFieldMetadata(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseFieldMetadata))
            {
                return (ReadOnlyDatabaseFieldMetadata)new DatabaseFieldMetadata(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseFieldMetadataSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.FieldName)));
            new FieldNameJsonConverter().Write(writer, value.FieldName, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.SpecialSettingDefinition)));
            new DatabaseFieldSpecialSettingDefinitionJsonConverter().Write(
                writer,
                value.SpecialSettingDefinition,
                options
            );

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.FieldMemo)));
            new FieldMemoJsonConverter().Write(writer, value.FieldMemo, options);

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string FieldName { get; }
            public string SpecialSettingDefinition { get; }
            public string FieldMemo { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                FieldName = options.GetConvertedPropertyName(nameof(DatabaseFieldMetadata.FieldName));
                SpecialSettingDefinition = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldMetadata.SpecialSettingDefinition)
                );
                FieldMemo = options.GetConvertedPropertyName(nameof(DatabaseFieldMetadata.FieldMemo));
            }
        }

        private class PropertyValues
        {
            public FieldName? FieldName { get; set; }
            public DatabaseFieldType? FieldType { get; set; }
            public IDatabaseFieldSpecialSettingDefinitionSettings? SpecialSettingDefinition { get; set; }
            public FieldMemo? FieldMemo { get; set; }
        }
    }
}
