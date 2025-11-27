// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataNamingDefinitionJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseDataNamingDefinitionJsonConverter))]
    public partial record DatabaseDataNamingDefinition
    {
    }

    /// <summary>
    ///     <see cref="DatabaseDataNamingDefinition"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseDataNamingDefinitionJsonConverter : JsonConverter<DatabaseDataNamingDefinition>
    {
        #region Read

        /// <inheritdoc/>
        public override DatabaseDataNamingDefinition Read(
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

            return CreateInstance(properties);
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
                if (currentProperty == propertyNames.NamingType)
                {
                    properties.NamingType = new DatabaseDataNamingTypeJsonConverter().Read(
                        ref reader,
                        typeToConvert,
                        options
                    );
                }
                else if (currentProperty == propertyNames.DBKind)
                {
                    properties.DBKind = new DatabaseKindJsonConverter().Read(
                        ref reader,
                        typeToConvert,
                        options
                    );
                }
                else if (currentProperty == propertyNames.TypeId)
                {
                    properties.TypeId = new TypeIdJsonConverter().Read(
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

        private DatabaseDataNamingDefinition CreateInstance(PropertyValues properties)
        {
            return new DatabaseDataNamingDefinition(
                properties.NamingType,
                properties.DBKind,
                properties.TypeId
            );
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            DatabaseDataNamingDefinition value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.NamingType)));
            new DatabaseDataNamingTypeJsonConverter().Write(writer, value.NamingType, options);

            if (value.NamingType == DatabaseDataNamingType.DesignatedType)
            {
                writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DBKind)));
                new DatabaseKindJsonConverter().Write(writer, value.DBKind, options);

                writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.TypeId)));
                new TypeIdJsonConverter().Write(writer, value.TypeId, options);
            }

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string NamingType { get; }
            public string DBKind { get; }
            public string TypeId { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                NamingType = options.GetConvertedPropertyName(nameof(DatabaseDataNamingDefinition.NamingType));
                DBKind = options.GetConvertedPropertyName(nameof(DatabaseDataNamingDefinition.DBKind));
                TypeId = options.GetConvertedPropertyName(nameof(DatabaseDataNamingDefinition.TypeId));
            }
        }

        private class PropertyValues
        {
            public DatabaseDataNamingType? NamingType { get; set; }
            public DatabaseKind? DBKind { get; set; }
            public TypeId? TypeId { get; set; }
        }
    }
}
