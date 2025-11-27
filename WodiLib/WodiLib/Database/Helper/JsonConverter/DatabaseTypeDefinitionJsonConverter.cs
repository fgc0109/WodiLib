// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeDefinitionJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseTypeDefinitionJsonConverter))]
    public partial interface IDatabaseTypeDefinitionSettings
    {
    }

    [JsonConverter(typeof(DatabaseTypeDefinitionJsonConverter))]
    public partial record DatabaseTypeDefinitionSettings
    {
    }

    [JsonConverter(typeof(DatabaseTypeDefinitionJsonConverter))]
    public partial class DatabaseTypeDefinition
    {
    }

    [JsonConverter(typeof(DatabaseTypeDefinitionJsonConverter))]
    public partial class ReadOnlyDatabaseTypeDefinition
    {
    }

    /// <summary>
    ///     <see cref="DatabaseTypeDefinition"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseTypeDefinitionJsonConverter : JsonConverter<IDatabaseTypeDefinitionSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseTypeDefinitionSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseTypeDefinitionSettings Read(
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
                if (currentProperty == propertyNames.TypeName)
                {
                    properties.TypeName = new TypeNameJsonConverter().Read(
                        ref reader,
                        typeToConvert,
                        options
                    );
                }
                else if (currentProperty == propertyNames.Memo)
                {
                    properties.Memo = new DatabaseMemoJsonConverter().Read(
                        ref reader,
                        typeToConvert,
                        options
                    );
                }
                else if (currentProperty == propertyNames.FieldDefinitionList)
                {
                    properties.FieldDefinitionList = new DatabaseFieldDefinitionListJsonConverter().Read(
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

        private DatabaseTypeDefinitionSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.TypeName, propertyNames.TypeName)
                || validator.IsNull(properties.Memo, propertyNames.Memo)
                || validator.IsNull(properties.FieldDefinitionList, propertyNames.FieldDefinitionList)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseTypeDefinitionSettings
            {
                TypeName = properties.TypeName,
                Memo = properties.Memo,
                FieldDefinitionList = properties.FieldDefinitionList,
            };
        }

        private IDatabaseTypeDefinitionSettings WrapSettings(
            DatabaseTypeDefinitionSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseTypeDefinition))
            {
                return new DatabaseTypeDefinition(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseTypeDefinition))
            {
                return (ReadOnlyDatabaseTypeDefinition)new DatabaseTypeDefinition(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseTypeDefinitionSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.TypeName)));
            new TypeNameJsonConverter().Write(writer, value.TypeName, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.Memo)));
            new DatabaseMemoJsonConverter().Write(writer, value.Memo, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.FieldDefinitionList)));
            new DatabaseFieldDefinitionListJsonConverter().Write(writer, value.FieldDefinitionList, options);

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string TypeName { get; }
            public string Memo { get; }
            public string FieldDefinitionList { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                TypeName = options.GetConvertedPropertyName(nameof(DatabaseTypeDefinition.TypeName));
                Memo = options.GetConvertedPropertyName(nameof(DatabaseTypeDefinition.Memo));
                FieldDefinitionList =
                    options.GetConvertedPropertyName(nameof(DatabaseTypeDefinition.FieldDefinitionList));
            }
        }

        private class PropertyValues
        {
            public TypeName? TypeName { get; set; }
            public DatabaseMemo? Memo { get; set; }
            public IDatabaseFieldDefinitionListSettings? FieldDefinitionList { get; set; }
        }
    }
}
