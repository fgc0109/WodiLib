// ========================================
// Project Name : WodiLib
// File Name    : DatabaseValueCaseJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseValueCaseJsonConverter))]
    public partial record DatabaseValueCase
    {
    }

    /// <summary>
    ///     <see cref="DatabaseValueCase"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseValueCaseJsonConverter : JsonConverter<DatabaseValueCase>
    {
        #region Read

        /// <inheritdoc/>
        public override DatabaseValueCase Read(
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

            return CreateInstance(properties, propertyNames);
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
                if (currentProperty == propertyNames.CaseNumber)
                {
                    properties.CaseNumber = new DatabaseValueCaseNumberJsonConverter().Read(
                        ref reader,
                        typeToConvert,
                        options
                    );
                }
                else if (currentProperty == propertyNames.Description)
                {
                    properties.Description = new DatabaseValueCaseDescriptionJsonConverter().Read(
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

        private DatabaseValueCase CreateInstance(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.CaseNumber, propertyNames.CaseNumber)
                || validator.IsNull(properties.Description, propertyNames.Description)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseValueCase(
                properties.CaseNumber,
                properties.Description
            );
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DatabaseValueCase value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.CaseNumber)));
            new DatabaseValueCaseNumberJsonConverter().Write(writer, value.CaseNumber, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.Description)));
            new DatabaseValueCaseDescriptionJsonConverter().Write(writer, value.Description, options);

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string CaseNumber { get; }
            public string Description { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                CaseNumber = options.GetConvertedPropertyName(nameof(DatabaseValueCase.CaseNumber));
                Description = options.GetConvertedPropertyName(nameof(DatabaseValueCase.Description));
            }
        }

        private class PropertyValues
        {
            public DatabaseValueCaseNumber? CaseNumber { get; set; }
            public DatabaseValueCaseDescription? Description { get; set; }
        }
    }
}
