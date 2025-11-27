// ========================================
// Project Name : WodiLib
// File Name    : DBProjectJsonConverter.cs
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
    [JsonConverter(typeof(DBProjectJsonConverter))]
    public partial interface IDBProjectSettings
    {
    }

    [JsonConverter(typeof(DBProjectJsonConverter))]
    public partial record DBProjectSettings
    {
    }

    [JsonConverter(typeof(DBProjectJsonConverter))]
    public partial class DBProject
    {
    }

    [JsonConverter(typeof(DBProjectJsonConverter))]
    public partial class ReadOnlyDBProject
    {
    }

    /// <summary>
    ///     <see cref="DBProject"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DBProjectJsonConverter : JsonConverter<IDBProjectSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDBProjectSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDBProjectSettings Read(
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
                else if (currentProperty == propertyNames.ProjectTypeList)
                {
                    properties.ProjectTypeList = new DatabaseProjectTypeListJsonConverter().Read(
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

        private DBProjectSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (
                validator.IsNull(properties.DbKind, propertyNames.DbKind)
                || validator.IsNull(properties.ProjectTypeList, propertyNames.ProjectTypeList)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DBProjectSettings
            {
                DbKind = properties.DbKind,
                ProjectTypeList = properties.ProjectTypeList,
            };
        }

        private IDBProjectSettings WrapSettings(
            DBProjectSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DBProject))
            {
                return new DBProject(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDBProject))
            {
                return (ReadOnlyDBProject)new DBProject(settings);
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, IDBProjectSettings value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DbKind)));
            new DatabaseKindJsonConverter().Write(writer, value.DbKind, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.ProjectTypeList)));
            new DatabaseProjectTypeListJsonConverter().Write(writer, value.ProjectTypeList, options);

            writer.WriteEndObject();
        }

        #endregion

        private class PropertyNames
        {
            public string DbKind { get; }
            public string ProjectTypeList { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                DbKind = options.GetConvertedPropertyName(nameof(DBProject.DbKind));
                ProjectTypeList = options.GetConvertedPropertyName(nameof(DBProject.ProjectTypeList));
            }
        }

        private class PropertyValues
        {
            public DatabaseKind? DbKind { get; set; }
            public IDatabaseProjectTypeListSettings? ProjectTypeList { get; set; }
        }
    }
}
