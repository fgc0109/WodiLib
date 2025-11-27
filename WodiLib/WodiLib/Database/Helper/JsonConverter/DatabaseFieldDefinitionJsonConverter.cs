// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldDefinitionJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseFieldDefinitionJsonConverter))]
    public partial interface IDatabaseFieldDefinitionSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldDefinitionJsonConverter))]
    public partial record DatabaseFieldDefinitionSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldDefinitionJsonConverter))]
    public partial class DatabaseFieldDefinition
    {
    }

    [JsonConverter(typeof(DatabaseFieldDefinitionJsonConverter))]
    public partial class ReadOnlyDatabaseFieldDefinition
    {
    }

    /// <summary>
    ///     <see cref="DatabaseFieldDefinition"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseFieldDefinitionJsonConverter : JsonConverter<IDatabaseFieldDefinitionSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseFieldDefinitionSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseFieldDefinitionSettings Read(
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
                else if (currentProperty == propertyNames.FieldType)
                {
                    properties.FieldType = new DatabaseFieldTypeJsonConverter().Read(
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

        private DatabaseFieldDefinitionSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.FieldName, propertyNames.FieldName)
                || validator.IsNull(properties.FieldType, propertyNames.FieldType)
                || validator.IsNull(properties.SpecialSettingDefinition, propertyNames.SpecialSettingDefinition)
                || validator.IsNull(properties.FieldMemo, propertyNames.FieldMemo)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseFieldDefinitionSettings
            {
                FieldName = properties.FieldName,
                FieldType = properties.FieldType,
                SpecialSettingDefinition = properties.SpecialSettingDefinition,
                FieldMemo = properties.FieldMemo,
            };
        }

        private IDatabaseFieldDefinitionSettings WrapSettings(
            DatabaseFieldDefinitionSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseFieldDefinition))
            {
                return new DatabaseFieldDefinition(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseFieldDefinition))
            {
                return (ReadOnlyDatabaseFieldDefinition)new DatabaseFieldDefinition(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseFieldDefinitionSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.FieldName)));
            new FieldNameJsonConverter().Write(writer, value.FieldName, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.FieldType)));
            new DatabaseFieldTypeJsonConverter().Write(writer, value.FieldType, options);

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
            public string FieldType { get; }
            public string SpecialSettingDefinition { get; }
            public string FieldMemo { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                FieldName = options.GetConvertedPropertyName(nameof(DatabaseFieldDefinition.FieldName));
                FieldType = options.GetConvertedPropertyName(nameof(DatabaseFieldDefinition.FieldType));
                SpecialSettingDefinition = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldDefinition.SpecialSettingDefinition)
                );
                FieldMemo = options.GetConvertedPropertyName(nameof(DatabaseFieldDefinition.FieldMemo));
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
