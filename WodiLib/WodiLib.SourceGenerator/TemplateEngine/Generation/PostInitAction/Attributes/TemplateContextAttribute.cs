// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TemplateContextAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;

namespace WodiLib.SourceGenerator.TemplateEngine.Generation.PostInitAction.Attributes
{
    /// <summary>
    /// </summary>
    internal class TemplateContextAttribute : InitializeAttributeSourceAddable
    {
        public override string AttributeName => nameof(TemplateContextAttribute);

        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        public override AttributeTargets AttributeTargets => AttributeTargets.Class;

        public override string Summary => "テンプレート情報";

        public override string Remarks => "テンプレート文字列からpartialクラスを生成したい場合に付与する属性。<br/>"
                                          + "テンプレートとなる文字列と、文字列内で置換したいKeyValueを指定する。<br/>"
                                          + "生成した文字列を、この属性を付与したクラスのボディとして出力する。";

        public static readonly PropertyInfo OutKey = new()
        {
            Name = nameof(OutKey),
            Type = typeof(string).FullName!,
            Summary = "出力ファイル名に付与するキー文字列",
            Remarks = "TemplateContextAttribute によるソースコード出力時に出力ファイル名が被らないようにするための文字列。<br/>"
                      + "出力ファイル名は 名前空間+クラス名_OutKey.cs となる。",
        };

        public static readonly PropertyInfo ClassBodyTemplateLiteral = new()
        {
            Name = nameof(ClassBodyTemplateLiteral),
            Type = typeof(string).FullName!,
            Summary = "出力テンプレートとなる文字列",
            Remarks = "この文字列中の \"#\" で囲まれた文字列をパラメータとし、<see cref=\"Args\"/> で与えた文字列に置換する。",
        };

        public static readonly PropertyInfo Args = new()
        {
            Name = nameof(Args),
            Type = typeof(string[]).FullName!,
            Summary = "テンプレート内で置換する文字列土地看護の値セット一覧",
            Remarks = "\"KEY=VALUE\" の形式で指定する。テンプレート文字列中の \"#KEY#\" を探し、VALUE に置換する。",
        };

        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                OutKey,
                ClassBodyTemplateLiteral,
                Args,
            };

        private TemplateContextAttribute()
        {
        }

        public static TemplateContextAttribute Instance { get; } = new();
    }
}
