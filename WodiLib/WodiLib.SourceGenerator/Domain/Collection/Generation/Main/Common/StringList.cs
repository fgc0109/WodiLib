// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : StringList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Text;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    /// <summary>
    /// </summary>
    internal class StringList
    {
        private readonly StringBuilder stringBuilder = new();

        public StringList AppendLine(string str)
        {
            stringBuilder.AppendLine(str);
            return this;
        }

        public StringList Append(string str)
        {
            stringBuilder.Append(str);
            return this;
        }

        public StringList Append(IEnumerable<string> str)
        {
            stringBuilder.Append(string.Join(Environment.NewLine, str));
            return this;
        }

        public string[] ToArray()
        {
            return stringBuilder.ToString().Replace("\r\n", "\n").Split('\n');
        }
    }
}
