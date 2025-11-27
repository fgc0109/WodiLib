// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeMetadataJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseTypeMetadataJsonConverter))]
    public partial interface IDatabaseTypeMetadataSettings
    {
    }

    [JsonConverter(typeof(DatabaseTypeMetadataJsonConverter))]
    public partial record DatabaseTypeMetadataSettings
    {
    }

    [JsonConverter(typeof(DatabaseTypeMetadataJsonConverter))]
    public partial class DatabaseTypeMetadata
    {
    }

    [JsonConverter(typeof(DatabaseTypeMetadataJsonConverter))]
    public partial class ReadOnlyDatabaseTypeMetadata
    {
    }

    /// <summary>
    ///     <see cref="DatabaseTypeMetadata"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseTypeMetadataJsonConverter : JsonConverter<IDatabaseTypeMetadataSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseTypeMetadataSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseTypeMetadataSettings Read(
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
                else if (currentProperty == propertyNames.DataNamingDefinition)
                {
                    properties.DataNamingDefinition = new DatabaseDataNamingDefinitionJsonConverter().Read(
                        ref reader,
                        typeToConvert,
                        options
                    );
                }
                else if (currentProperty == propertyNames.FieldMetadataList)
                {
                    properties.FieldMetadataList = new DatabaseFieldMetadataListJsonConverter().Read(
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

        private DatabaseTypeMetadataSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.TypeName, propertyNames.TypeName)
                || validator.IsNull(properties.Memo, propertyNames.Memo)
                || validator.IsNull(properties.DataNamingDefinition, propertyNames.DataNamingDefinition)
                || validator.IsNull(properties.FieldMetadataList, propertyNames.FieldMetadataList)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseTypeMetadataSettings
            {
                TypeName = properties.TypeName,
                Memo = properties.Memo,
                DataNamingDefinition = properties.DataNamingDefinition,
                FieldMetadataList = properties.FieldMetadataList,
            };
        }

        private IDatabaseTypeMetadataSettings WrapSettings(
            DatabaseTypeMetadataSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseTypeMetadata))
            {
                return new DatabaseTypeMetadata(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseTypeMetadata))
            {
                return (ReadOnlyDatabaseTypeMetadata)new DatabaseTypeMetadata(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseTypeMetadataSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.TypeName)));
            new TypeNameJsonConverter().Write(writer, value.TypeName, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.Memo)));
            new DatabaseMemoJsonConverter().Write(writer, value.Memo, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DataNamingDefinition)));
            new DatabaseDataNamingDefinitionJsonConverter().Write(writer, value.DataNamingDefinition, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.FieldMetadataList)));
            new DatabaseFieldMetadataListJsonConverter().Write(writer, value.FieldMetadataList, options);

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string TypeName { get; }
            public string Memo { get; }
            public string DataNamingDefinition { get; }
            public string FieldMetadataList { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                TypeName = options.GetConvertedPropertyName(nameof(DatabaseTypeMetadata.TypeName));
                Memo = options.GetConvertedPropertyName(nameof(DatabaseTypeMetadata.Memo));
                DataNamingDefinition =
                    options.GetConvertedPropertyName(nameof(DatabaseTypeMetadata.DataNamingDefinition));
                FieldMetadataList =
                    options.GetConvertedPropertyName(nameof(DatabaseTypeMetadata.FieldMetadataList));
            }
        }

        private class PropertyValues
        {
            public TypeName? TypeName { get; set; }
            public DatabaseMemo? Memo { get; set; }
            public DatabaseDataNamingDefinition? DataNamingDefinition { get; set; }
            public IDatabaseFieldMetadataListSettings? FieldMetadataList { get; set; }
        }
    }
}
