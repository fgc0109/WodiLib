// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldSpecialSettingDefinitionJsonConverter.cs
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
    #region CommonClass

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial interface IDatabaseFieldSpecialSettingDefinitionSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial record DatabaseFieldSpecialSettingDefinitionSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial class DatabaseFieldSpecialSettingDefinition
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial class ReadOnlyDatabaseFieldSpecialSettingDefinition
    {
    }

    #endregion

    #region Normal

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial interface IDatabaseFieldSpecialSettingDefinitionNormalSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial record DatabaseFieldSpecialSettingDefinitionNormalSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial class DatabaseFieldSpecialSettingDefinitionNormal
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial class ReadOnlyDatabaseFieldSpecialSettingDefinitionNormal
    {
    }

    #endregion

    #region LoadFile

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial interface IDatabaseFieldSpecialSettingDefinitionLoadFileSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial record DatabaseFieldSpecialSettingDefinitionLoadFileSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial class DatabaseFieldSpecialSettingDefinitionLoadFile
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial class ReadOnlyDatabaseFieldSpecialSettingDefinitionLoadFile
    {
    }

    #endregion

    #region DatabaseReference

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial interface IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial record DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial class DatabaseFieldSpecialSettingDefinitionDatabaseReference
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial class ReadOnlyDatabaseFieldSpecialSettingDefinitionDatabaseReference
    {
    }

    #endregion

    #region Manual

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial interface IDatabaseFieldSpecialSettingDefinitionManualSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial record DatabaseFieldSpecialSettingDefinitionManualSettings
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial class DatabaseFieldSpecialSettingDefinitionManual
    {
    }

    [JsonConverter(typeof(DatabaseFieldSpecialSettingDefinitionJsonConverter))]
    public partial class ReadOnlyDatabaseFieldSpecialSettingDefinitionManual
    {
    }

    #endregion

    /// <summary>
    ///     <see cref="DatabaseFieldSpecialSettingDefinition"/> インスタンスのJSONシリアライズ/デシリアライズクラス
    /// </summary>
    public class DatabaseFieldSpecialSettingDefinitionJsonConverter :
        JsonConverter<IDatabaseFieldSpecialSettingDefinitionSettings>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IDatabaseFieldSpecialSettingDefinitionSettings).IsAssignableFrom(typeToConvert);
        }

        #region Read

        /// <inheritdoc/>
        public override IDatabaseFieldSpecialSettingDefinitionSettings Read(
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
            var properties = ReadProperties(ref reader, propertyNames, options);

            var settings = CreateSettings(properties, propertyNames);
            return WrapSettings(settings, typeToConvert);
        }

        private PropertyValues ReadProperties(
            ref Utf8JsonReader reader,
            PropertyNames propertyNames,
            JsonSerializerOptions options
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

                if (currentProperty == propertyNames.SettingType)
                {
                    properties.SettingType = new DatabaseFieldSpecialSettingTypeJsonConverter().Read(
                        ref reader,
                        typeof(DatabaseFieldSpecialSettingType),
                        options
                    );
                }
                else if (currentProperty == propertyNames.InitValue)
                {
                    properties.InitValue = JsonSerializer.Deserialize<DatabaseValueInt>(ref reader, options);
                }
                else if (currentProperty == propertyNames.FolderName)
                {
                    properties.FolderName = JsonSerializer.Deserialize<DBSettingFolderName>(ref reader, options);
                }
                else if (currentProperty == propertyNames.IsOmitFolderName)
                {
                    properties.IsOmitFolderName = reader.GetBoolean();
                }
                else if (currentProperty == propertyNames.DatabaseReferKind)
                {
                    properties.DatabaseReferKind = new DatabaseReferTypeJsonConverter().Read(
                        ref reader,
                        typeof(DatabaseReferType),
                        options
                    );
                }
                else if (currentProperty == propertyNames.DatabaseDbTypeId)
                {
                    properties.DatabaseDbTypeId = JsonSerializer.Deserialize<TypeId>(ref reader, options);
                }
                else if (currentProperty == propertyNames.IsUseAdditionalItems)
                {
                    properties.IsUseAdditionalItems = reader.GetBoolean();
                }
                else if (currentProperty == propertyNames.AdditionalCase1)
                {
                    properties.AdditionalCase1 =
                        JsonSerializer.Deserialize<DatabaseValueCaseDescription>(ref reader, options);
                }
                else if (currentProperty == propertyNames.AdditionalCase2)
                {
                    properties.AdditionalCase2 =
                        JsonSerializer.Deserialize<DatabaseValueCaseDescription>(ref reader, options);
                }
                else if (currentProperty == propertyNames.AdditionalCase3)
                {
                    properties.AdditionalCase3 =
                        JsonSerializer.Deserialize<DatabaseValueCaseDescription>(ref reader, options);
                }
                else if (currentProperty == propertyNames.SpecialCases)
                {
                    properties.SpecialCases =
                        JsonSerializer.Deserialize<IDatabaseValueCaseListSettings>(ref reader, options);
                }
                else
                {
                    throw new JsonException($"予期しないプロパティです。(プロパティ名: {currentProperty})");
                }
            }

            return properties;
        }

        private DatabaseFieldSpecialSettingDefinitionSettings CreateSettings(
            PropertyValues properties,
            PropertyNames propertyNames
        )
        {
            var validator = new JsonPropertyValidator();
            if (validator.IsNull(properties.SettingType, propertyNames.SettingType))
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            if (properties.SettingType == DatabaseFieldSpecialSettingType.Normal)
            {
                return CreateNormalSettings(properties, propertyNames, validator);
            }

            if (properties.SettingType == DatabaseFieldSpecialSettingType.LoadFile)
            {
                return CreateLoadFileSettings(properties, propertyNames, validator);
            }

            if (properties.SettingType == DatabaseFieldSpecialSettingType.ReferDatabase)
            {
                return CreateDatabaseReferenceSettings(properties, propertyNames, validator);
            }

            if (properties.SettingType == DatabaseFieldSpecialSettingType.Manual)
            {
                return CreateManualSettings(properties, propertyNames, validator);
            }

            throw new JsonException($"SettingType の値が不正です。(設定値:{properties.SettingType})");
        }

        private DatabaseFieldSpecialSettingDefinitionNormalSettings CreateNormalSettings(
            PropertyValues properties,
            PropertyNames propertyNames,
            JsonPropertyValidator validator
        )
        {
            if (validator.IsNull(properties.InitValue, propertyNames.InitValue))
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseFieldSpecialSettingDefinitionNormalSettings
            {
                InitValue = properties.InitValue,
            };
        }

        private DatabaseFieldSpecialSettingDefinitionLoadFileSettings CreateLoadFileSettings(
            PropertyValues properties,
            PropertyNames propertyNames,
            JsonPropertyValidator validator
        )
        {
            if (
                validator.IsNull(properties.FolderName, propertyNames.FolderName)
                || validator.IsNull(properties.IsOmitFolderName, propertyNames.IsOmitFolderName)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
            {
                InitValue = properties.InitValue ?? 0,
                FolderName = properties.FolderName,
                IsOmitFolderName = properties.IsOmitFolderName.Value,
            };
        }

        private DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings CreateDatabaseReferenceSettings(
            PropertyValues properties,
            PropertyNames propertyNames,
            JsonPropertyValidator validator
        )
        {
            if (
                validator.IsNull(properties.InitValue, propertyNames.InitValue)
                || validator.IsNull(properties.DatabaseReferKind, propertyNames.DatabaseReferKind)
                || validator.IsNull(properties.DatabaseDbTypeId, propertyNames.DatabaseDbTypeId)
                || validator.IsNull(properties.IsUseAdditionalItems, propertyNames.IsUseAdditionalItems)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            if (properties.IsUseAdditionalItems.Value
                && (
                    validator.IsNull(properties.AdditionalCase1, propertyNames.AdditionalCase1)
                    || validator.IsNull(properties.AdditionalCase2, propertyNames.AdditionalCase2)
                    || validator.IsNull(properties.AdditionalCase3, propertyNames.AdditionalCase3)
                ))
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            {
                InitValue = properties.InitValue,
                DatabaseReferKind = properties.DatabaseReferKind,
                DatabaseDbTypeId = properties.DatabaseDbTypeId,
                IsUseAdditionalItems = properties.IsUseAdditionalItems.Value,
                AdditionalCase1 = properties.AdditionalCase1 ?? "",
                AdditionalCase2 = properties.AdditionalCase2 ?? "",
                AdditionalCase3 = properties.AdditionalCase3 ?? "",
            };
        }

        private DatabaseFieldSpecialSettingDefinitionManualSettings CreateManualSettings(
            PropertyValues properties,
            PropertyNames propertyNames,
            JsonPropertyValidator validator
        )
        {
            if (
                validator.IsNull(properties.InitValue, propertyNames.InitValue)
                || validator.IsNull(properties.SpecialCases, propertyNames.SpecialCases)
            )
            {
                throw new JsonException(
                    $"必須プロパティがありません。（不足プロパティ：{string.Join(", ", validator.NullPropertyNameList)}）"
                );
            }

            return new DatabaseFieldSpecialSettingDefinitionManualSettings
            {
                InitValue = properties.InitValue,
                SpecialCases = properties.SpecialCases,
            };
        }

        private IDatabaseFieldSpecialSettingDefinitionSettings WrapSettings(
            DatabaseFieldSpecialSettingDefinitionSettings settings,
            Type typeToConvert
        )
        {
            if (typeToConvert == typeof(DatabaseFieldSpecialSettingDefinition))
            {
                return new DatabaseFieldSpecialSettingDefinition(settings);
            }

            if (typeToConvert == typeof(ReadOnlyDatabaseFieldSpecialSettingDefinition))
            {
                return (ReadOnlyDatabaseFieldSpecialSettingDefinition)new DatabaseFieldSpecialSettingDefinition(
                    settings
                );
            }

            return settings;
        }

        #endregion

        #region Write

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer,
            IDatabaseFieldSpecialSettingDefinitionSettings value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();

            if (value.TryCastNormalSettings(out var normalSettings))
            {
                WriteNormal(writer, normalSettings, options);
            }
            else if (value.TryCastLoadFileSettings(out var loadFileSettings))
            {
                WriteLoadFile(writer, loadFileSettings, options);
            }
            else if (value.TryCastDatabaseReferenceSettings(out var databaseReferenceSettings))
            {
                WriteDatabaseReference(writer, databaseReferenceSettings, options);
            }
            else if (value.TryCastManualSettings(out var manualSettings))
            {
                WriteManual(writer, manualSettings, options);
            }
            else
            {
                throw new JsonException("JSONシリアル化に対応していないオブジェクトです。");
            }

            writer.WriteEndObject();
        }

        private void WriteNormal(
            Utf8JsonWriter writer,
            IDatabaseFieldSpecialSettingDefinitionNormalSettings normalSettings,
            JsonSerializerOptions options
        )
        {
            writer.WritePropertyName(
                options.GetConvertedPropertyName(nameof(DatabaseFieldSpecialSettingDefinition.SettingType))
            );
            new DatabaseFieldSpecialSettingTypeJsonConverter().Write(
                writer,
                DatabaseFieldSpecialSettingType.Normal,
                options
            );

            writer.WritePropertyName(
                options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionNormalSettings.InitValue)
                )
            );
            new DatabaseValueIntJsonConverter().Write(writer, normalSettings.InitValue, options);
        }

        private void WriteLoadFile(
            Utf8JsonWriter writer,
            IDatabaseFieldSpecialSettingDefinitionLoadFileSettings loadFileSettings,
            JsonSerializerOptions options
        )
        {
            writer.WritePropertyName(
                options.GetConvertedPropertyName(nameof(DatabaseFieldSpecialSettingDefinition.SettingType))
            );
            new DatabaseFieldSpecialSettingTypeJsonConverter().Write(
                writer,
                DatabaseFieldSpecialSettingType.LoadFile,
                options
            );

            if (loadFileSettings.InitValue != 0)
            {
                writer.WritePropertyName(
                    options.GetConvertedPropertyName(
                        nameof(DatabaseFieldSpecialSettingDefinitionLoadFileSettings.InitValue)
                    )
                );
                new DatabaseValueIntJsonConverter().Write(writer, loadFileSettings.InitValue, options);
            }

            writer.WritePropertyName(
                options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionLoadFileSettings.FolderName)
                )
            );
            new DBSettingFolderNameJsonConverter().Write(writer, loadFileSettings.FolderName, options);

            writer.WritePropertyName(
                options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionLoadFileSettings.IsOmitFolderName)
                )
            );
            writer.WriteBooleanValue(loadFileSettings.IsOmitFolderName);
        }

        private void WriteDatabaseReference(
            Utf8JsonWriter writer,
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings databaseReferenceSettings,
            JsonSerializerOptions options
        )
        {
            writer.WritePropertyName(
                options.GetConvertedPropertyName(nameof(DatabaseFieldSpecialSettingDefinition.SettingType))
            );
            new DatabaseFieldSpecialSettingTypeJsonConverter().Write(
                writer,
                DatabaseFieldSpecialSettingType.ReferDatabase,
                options
            );

            writer.WritePropertyName(
                options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.InitValue)
                )
            );
            new DatabaseValueIntJsonConverter().Write(writer, databaseReferenceSettings.InitValue, options);

            writer.WritePropertyName(
                options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseReferKind)
                )
            );
            new DatabaseReferTypeJsonConverter().Write(
                writer,
                databaseReferenceSettings.DatabaseReferKind,
                options
            );

            writer.WritePropertyName(
                options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseDbTypeId)
                )
            );
            new TypeIdJsonConverter().Write(writer, databaseReferenceSettings.DatabaseDbTypeId, options);

            writer.WritePropertyName(
                options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.IsUseAdditionalItems)
                )
            );
            writer.WriteBooleanValue(databaseReferenceSettings.IsUseAdditionalItems);

            if (databaseReferenceSettings.IsUseAdditionalItems)
            {
                writer.WritePropertyName(
                    options.GetConvertedPropertyName(
                        nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase1)
                    )
                );
                new DatabaseValueCaseDescriptionJsonConverter().Write(
                    writer,
                    databaseReferenceSettings.AdditionalCase1,
                    options
                );

                writer.WritePropertyName(
                    options.GetConvertedPropertyName(
                        nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase2)
                    )
                );
                new DatabaseValueCaseDescriptionJsonConverter().Write(
                    writer,
                    databaseReferenceSettings.AdditionalCase2,
                    options
                );

                writer.WritePropertyName(
                    options.GetConvertedPropertyName(
                        nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase3)
                    )
                );
                new DatabaseValueCaseDescriptionJsonConverter().Write(
                    writer,
                    databaseReferenceSettings.AdditionalCase3,
                    options
                );
            }
        }

        private void WriteManual(
            Utf8JsonWriter writer,
            IDatabaseFieldSpecialSettingDefinitionManualSettings manualSettings,
            JsonSerializerOptions options
        )
        {
            writer.WritePropertyName(
                options.GetConvertedPropertyName(nameof(DatabaseFieldSpecialSettingDefinition.SettingType))
            );
            new DatabaseFieldSpecialSettingTypeJsonConverter().Write(
                writer,
                DatabaseFieldSpecialSettingType.Manual,
                options
            );

            writer.WritePropertyName(
                options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.InitValue)
                )
            );
            new DatabaseValueIntJsonConverter().Write(writer, manualSettings.InitValue, options);

            writer.WritePropertyName(
                options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.SpecialCases)
                )
            );
            new DatabaseValueCaseListJsonConverter().Write(writer, manualSettings.SpecialCases, options);
        }

        #endregion

        private class PropertyNames
        {
            public string SettingType { get; }
            public string InitValue { get; }
            public string FolderName { get; }
            public string IsOmitFolderName { get; }
            public string DatabaseReferKind { get; }
            public string DatabaseDbTypeId { get; }
            public string IsUseAdditionalItems { get; }
            public string AdditionalCase1 { get; }
            public string AdditionalCase2 { get; }
            public string AdditionalCase3 { get; }
            public string SpecialCases { get; }

            public PropertyNames(JsonSerializerOptions options)
            {
                SettingType = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinition.SettingType)
                );
                InitValue = options.GetConvertedPropertyName(nameof(DatabaseFieldSpecialSettingDefinition.InitValue));
                FolderName = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionLoadFile.FolderName)
                );
                IsOmitFolderName = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionLoadFile.IsOmitFolderName)
                );
                DatabaseReferKind = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.DatabaseReferKind)
                );
                DatabaseDbTypeId = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.DatabaseDbTypeId)
                );
                IsUseAdditionalItems = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.IsUseAdditionalItems)
                );
                AdditionalCase1 = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.AdditionalCase1)
                );
                AdditionalCase2 = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.AdditionalCase2)
                );
                AdditionalCase3 = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.AdditionalCase3)
                );
                SpecialCases = options.GetConvertedPropertyName(
                    nameof(DatabaseFieldSpecialSettingDefinitionManual.SpecialCases)
                );
            }
        }

        private class PropertyValues
        {
            public DatabaseFieldSpecialSettingType? SettingType { get; set; }
            public DatabaseValueInt? InitValue { get; set; }
            public DBSettingFolderName? FolderName { get; set; }
            public bool? IsOmitFolderName { get; set; }
            public DatabaseReferType? DatabaseReferKind { get; set; }
            public TypeId? DatabaseDbTypeId { get; set; }
            public bool? IsUseAdditionalItems { get; set; }
            public DatabaseValueCaseDescription? AdditionalCase1 { get; set; }
            public DatabaseValueCaseDescription? AdditionalCase2 { get; set; }
            public DatabaseValueCaseDescription? AdditionalCase3 { get; set; }
            public IDatabaseValueCaseListSettings? SpecialCases { get; set; }
        }
    }
}
