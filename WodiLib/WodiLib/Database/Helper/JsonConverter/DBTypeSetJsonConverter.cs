// ========================================
// Project Name : WodiLib
// File Name    : DBTypeSetJsonConverter.cs
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
    [JsonConverter(typeof(DBTypeSetJsonConverter))]
    public partial interface IDBTypeSetSettings
    {
    }

    [JsonConverter(typeof(DBTypeSetJsonConverter))]
    public partial record DBTypeSetSettings
    {
    }

    [JsonConverter(typeof(DBTypeSetJsonConverter))]
    public partial class DBTypeSet
    {
    }

    [JsonConverter(typeof(DBTypeSetJsonConverter))]
    public partial class ReadOnlyDBTypeSet
    {
    }

    /// <summary>
    ///     <see cref="DBTypeSet"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DBTypeSetJsonConverter : JsonConverter<IDBTypeSetSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDBTypeSetSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDBTypeSetSettings Read(
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
                if (currentProperty == propertyNames.TypeDefinition)
                {
                    properties.TypeDefinition = new DatabaseTypeDefinitionJsonConverter().Read(
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

        private DBTypeSetSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.TypeDefinition, propertyNames.TypeDefinition)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DBTypeSetSettings
            {
                TypeDefinition = properties.TypeDefinition,
            };
        }

        private IDBTypeSetSettings WrapSettings(
            DBTypeSetSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DBTypeSet))
            {
                return new DBTypeSet(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDBTypeSet))
            {
                return (ReadOnlyDBTypeSet)new DBTypeSet(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, IDBTypeSetSettings value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.TypeDefinition)));
            new DatabaseTypeDefinitionJsonConverter().Write(writer, value.TypeDefinition, options);

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string TypeDefinition { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                TypeDefinition = options.GetConvertedPropertyName(nameof(DBTypeSet.TypeDefinition));
            }
        }

        private class PropertyValues
        {
            public IDatabaseTypeDefinitionSettings? TypeDefinition { get; set; }
        }
    }
}
