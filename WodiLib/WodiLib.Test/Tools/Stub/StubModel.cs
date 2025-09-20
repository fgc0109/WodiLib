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
     */

    /*
     * IStubModelSettings は SourceGenerator で自動生成されるため、
     * 手作業での作成不要。
     *
     * 以下、自動生成される設定DTOインタフェース
     */
    // public partial interface IStubModelSettings :
    //     IEqualityComparable<IStubModelSettings>
    // {
    //     /*
    //      * 設定DTOが実装するプロパティは ReadOnlyModel のプロパティ定義に付与した
    //      * SettingsProperty から拾う。
    //      */
    //     /// <inheritdoc cref="ReadOnlyStubModel.StringValue"/>
    //     public string StringValue { get; }
    // }

    /*
     * StubModelSettings は SourceGenerator で自動生成されるため、
     * 本来は手作業での作成不要。
     *
     * 以下、自動生成される設定DTOクラス
     */
    // public partial record StubModelSettings() : IStubModelSettings
    // {
    //     /*
    //      * 設定DTOが実装するプロパティの初期値は ReadOnlyModel のプロパティ定義に付与した
    //      * SettingsProperty から拾う。
    //      *
    //      * すべてのプロパティについて、setter は init ではなく setとする。
    //      */
    //     /// <inheritdoc cref="ReadOnlyStubModel.StringValue"/>
    //     public string StringValue { get; set; } = "InitValue";
    //
    //     public IReadOnlyList<string> Tags { get; set; } = new List<string> { "Tag1", "Tag2" };
    //
    //     /*
    //      * ItemEquals(設定DTO) の実装は ReadOnlyModel に実装した ItemEquals メソッドを
    //      * そのままコピーする。
    //      */
    //     public bool ItemEquals(IStubModelSettings? other)
    //     {
    //         if (ReferenceEquals(null, other)) return false;
    //         if (ReferenceEquals(this, other)) return true;
    //         return StringValue == other.StringValue;
    //     }
    //
    //     /*
    //      * ItemEquals(object) の実装は、 other を 設定DTO に変換して比較する処理一択となる。
    //      */
    //     public bool ItemEquals(object? other) => ItemEquals(other as IStubModelSettings);
    // }

    /*
     * 読取専用クラス定義に Model 属性を付与することで
     * SourceGenerator による自動生成の対象とする。
     *
     * クラスのドキュメントコメントは
     */
    [Model(Description = "<see cref=\"ModelBase\"/> スタブ用")]
    public partial class ReadOnlyStubModel
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
         * MutableProperty を付与することで、編集可能モデルに実装するプロパティの対象とする。
         *
         * 編集可能のモデルの getter や setter は 読取専用クラスのプロパティにアクセスするだけの動作となる。
         * getter や setter をカスタマイズしたい・扱う型を変えたい場合、自分で実装する必要がある。
         */
        [MutableProperty(
            Accessibility = "public" // "NONE" を設定した場合、設定DTOのプロパティに setter を付与しない
            //                          デフォルト値が "public" のため、この指定はなくても良い
        )]
        public string StringValue
        {
            get => stringValue;
            protected set => SetField(ref stringValue, value);
        }

        /// <summary>
        ///     タグ一覧
        /// </summary>
        /// <remarks>
        ///     プロパティ変更通知に対応させていないのでテスト時に注意。
        /// </remarks>
        [SettingsProperty(
            SetterAccessibility = "public",
            DefaultValue = "new [] {\"Targ1\", \"Tag2\" }"
        )]
        /*
         * 編集可能モデルではこのプロパティで扱う型を IList<string> としたい。
         * そのため、 MutableProperty 属性を付与しない。
         */
        public IReadOnlyList<string> Tags => tags;

        private string stringValue;
        protected readonly List<string> tags = new();

        /*
         * MutableConstructor 属性を付与すると、
         * 編集可能クラスにもコンストラクタとして定義する。
         *
         * 編集可能クラスのコンストラクタではすべての引数を base コンストラクタに渡す処理となる。
         */
        [MutableConstructor]
        public ReadOnlyStubModel(string str = "")
        {
            stringValue = str;
        }

        /*
         * 設定DTOを引数に持つコンストラクタを実装する。
         * DeepClone メソッドで使用するコンストラクタとなるため、必ず実装が必要。
         * また、 MutableConstructor 属性も必須。
         */
        [MutableConstructor]
        public ReadOnlyStubModel(IStubModelSettings settings) : this(settings.StringValue)
        {
        }

        /*
         * 純粋メソッド。
         * 編集可能モデルクラスでも通常使用可能なため、属性はつけない。
         */
        public string ToJsonString()
        {
            return $"{{\"{nameof(StringValue)}\":\"{StringValue}\"}}";
        }

        /// <summary>
        ///     StringValueに現在の日時文字列をセットする
        /// </summary>
        /*
         * MutableMethod 属性を付与すると、
         * 編集可能モデルにも同じメソッドが定義される。
         *
         * メソッドの実装は読取専用クラスのメソッドへの転送となる。
         */
        [MutableMethod(
            Accessibility = "public" // デフォルト値が "public" のため、この指定はなくても良い
        )]
        protected void SetNowStringValue()
        {
            /*
             * プロパティを編集するときは、プロパティ変更通知を発火させるため、
             * フィールドではなくプロパティに値をセットする。
             */
            StringValue = new DateTime().ToString("yyyy/MM/dd HH:mm:ss");
        }

        /*
         * ItemEquals(設定DTO) は自分で実装が必要。
         *
         * 実装した ItemEquals(設定DTO) メソッドは
         * 設定DTO クラスにもコピーされる。
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

        /*
         * 以下の ItemEquals メソッドは実装不要。
         * SourceGenerator で自動付与する。
         */
        // public bool ItemEquals(ReadOnlyStubModel? other) => ItemEquals(other as IStubModelSettings);
        // public bool ItemEquals(StubModel? other) => ItemEquals(other as IStubModelSettings);
        // public bool ItemEquals(object? other) => ItemEquals(other as IStubModelSettings);

        /*
         * 以下の DeepClone メソッドは実装不要。
         * SourceGenerator で自動付与する。
         */
        // public ReadOnlyStubModel DeepClone() => new(this);
        // object IDeepCloneable.DeepClone() => DeepClone();
    }

    /*
     * 以下は SourceGeneratorで生成されるクラス定義のサンプル。
     *
     * クラスのドキュメントコメントは ModelAttribute.Description の頭に "【読取専用】" の文字を付与した内容となる。
     */
    // /// <summary>
    // ///     【読取専用】<see cref="ModelBase"/> スタブ用
    // /// </summary>
    // public partial class ReadOnlyStubModel :
    //     /* 以下のインタフェースが付与される */
    //     ModelBase,
    //     IStubModelSettings,
    //     IEqualityComparable<StubModel>,
    //     IEqualityComparable<ReadOnlyStubModel>,
    //     IDeepCloneable<ReadOnlyStubModel>
    // {
    //     /*
    //      * 自動生成で付与されたインタフェースのクラスのうち、
    //      * 以下のメソッドは同時に SourceGenerator 側で定義を実装する。
    //      */
    //     public bool ItemEquals(ReadOnlyStubModel? other) => ItemEquals(other as IStubModelSettings);
    //     public bool ItemEquals(StubModel? other) => ItemEquals(other as IStubModelSettings);
    //     public bool ItemEquals(object? other) => ItemEquals(other as IStubModelSettings);
    //
    //     public ReadOnlyStubModel DeepClone() => new(this);
    //     object IDeepCloneable.DeepClone() => DeepClone();
    // }

    public partial class StubModel
    {
        public new IList<string> Tags => tags;
    }

    /*
     * 以下は SourceGeneratorで生成されるクラス定義のサンプル。
     *
     * クラスのドキュメントコメントは ModelAttribute.Description となる。
     */
    // /// <summary>
    // ///     <see cref="ModelBase"/> スタブ用
    // /// </summary>
    // public partial class StubModel : ReadOnlyStubModel,
    //     IDeepCloneable<StubModel>
    // {
    //     /// <inheritdoc cref="ReadOnlyStubModel.StringValue"/>
    //     public new string StringValue
    //     {
    //         get => base.StringValue;
    //         set => SetField(value, () => base.StringValue, v => base.StringValue = v);
    //     }
    //
    //     public StubModel(string str = "") : base(str)
    //     {
    //     }
    //
    //     public StubModel(IStubModelSettings settings) : base(settings)
    //     {
    //     }
    //
    //     public new void SetNowStringValue() => base.SetNowStringValue();
    //
    //     public new StubModel DeepClone() => new(this);
    //     object IDeepCloneable.DeepClone() => DeepClone();
    // }
}
