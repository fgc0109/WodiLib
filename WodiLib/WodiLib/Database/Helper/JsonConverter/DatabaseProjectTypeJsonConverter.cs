// ========================================
// Project Name : WodiLib
// File Name    : DatabaseProjectTypeJsonConverter.cs
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
    [JsonConverter(typeof(DatabaseProjectTypeJsonConverter))]
    public partial interface IDatabaseProjectTypeSettings
    {
    }

    [JsonConverter(typeof(DatabaseProjectTypeJsonConverter))]
    public partial record DatabaseProjectTypeSettings
    {
    }

    [JsonConverter(typeof(DatabaseProjectTypeJsonConverter))]
    public partial class DatabaseProjectType
    {
    }

    [JsonConverter(typeof(DatabaseProjectTypeJsonConverter))]
    public partial class ReadOnlyDatabaseProjectType
    {
    }

    /// <summary>
    ///     <see cref="DatabaseProjectType"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseProjectTypeJsonConverter : JsonConverter<IDatabaseProjectTypeSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseProjectTypeSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseProjectTypeSettings Read(
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
                else if (currentProperty == propertyNames.DataNameList)
                {
                    properties.DataNameList = new DatabaseDataNameListJsonConverter().Read(
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

        private DatabaseProjectTypeSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.TypeName, propertyNames.TypeName)
                || validator.IsNull(properties.Memo, propertyNames.Memo)
                || validator.IsNull(properties.DataNameList, propertyNames.DataNameList)
                || validator.IsNull(properties.FieldMetadataList, propertyNames.FieldMetadataList)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseProjectTypeSettings
            {
                TypeName = properties.TypeName,
                Memo = properties.Memo,
                DataNameList = properties.DataNameList,
                FieldMetadataList = properties.FieldMetadataList,
            };
        }

        private IDatabaseProjectTypeSettings WrapSettings(
            DatabaseProjectTypeSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseProjectType))
            {
                return new DatabaseProjectType(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseProjectType))
            {
                return (ReadOnlyDatabaseProjectType)new DatabaseProjectType(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseProjectTypeSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.TypeName)));
            new TypeNameJsonConverter().Write(writer, value.TypeName, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.Memo)));
            new DatabaseMemoJsonConverter().Write(writer, value.Memo, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DataNameList)));
            new DatabaseDataNameListJsonConverter().Write(writer, value.DataNameList, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.FieldMetadataList)));
            new DatabaseFieldMetadataListJsonConverter().Write(writer, value.FieldMetadataList, options);

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string TypeName { get; }
            public string Memo { get; }
            public string DataNameList { get; }
            public string FieldMetadataList { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                TypeName = options.GetConvertedPropertyName(nameof(DatabaseProjectType.TypeName));
                Memo = options.GetConvertedPropertyName(nameof(DatabaseProjectType.Memo));
                DataNameList = options.GetConvertedPropertyName(nameof(DatabaseProjectType.DataNameList));
                FieldMetadataList = options.GetConvertedPropertyName(nameof(DatabaseProjectType.FieldMetadataList));
            }
        }

        private class PropertyValues
        {
            public TypeName? TypeName { get; set; }
            public DatabaseMemo? Memo { get; set; }
            public IDatabaseDataNameListSettings? DataNameList { get; set; }
            public IDatabaseFieldMetadataListSettings? FieldMetadataList { get; set; }
        }
    }
}
