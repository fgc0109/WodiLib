// ========================================
// Project Name : WodiLib.Test
// File Name    : StubModel.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Domain.Model.Attributes;

namespace WodiLib.Test.Tools
{
    /*
     * テスト用のモデルクラス。
     * WodiLib内で使用するModelクラス（プロパティやメソッドを持ち、
     * 状態変化時に変更通知を行う）の実装サンプルも兼ねる。
     *
     * 実際にModelクラスを定義する場合はいくつかの属性を使用し、
     * SourceGenerator で自動生成する。
     *
     * 生成されたソースは Generated/{名前空間}/{クラス名}.cs に出力される。
     *
     * SourceGeneratorによって、
     * ・コンストラクタに与える設定インタフェース（初期化パラメータを定義する）
     * ・コンストラクタに与える設定DTO
     * ・読取専用モデルクラス
     * を自動生成する。
     * また、各種モデルクラスに共通的に付与するインタフェース
     * ・WodiLib.Sys.IEqualityComparable{T}
     * ・WodLib.Sys.IDeepCloneable{T}
     * とその実装を自動生成する。
     */

    /*
     * IStubModelSettings は SourceGenerator で自動生成されるため、
     * 手作業での作成不要。
     *
     * 以下、自動生成される設定DTOインタフェース
     */
    /*
    /// <summary>
    ///     <see cref="ModelBase"/> スタブ用設定インタフェース
    /// </summary>
    public partial interface IStubModelSettings : WodiLib.Sys.IEqualityComparable<IStubModelSettings>
    {
        /// <inheritdoc cref="ReadOnlyStubModel.StringValue" />
        System.String StringValue { get; }
        /// <inheritdoc cref="ReadOnlyStubModel.Tags" />
        System.Collections.Generic.IReadOnlyList<System.String> Tags { get; }
    }
    */

    /*
     * StubModelSettings は SourceGenerator で自動生成されるため、
     * 本来は手作業での作成不要。
     *
     * 以下、自動生成される設定DTOクラス
     */
    /*
    /// <summary>
    ///     <see cref="ModelBase"/> スタブ用設定DTO
    /// </summary>
    public partial record StubModelSettings : IStubModelSettings
    {
        /// <inheritdoc cref="IStubModelSettings.StringValue" path="summary|remarks" />
        public System.String StringValue { get; set; } = "InitValue";
        /// <inheritdoc cref="IStubModelSettings.Tags" path="summary|remarks" />
        public System.Collections.Generic.IReadOnlyList<System.String> Tags { get; set; } = new [] {"Tag1", "Tag2" };

        /// <inheritdoc/>
        public bool ItemEquals(IStubModelSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return StringValue == other.StringValue;
        }

        /// <inheritdoc/>
        public bool ItemEquals(object? other) => ItemEquals(other as IStubModelSettings);
    }
    */

    /*
     * クラス定義に Model 属性を付与することで
     * SourceGenerator による自動生成の対象とする。
     */
    [Model(Description = "<see cref=\"ModelBase\"/> スタブ用")]
    public partial class StubModel
        /*
         * 以下5つの基底クラス・インタフェースは SourceGenerator が自動的に付与する。
         */
        // ModelBase,
        // IStubModelSettings,
        // IEqualityComparable<StubModel>,
        // IEqualityComparable<ReadOnlyStubModel>,
        // IDeepCloneable<ReadOnlyStubModel>
    {
        /// <summary>
        ///     スタブ用の文字列値
        /// </summary>
        /*
         * SettingProperty を付与することで、設定DTOに実装するプロパティの対象とする。
         */
        [SettingsProperty(
            SetterAccessibility = "public", // "NONE" を設定した場合、設定DTOのプロパティに setter を付与しない
            //                                 デフォルト値が "public" のため、この指定はなくても良い
            ReturnType = typeof(string), // 未指定の場合は読取専用モデルクラスのプロパティと同じ型を扱う
            DefaultValue = "\"InitValue\"" // 未指定の場合は "default" で初期化する
        )]
        /*
         * ImmutableProperty を付与することで、読取専用モデルに実装するプロパティの対象とする。
         *
         * 編集可能のモデルの getter や setter は 読取専用クラスのプロパティにアクセスするだけの動作となる。
         * getter や setter をカスタマイズしたい・扱う型を変えたい場合、自分で実装する必要がある。
         */
        [ImmutableProperty(
            Accessibility = "public" // デフォルト値が "public" のため、この指定はなくても良い
        )]
        public string StringValue
        {
            get => stringValue;
            set => SetField(ref stringValue, value);
        }

        /// <summary>
        ///     タグ一覧
        /// </summary>
        /// <remarks>
        ///     プロパティ変更通知に対応させていないのでテスト時に注意。
        /// </remarks>
        [SettingsProperty(
            SetterAccessibility = "public",
            ReturnType = typeof(IReadOnlyList<string>),
            DefaultValue = "new [] {\"Tag1\", \"Tag2\" }"
        )]
        /*
         * 読取専用モデルではこのプロパティで扱う型を IList<string> としたい。
         * そのため、 MutableProperty 属性を付与しない。
         */
        [ImmutableProperty(
            ReturnType = typeof(IReadOnlyList<string>)
        )]
        public List<string> Tags { get; }

        private string stringValue;

        /*
         * 設定インタフェースを引数に持つコンストラクタを実装する。
         * DeepClone メソッドで使用するコンストラクタとなるため、必ず実装が必要。
         */
        public StubModel(IStubModelSettings settings)
        {
            stringValue = settings.StringValue;
            Tags = new List<string>(settings.Tags);
        }

        /*
         * 設定インタフェース以外を使うコンストラクタは自由に定義可能。
         */
        public StubModel() : this(new StubModelSettings())
        {
        }

        public StubModel(string str) : this(
            new StubModelSettings { StringValue = str, Tags = Array.Empty<string>() }
        )
        {
        }

        /*
         * 純粋メソッド。
         * ImmutableMethod 属性を付与することで読取専用クラスにも定義される。。
         */
        [ImmutableMethod(Accessibility = "public")]
        public string ToJsonString()
        {
            return $"{{\"{nameof(StringValue)}\":\"{StringValue}\"}}";
        }

        /// <summary>
        ///     StringValueに現在の日時文字列をセットする
        /// </summary>
        public void SetNowStringValue()
        {
            /*
             * プロパティを編集するときは、プロパティ変更通知を発火させるため、
             * フィールドではなくプロパティに値をセットする。
             */
            StringValue = new DateTime().ToString("yyyy/MM/dd HH:mm:ss");
        }

        /*
         * ItemEquals(設定インタフェース) は自分で実装が必要。
         *
         * その他の ItemEquals メソッドはすべてこのメソッドに転送される。
         */
        public bool ItemEquals(IStubModelSettings? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return StringValue == other.StringValue;
        }

        public override string ToString()
        {
            return $"{typeof(ReadOnlyStubModel).FullName}{{"
                   + $"{nameof(StringValue)}=\"{StringValue}\""
                   + $"}}";
        }
    }

    /*
     * 以下は SourceGeneratorで生成される編集可能モデルクラス定義のサンプル。
     *
     * クラスのドキュメントコメントは ModelAttribute.Description となる。
     */
    /*

    /// <summary>
    ///     <see cref="ModelBase"/> スタブ用
    /// </summary>
    public partial class StubModel : ModelBase,
        IStubModelSettings,
        IEqualityComparable<StubModel>,
        IEqualityComparable<ReadOnlyStubModel>,
        IDeepCloneable<StubModel>
    {
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(StubModelSettings? other) => ItemEquals(other as IStubModelSettings);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(ReadOnlyStubModel? other) => ItemEquals(other as IStubModelSettings);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(StubModel? other) => ItemEquals(other as IStubModelSettings);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other) => ItemEquals(other as IStubModelSettings);

        /// <inheritdoc/>
        [Pure]
        public StubModel DeepClone() => new(this);
        object IDeepCloneable.DeepClone() => DeepClone();

        System.String IStubModelSettings.StringValue => StringValue;
        System.Collections.Generic.IReadOnlyList<System.String> IStubModelSettings.Tags => Tags;

        private ReadOnlyStubModel? immutableInstance = null;

        /// <summary>
        ///     読取専用クラスへの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(src))]
        public static implicit operator ReadOnlyStubModel?(StubModel? src)
        {
            if (src is null) return null;
            src.immutableInstance ??= new ReadOnlyStubModel(src);
            return src.immutableInstance;
        }
    }
     */

    /*
     * 以下は SourceGeneratorで生成される読取専用クラス定義のサンプル。
     *
     * クラスのドキュメントコメントは ModelAttribute.Description の頭に "【読取専用】" の文字を付与した内容となる。
     */
    /*
    /// <summary>
    ///     【読取専用】<see cref="ModelBase"/> スタブ用
    /// </summary>
    public partial class ReadOnlyStubModel : WodiLib.Sys.ModelBase,
        IStubModelSettings,
        WodiLib.Sys.IEqualityComparable<StubModel>,
        WodiLib.Sys.IEqualityComparable<ReadOnlyStubModel>,
        WodiLib.Sys.IDeepCloneable<ReadOnlyStubModel>
    {
        /// <inheritdoc/>
        public string StringValue => mutableInstance.StringValue;
        /// <inheritdoc/>
        public System.Collections.Generic.IReadOnlyList<string> Tags => mutableInstance.Tags;

        private readonly StubModel mutableInstance;

        internal ReadOnlyStubModel(StubModel mutableInstance)
        {
            this.mutableInstance = mutableInstance;
            PropagatePropertyChangeEvent(mutableInstance);
        }

        /// <inheritdoc/>
        public System.String ToJsonString() => mutableInstance.ToJsonString();

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IStubModelSettings? other) => mutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(StubModelSettings? other) => mutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(ReadOnlyStubModel? other) => mutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(StubModel? other) => mutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other) => mutableInstance.ItemEquals(other);

        /// <inheritdoc/>
        [Pure]
        public ReadOnlyStubModel DeepClone() => new StubModel(this);
        object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();
    }
    */
}
