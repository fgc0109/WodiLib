// ========================================
// Project Name : WodiLib
// File Name    : PropertyValue.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.SourceGenerator.Core.Dtos
{
    internal class PropertyValue
    {
        private readonly string value;

        public PropertyValue(string value)
        {
            this.value = value;
        }

        public override string ToString() => value;
    }
}
