// ========================================
// Project Name : WodiLib
// File Name    : DBProject.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.Contracts;
using WodiLib.SourceGenerator.Domain.Model.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    [Model(Description = "DBプロジェクト情報（XXXDatabase.project）")]
    public partial class DBProject
    {
        #region Properties

        #region public

        /// <summary>DB種別</summary>
        [ImmutableProperty]
        [SettingsProperty]
        public DatabaseKind? DbKind
        {
            get => dbKind;
            set => SetField(ref dbKind, value);
        }

        /// <summary>
        ///     タイプ情報リスト
        /// </summary>
        [ImmutableProperty(
            ReturnType = typeof(ReadOnlyDatabaseProjectTypeList)
        )]
        [SettingsProperty(
            ReturnType = typeof(IDatabaseProjectTypeListSettings),
            DefaultValue = "new DatabaseProjectTypeListSettings()"
        )]
        public DatabaseProjectTypeList ProjectTypeList
        {
            get => projectTypeList;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(ProjectTypeList));

                SetField(ref projectTypeList, value);
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private DatabaseKind? dbKind;
        private DatabaseProjectTypeList projectTypeList;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="settings">設定DTO</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="settings"/> に不適切な <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        public DBProject(IDBProjectSettings settings)
        {
            ThrowHelper.ValidateArgumentNotNull(settings is null, nameof(settings));
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.ProjectTypeList is null,
                nameof(settings),
                nameof(settings.ProjectTypeList)
            );
            ThrowHelper.ValidateArgumentPropertyNotNull(
                settings.ProjectTypeList.Settings is null,
                nameof(settings),
                nameof(settings.ProjectTypeList.Settings)
            );
            ThrowHelper.ValidateArgumentPropertyItemsHasNotNull(
                settings.ProjectTypeList.Settings.HasNullItem(),
                nameof(settings),
                nameof(settings.ProjectTypeList.Settings)
            );

            dbKind = settings.DbKind;
            projectTypeList = new DatabaseProjectTypeList(settings.ProjectTypeList);
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DBProject() : this(new DBProjectSettings())
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IDBProjectSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DbKind == other.DbKind
                   && ProjectTypeList.ItemEquals(other.ProjectTypeList);
        }

        #endregion

        #endregion
    }
}
