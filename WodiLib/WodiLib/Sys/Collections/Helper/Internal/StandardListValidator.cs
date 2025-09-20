// ========================================
// Project Name : WodiLib
// File Name    : StandardListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="TElementSettings">リスト内包型の入力パラメータ型</typeparam>
    internal class StandardListValidator<TElementSettings> : IWodiLibListValidator<TElementSettings>
    {
        public delegate int GetCountDelegate();

        protected GetCountDelegate CountGetter { get; }

        public StandardListValidator(GetCountDelegate countGetter)
        {
            CountGetter = countGetter;
        }

        public virtual void Constructor(NamedValue<IEnumerable<TElementSettings>> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems.Value is null, initItems.Name);
            ListValidationHelper.ItemsHasNotNull(initItems);
        }

        public virtual void Get(NamedValue<int> index, NamedValue<int> count)
        {
            var namedCount = new NamedValue<int>("Count", CountGetter.Invoke());

            ListValidationHelper.SelectIndex(index, namedCount);
            ListValidationHelper.Count(count, namedCount);
            ListValidationHelper.Range(index, count, namedCount);
        }

        public virtual void Set(NamedValue<int> index, NamedValue<IEnumerable<TElementSettings>> items)
        {
            var namedCount = new NamedValue<int>("Count", CountGetter.Invoke());

            ListValidationHelper.SelectIndex(index, namedCount);
            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.ItemsHasNotNull(items);
            ListValidationHelper.Range(
                index,
                ($"{nameof(items)}の要素数", items.Value.Count()),
                namedCount
            );
        }

        public virtual void Insert(NamedValue<int> index, NamedValue<IEnumerable<TElementSettings>> items)
        {
            var namedCount = new NamedValue<int>("Count", CountGetter.Invoke());

            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.ItemsHasNotNull(items);
            ListValidationHelper.InsertIndex(
                index,
                namedCount
            );
        }

        public virtual void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<TElementSettings>> items)
        {
            var namedCount = new NamedValue<int>("Count", CountGetter.Invoke());

            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.ItemsHasNotNull(items);
            ListValidationHelper.InsertIndex(
                index,
                namedCount
            );
        }

        public virtual void Move(NamedValue<int> oldIndex, NamedValue<int> newIndex, NamedValue<int> count)
        {
            var namedCount = new NamedValue<int>("Count", CountGetter.Invoke());

            ListValidationHelper.ItemCountNotZero(namedCount);
            ListValidationHelper.SelectIndex(oldIndex, namedCount);
            ListValidationHelper.InsertIndex(newIndex, namedCount);
            ListValidationHelper.Count(count, namedCount);
            ListValidationHelper.Range(oldIndex, count, namedCount);
            ListValidationHelper.Range(count, newIndex, namedCount);
        }

        public virtual void Remove(NamedValue<int> index, NamedValue<int> count)
        {
            var namedCount = new NamedValue<int>("Count", CountGetter.Invoke());

            ListValidationHelper.SelectIndex(index, namedCount);
            ListValidationHelper.Count(count, namedCount);
            ListValidationHelper.Range(index, count, namedCount);
        }

        public virtual void AdjustLength(NamedValue<int> length)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                length.Value < 0,
                nameof(length),
                0,
                length.Value
            );
        }

        public virtual void Reset(NamedValue<IEnumerable<TElementSettings>> settings, bool canChangeSize = true)
        {
            ThrowHelper.ValidateArgumentNotNull(settings.Value is null, settings.Name);
            ListValidationHelper.ItemsHasNotNull(settings);

            if (!canChangeSize)
            {
                var namedCount = new NamedValue<int>("Count", CountGetter.Invoke());

                ListValidationHelper.ItemCount(
                    count: settings.Value.Count(),
                    capacity: namedCount.Value,
                    itemName: namedCount.Name
                );
            }
        }

        public virtual void Reset()
        {
            // 無条件で可能
        }

        public virtual void Clear()
        {
            // 無条件で可能
        }
    }
}
