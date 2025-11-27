// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeTableJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseTypeTableJsonConverter))]
    public partial interface IDatabaseTypeTableSettings
    {
    }

    [JsonConverter(typeof(DatabaseTypeTableJsonConverter))]
    public partial record DatabaseTypeTableSettings
    {
    }

    [JsonConverter(typeof(DatabaseTypeTableJsonConverter))]
    public partial class DatabaseTypeTable
    {
    }

    [JsonConverter(typeof(DatabaseTypeTableJsonConverter))]
    public partial class FixedDatabaseTypeTable
    {
    }

    [JsonConverter(typeof(DatabaseTypeTableJsonConverter))]
    public partial class ReadOnlyDatabaseTypeTable
    {
    }

    /// <summary>
    ///     <see cref="DatabaseTypeTable"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseTypeTableJsonConverter : JsonConverter<IDatabaseTypeTableSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseTypeTableSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseTypeTableSettings Read(
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
                else if (currentProperty == propertyNames.FieldDefinitionList)
                {
                    properties.FieldDefinitionList = new DatabaseFieldDefinitionListJsonConverter().Read(
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

        private DatabaseTypeTableSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.TypeName, propertyNames.TypeName)
                || validator.IsNull(properties.Memo, propertyNames.Memo)
                || validator.IsNull(properties.DataNamingDefinition, propertyNames.DataNamingDefinition)
                || validator.IsNull(properties.FieldDefinitionList, propertyNames.FieldDefinitionList)
                || validator.IsNull(properties.Items, propertyNames.Items)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseTypeTableSettings(properties.Items)
            {
                TypeName = properties.TypeName,
                Memo = properties.Memo,
                DataNamingDefinition = properties.DataNamingDefinition,
                FieldDefinitionList = properties.FieldDefinitionList,
            };
        }

        private IDatabaseTypeTableSettings WrapSettings(
            DatabaseTypeTableSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseTypeTable))
            {
                return new DatabaseTypeTable(settings);
            }

            if (typeToConvert == typeof(FixedDatabaseTypeTable))
            {
                return (FixedDatabaseTypeTable)new DatabaseTypeTable(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseTypeTable))
            {
                return (ReadOnlyDatabaseTypeTable)new DatabaseTypeTable(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseTypeTableSettings value,
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
            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.FieldDefinitionList)));
            new DatabaseFieldDefinitionListJsonConverter().Write(writer, value.FieldDefinitionList, options);
            writer.WritePropertyName(options.GetConvertedPropertyName(ListConstant.JsonFieldNameItems));
            WriteItems(writer, value, options);

            writer.WriteEndObject();
        }

        private void WriteItems(
            Utf8JsonWriter writer,
            IDatabaseTypeTableSettings value,
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
            public string FieldDefinitionList { get; }
            public string Items { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                TypeName = options.GetConvertedPropertyName(nameof(DatabaseTypeTable.TypeName));
                Memo = options.GetConvertedPropertyName(nameof(DatabaseTypeTable.Memo));
                DataNamingDefinition = options.GetConvertedPropertyName(nameof(DatabaseTypeTable.DataNamingDefinition));
                FieldDefinitionList = options.GetConvertedPropertyName(nameof(DatabaseTypeTable.FieldDefinitionList));
                Items = options.GetConvertedPropertyName(ListConstant.JsonFieldNameItems);
            }
        }

        private class PropertyValues
        {
            public TypeName? TypeName { get; set; }
            public DatabaseMemo? Memo { get; set; }
            public DatabaseDataNamingDefinition? DataNamingDefinition { get; set; }
            public IDatabaseFieldDefinitionListSettings? FieldDefinitionList { get; set; }
            public IList<IDatabaseNamedDataRowSettings>? Items { get; set; }
        }
    }
}
