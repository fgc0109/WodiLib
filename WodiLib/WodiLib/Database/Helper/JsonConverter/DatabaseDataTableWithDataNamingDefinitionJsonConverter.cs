// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataTableWithDataNamingDefinitionJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseDataTableWithDataNamingDefinitionJsonConverter))]
    public partial interface IDatabaseDataTableWithDataNamingDefinitionSettings
    {
    }

    [JsonConverter(typeof(DatabaseDataTableWithDataNamingDefinitionJsonConverter))]
    public partial record DatabaseDataTableWithDataNamingDefinitionSettings
    {
    }

    [JsonConverter(typeof(DatabaseDataTableWithDataNamingDefinitionJsonConverter))]
    public partial class DatabaseDataTableWithDataNamingDefinition
    {
    }

    [JsonConverter(typeof(DatabaseDataTableWithDataNamingDefinitionJsonConverter))]
    public partial class ReadOnlyDatabaseDataTableWithDataNamingDefinition
    {
    }

    /// <summary>
    ///     <see cref="DatabaseDataTableWithDataNamingDefinition"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseDataTableWithDataNamingDefinitionJsonConverter :
        JsonConverter<IDatabaseDataTableWithDataNamingDefinitionSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseDataTableWithDataNamingDefinitionSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseDataTableWithDataNamingDefinitionSettings Read(
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
                    properties.DataTable = new DatabaseDataTableJsonConverter().Read(
                        ref reader,
                        typeToConvert,
                        options
                    );
                }
                else if (currentProperty == propertyNames.DataNamingDefinition)
                {
                    properties.DataNamingDefinition =
                        new DatabaseDataNamingDefinitionJsonConverter().Read(
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

        private DatabaseDataTableWithDataNamingDefinitionSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.DataTable, propertyNames.DataTable)
                || validator.IsNull(properties.DataNamingDefinition, propertyNames.DataNamingDefinition)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseDataTableWithDataNamingDefinitionSettings
            {
                DataTable = properties.DataTable,
                DataNamingDefinition = properties.DataNamingDefinition,
            };
        }

        private IDatabaseDataTableWithDataNamingDefinitionSettings WrapSettings(
            DatabaseDataTableWithDataNamingDefinitionSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseDataTableWithDataNamingDefinition))
            {
                return new DatabaseDataTableWithDataNamingDefinition(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseDataTableWithDataNamingDefinition))
            {
                return (ReadOnlyDatabaseDataTableWithDataNamingDefinition)
                    new DatabaseDataTableWithDataNamingDefinition(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseDataTableWithDataNamingDefinitionSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DataTable)));
            new DatabaseDataTableJsonConverter().Write(writer, value.DataTable, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DataNamingDefinition)));
            new DatabaseDataNamingDefinitionJsonConverter().Write(writer, value.DataNamingDefinition, options);

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string DataTable { get; }
            public string DataNamingDefinition { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                DataTable = options.GetConvertedPropertyName(
                    nameof(DatabaseDataTableWithDataNamingDefinition.DataTable)
                );
                DataNamingDefinition = options.GetConvertedPropertyName(
                    nameof(DatabaseDataTableWithDataNamingDefinition.DataNamingDefinition)
                );
            }
        }

        private class PropertyValues
        {
            public IDatabaseDataTableSettings? DataTable { get; set; }
            public DatabaseDataNamingDefinition? DataNamingDefinition { get; set; }
        }
    }
}
