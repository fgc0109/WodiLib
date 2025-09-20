// ========================================
// Project Name : WodiLib.Test
// File Name    : MockWodiLibListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     <see cref="IWodiLibListValidator{T}"/> モック用
    /// </summary>
    internal class MockWodiLibListValidator<T> : MockBase<IWodiLibListValidator<T>>,
        IWodiLibListValidator<T>
    {
        public void Constructor(NamedValue<IEnumerable<T>> initItems)
        {
            AddCalledHistory(nameof(Constructor), initItems.Value);
        }

        public void Get(NamedValue<int> index, NamedValue<int> count)
        {
            AddCalledHistory(nameof(Get), index.Value, count.Value);
        }

        public void Set(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
        {
            AddCalledHistory(nameof(Set), index.Value, items.Value);
        }

        public void Insert(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
        {
            AddCalledHistory(nameof(Insert), index.Value, items.Value);
        }

        public void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
        {
            AddCalledHistory(nameof(Overwrite), index.Value, items.Value);
        }

        public void Move(NamedValue<int> oldIndex, NamedValue<int> newIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(Move), oldIndex.Value, newIndex.Value, count.Value);
        }

        public void Remove(NamedValue<int> index, NamedValue<int> count)
        {
            AddCalledHistory(nameof(Remove), index.Value, count.Value);
        }

        public void AdjustLength(NamedValue<int> length)
        {
            AddCalledHistory(nameof(AdjustLength), length.Value);
        }

        public void Reset(NamedValue<IEnumerable<T>> settings, bool canChangeSize = true)
        {
            AddCalledHistory(nameof(Reset), settings.Value, canChangeSize);
        }

        public void Reset()
        {
            AddCalledHistory(nameof(Reset));
        }

        public void Clear()
        {
            AddCalledHistory(nameof(Clear));
        }
    }
}
