// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeMetadataTableJsonConverter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    [JsonConverter(typeof(DatabaseTypeMetadataTableJsonConverter))]
    public partial interface IDatabaseTypeMetadataTableSettings
    {
    }

    [JsonConverter(typeof(DatabaseTypeMetadataTableJsonConverter))]
    public partial record DatabaseTypeMetadataTableSettings
    {
    }

    [JsonConverter(typeof(DatabaseTypeMetadataTableJsonConverter))]
    public partial class DatabaseTypeMetadataTable
    {
    }

    [JsonConverter(typeof(DatabaseTypeMetadataTableJsonConverter))]
    public partial class FixedDatabaseTypeMetadataTable
    {
    }

    [JsonConverter(typeof(DatabaseTypeMetadataTableJsonConverter))]
    public partial class ReadOnlyDatabaseTypeMetadataTable
    {
    }

    /// <summary>
    ///     <see cref="DatabaseTypeMetadataTable"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseTypeMetadataTableJsonConverter : JsonConverter<IDatabaseTypeMetadataTableSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseTypeMetadataTableSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseTypeMetadataTableSettings Read(
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
                else if (currentProperty == propertyNames.Items)
                {
                    properties.Items = ReadItems(ref reader, options, typeToConvert);
                }
                else
                {
                    throw new JsonException($"予期しないプロパティです。(プロパティ名: {currentProperty})");
                }
            }

            return properties;
        }

        private List<IDatabaseNamedDataRowSettings> ReadItems(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options,
            Type typeToConvert
        )
        {
            var items = new List<IDatabaseNamedDataRowSettings>();
            var itemConverter = new DatabaseNamedDataRowJsonConverter();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                var item = itemConverter.Read(ref reader, typeToConvert, options);
                if (item is null)
                {
                    throw new JsonException("null 要素は許可されていません。");
                }

                items.Add(item);
            }

            return items;
        }

        private DatabaseTypeMetadataTableSettings CreateSettings(
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
                || validator.IsNull(properties.Items, propertyNames.Items)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseTypeMetadataTableSettings(properties.Items)
            {
                TypeName = properties.TypeName,
                Memo = properties.Memo,
                DataNamingDefinition = properties.DataNamingDefinition,
                FieldMetadataList = properties.FieldMetadataList,
            };
        }

        private IDatabaseTypeMetadataTableSettings WrapSettings(
            DatabaseTypeMetadataTableSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseTypeMetadataTable))
            {
                return new DatabaseTypeMetadataTable(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseTypeMetadataTable))
            {
                return (FixedDatabaseTypeMetadataTable)new DatabaseTypeMetadataTable(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseTypeMetadataTable))
            {
                return (ReadOnlyDatabaseTypeMetadataTable)new DatabaseTypeMetadataTable(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseTypeMetadataTableSettings value,
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
            writer.WritePropertyName(options.GetConvertedPropertyName(ListConstant.JsonFieldNameItems));
            WriteItems(writer, value, options);

            writer.WriteEndObject();
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseTypeMetadataTableSettings value,
            JsonSerializerOptions options
        )
        {
            var itemConverter = new DatabaseNamedDataRowJsonConverter();

            writer.WriteStartArray();

            foreach (var item in value.Settings)
            {
                itemConverter.Write(writer, item, options);
            }

            writer.WriteEndArray();
        }

        #endregion

        private class PropertyNames
        {
            public string TypeName { get; }
            public string Memo { get; }
            public string DataNamingDefinition { get; }
            public string FieldMetadataList { get; }
            public string Items { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                TypeName = options.GetConvertedPropertyName(nameof(DatabaseTypeMetadataTable.TypeName));
                Memo = options.GetConvertedPropertyName(nameof(DatabaseTypeMetadataTable.Memo));
                DataNamingDefinition =
                    options.GetConvertedPropertyName(nameof(DatabaseTypeMetadataTable.DataNamingDefinition));
                FieldMetadataList =
                    options.GetConvertedPropertyName(nameof(DatabaseTypeMetadataTable.FieldMetadataList));
                Items = options.GetConvertedPropertyName(ListConstant.JsonFieldNameItems);
            }
        }

        private class PropertyValues
        {
            public TypeName? TypeName { get; set; }
            public DatabaseMemo? Memo { get; set; }
            public DatabaseDataNamingDefinition? DataNamingDefinition { get; set; }
            public IDatabaseFieldMetadataListSettings? FieldMetadataList { get; set; }
            public IList<IDatabaseNamedDataRowSettings>? Items { get; set; }
        }
    }
}
