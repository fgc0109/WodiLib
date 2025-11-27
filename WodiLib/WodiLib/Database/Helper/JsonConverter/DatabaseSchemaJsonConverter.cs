// ========================================
// Project Name : WodiLib
// File Name    : DatabaseSchemaJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseSchemaJsonConverter))]
    public partial interface IDatabaseSchemaSettings
    {
    }

    [JsonConverter(typeof(DatabaseSchemaJsonConverter))]
    public partial record DatabaseSchemaSettings
    {
    }

    [JsonConverter(typeof(DatabaseSchemaJsonConverter))]
    public partial class DatabaseSchema
    {
    }

    [JsonConverter(typeof(DatabaseSchemaJsonConverter))]
    public partial class ReadOnlyDatabaseSchema
    {
    }

    /// <summary>
    ///     <see cref="DatabaseSchema"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseSchemaJsonConverter : JsonConverter<IDatabaseSchemaSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseSchemaSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseSchemaSettings Read(
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
                if (currentProperty == propertyNames.DbKind)
                {
                    properties.DbKind = new DatabaseKindJsonConverter().Read(
                        ref reader,
                        typeToConvert,
                        options
                    );
                }
                else if (currentProperty == propertyNames.TypeTableList)
                {
                    properties.TypeTableList = new DatabaseTypeTableListJsonConverter().Read(
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

        private DatabaseSchemaSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.DbKind, propertyNames.DbKind)
                || validator.IsNull(properties.TypeTableList, propertyNames.TypeTableList)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseSchemaSettings
            {
                DbKind = properties.DbKind,
                TypeTableList = properties.TypeTableList,
            };
        }

        private IDatabaseSchemaSettings WrapSettings(
            DatabaseSchemaSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseSchema))
            {
                return new DatabaseSchema(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseSchema))
            {
                return (ReadOnlyDatabaseSchema)new DatabaseSchema(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, IDatabaseSchemaSettings value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DbKind)));
            new DatabaseKindJsonConverter().Write(writer, value.DbKind, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.TypeTableList)));
            new DatabaseTypeTableListJsonConverter().Write(writer, value.TypeTableList, options);

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string DbKind { get; }
            public string TypeTableList { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                DbKind = options.GetConvertedPropertyName(nameof(DatabaseSchema.DbKind));
                TypeTableList = options.GetConvertedPropertyName(nameof(DatabaseSchema.TypeTableList));
            }
        }

        private class PropertyValues
        {
            public DatabaseKind? DbKind { get; set; }
            public IDatabaseTypeTableListSettings? TypeTableList { get; set; }
        }
    }
}
