// ========================================
// Project Name : WodiLib
// File Name    : NotChangeAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    ///     インスタンスが変更されないプロパティであることを示す属性。
    /// </summary>
    /// <remarks>
    ///     この属性が付与されたプロパティはSetterが設けられず、
    ///     参照するインスタンスがクラスメソッドの処理によって変更されることもない。
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Property)]
    public class InstanceNotChangeAttribute : Attribute
    {
    }
}
