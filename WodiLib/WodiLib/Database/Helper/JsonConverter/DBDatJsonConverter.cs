// ========================================
// Project Name : WodiLib
// File Name    : DBDatJsonConverter.cs
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
    [JsonConverter(typeof(DBDatJsonConverter))]
    public partial interface IDBDatSettings
    {
    }

    [JsonConverter(typeof(DBDatJsonConverter))]
    public partial record DBDatSettings
    {
    }

    [JsonConverter(typeof(DBDatJsonConverter))]
    public partial class DBDat
    {
    }

    [JsonConverter(typeof(DBDatJsonConverter))]
    public partial class ReadOnlyDBDat
    {
    }

    /// <summary>
    ///     <see cref="DBDat"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DBDatJsonConverter : JsonConverter<IDBDatSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDBDatSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDBDatSettings Read(
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
                else if (currentProperty == propertyNames.DataTableDefinitionList)
                {
                    properties.DataTableDefinitionList =
                        new DatabaseDataTableWithDataNamingDefinitionListJsonConverter().Read(
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

        private DBDatSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.DbKind, propertyNames.DbKind)
                || validator.IsNull(properties.DataTableDefinitionList, propertyNames.DataTableDefinitionList)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DBDatSettings
            {
                DbKind = properties.DbKind,
                DataTableDefinitionList = properties.DataTableDefinitionList,
            };
        }

        private IDBDatSettings WrapSettings(
            DBDatSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DBDat))
            {
                return new DBDat(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDBDat))
            {
                return (ReadOnlyDBDat)new DBDat(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, IDBDatSettings value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DbKind)));
            new DatabaseKindJsonConverter().Write(writer, value.DbKind, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DataTableDefinitionList)));
            new DatabaseDataTableWithDataNamingDefinitionListJsonConverter().Write(
                writer,
                value.DataTableDefinitionList,
                options
            );

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string DbKind { get; }
            public string DataTableDefinitionList { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                DbKind = options.GetConvertedPropertyName(nameof(DBDat.DbKind));
                DataTableDefinitionList = options.GetConvertedPropertyName(nameof(DBDat.DataTableDefinitionList));
            }
        }

        private class PropertyValues
        {
            public DatabaseKind? DbKind { get; set; }
            public IDatabaseDataTableWithDataNamingDefinitionListSettings? DataTableDefinitionList { get; set; }
        }
    }
}
