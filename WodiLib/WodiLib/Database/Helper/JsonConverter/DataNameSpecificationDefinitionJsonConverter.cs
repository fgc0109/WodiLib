// ========================================
// Project Name : WodiLib
// File Name    : DataNameSpecificationDefinitionJsonConverter.cs
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
    [JsonConverter(typeof(DataNameSpecificationDefinitionJsonConverter))]
    public partial record DataNameSpecificationDefinition
    {
    }

    /// <summary>
    ///     <see cref="DataNameSpecificationDefinition"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DataNameSpecificationDefinitionJsonConverter : JsonConverter<DataNameSpecificationDefinition>
    {
        /// <inheritdoc/>
        public override DataNameSpecificationDefinition Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            DatabaseKind? databaseKind = null;
            TypeId? typeId = null;

            // プロパティ名
            var databaseKindName =
                options.GetConvertedPropertyName(nameof(DataNameSpecificationDefinition.DatabaseKind));
            var typeIdName = options.GetConvertedPropertyName(nameof(DataNameSpecificationDefinition.TypeId));

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new DataNameSpecificationDefinition(
                        databaseKind,
                        typeId
                    );
                }

                // key
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

                // value
                var currentProperty = options.GetConvertedPropertyName(propertyName);
                if (currentProperty == databaseKindName)
                {
                    databaseKind = new DatabaseKindJsonConverter().Read(ref reader, typeToConvert, options);
                }
                else if (currentProperty == typeIdName)
                {
                    typeId = new TypeIdJsonConverter().Read(ref reader, typeToConvert, options);
                }
                else
                {
                    throw new JsonException($"予期しないプロパティです。(プロパティ名: {currentProperty})");
                }
            }

            throw new JsonException();
        }

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            DataNameSpecificationDefinition value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.DatabaseKind)));
            new DatabaseKindJsonConverter().Write(writer, value.DatabaseKind, options);

            writer.WritePropertyName(options.GetConvertedPropertyName(nameof(value.TypeId)));
            new TypeIdJsonConverter().Write(writer, value.TypeId, options);

            writer.WriteEndObject();
        }
    }
}
