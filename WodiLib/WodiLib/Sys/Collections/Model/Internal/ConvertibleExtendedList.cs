// ========================================
// Project Name : WodiLib
// File Name    : ConvertibleExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自リスト
    /// </summary>
    /// <remarks>
    ///     WodiLib内で使用する各種リストの処理転送先となるクラス。
    ///     外部には <typeparamref name="TVisibleItem"/> のリストであるように見せかけるが、
    ///     内部的には <typeparamref name="TOrdinalItem"/> のリストとして実装する。
    ///     <typeparamref name="TOrdinalItem"/> が変更通知を行うクラスだった場合、
    ///     通知を受け取ると自身の "Items[]" プロパティ変更通知を行う。
    /// </remarks>
    /// <typeparam name="TVisibleItem">見せかけのリスト内包クラス</typeparam>
    /// <typeparam name="TOrdinalItem">実際のリスト内包クラス</typeparam>
    internal class ConvertibleExtendedList<TVisibleItem, TOrdinalItem> :
        ModelBase<ConvertibleExtendedList<TVisibleItem, TOrdinalItem>>,
        IExtendedList<TVisibleItem>
    {
        /*
         * WodiLib 内部で使用する独自汎用リスト。
         * リスト本体の機能は SimpleList<T> に委譲。
         */
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Events
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => Items.CollectionChanged += value;
            remove => Items.CollectionChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public TVisibleItem this[int index]
        {
            get
            {
                this.ValidateGet(index);
                return this.GetCore(index);
            }
            set
            {
                this.ValidateSet(index, value);
                this.SetCore(index, value);
            }
        }

        public int Count => Items.Count;

        public Func<int, TVisibleItem> MakeDefaultItem { get; }

        public IWodiLibListValidator<TVisibleItem>? Validator => null;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト本体</summary>
        protected virtual ExtendedList<TOrdinalItem> Items { get; }

        /// <summary>
        ///     <typeparamref name="TOrdinalItem"/> から <typeparamref name="TVisibleItem"/> への変換処理
        /// </summary>
        /// <remarks>
        ///     <typeparamref name="TOrdinalItem"/> から <typeparamref name="TVisibleItem"/> へ直接変換できる場合は <see langword="null"/>。
        /// </remarks>
        protected virtual Func<TOrdinalItem, TVisibleItem>? ItemConverter { get; }

        /// <summary>
        ///     <typeparamref name="TVisibleItem"/> から <typeparamref name="TOrdinalItem"/> への変換処理
        /// </summary>
        /// <remarks>
        ///     <typeparamref name="TVisibleItem"/> から <typeparamref name="TOrdinalItem"/> へ直接変換できる場合は <see langword="null"/>。
        /// </remarks>
        protected virtual Func<TVisibleItem, TOrdinalItem>? ItemReverseConverter { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="items">デフォルト要素生成処理</param>
        /// <param name="itemConverter">
        ///     <typeparamref name="TOrdinalItem"/> から <typeparamref name="TVisibleItem"/> への変換処理<br/>
        ///     <typeparamref name="TOrdinalItem"/> から <typeparamref name="TVisibleItem"/> へ直接変換できる場合は <see langword="null"/>
        ///     を指定して良い。
        /// </param>
        /// <param name="itemReverseConverter">
        ///     <typeparamref name="TVisibleItem"/> から <typeparamref name="TOrdinalItem"/> への変換処理<br/>
        ///     <typeparamref name="TVisibleItem"/> から <typeparamref name="TOrdinalItem"/> へ直接変換できる場合は <see langword="null"/>
        ///     を指定して良い。
        /// </param>
        public ConvertibleExtendedList(
            ExtendedList<TOrdinalItem> items,
            Func<TOrdinalItem, TVisibleItem>? itemConverter,
            Func<TVisibleItem, TOrdinalItem>? itemReverseConverter
        )
        {
            Items = items;
            ItemConverter = itemConverter;
            ItemReverseConverter = itemReverseConverter;
            MakeDefaultItem = i => CastOrdinalItemToVisibleItem(Items.MakeDefaultItem(i));

            PropagatePropertyChangeEvent(Items);
        }

        /// <summary>
        ///     ディープコピーコンストラクタ
        /// </summary>
        /// <param name="src">コピー元</param>
        private ConvertibleExtendedList(
            ConvertibleExtendedList<TVisibleItem, TOrdinalItem> src
        ) : this(src.Items.DeepClone(), src.ItemConverter, src.ItemReverseConverter)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public IEnumerator<TVisibleItem> GetEnumerator() => GetCastedItems().GetEnumerator();

        public IEnumerable<TVisibleItem> GetRangeCore(int index, int count)
            => CastOrdinalItemsToVisibleItems(Items.GetRangeCore(index, count));

        public void SetRangeCore(int index, IEnumerable<TVisibleItem> items) => Items.SetRangeCore(
            index,
            ReverseCastOrdinalItemsToVisibleItems(items).ToArray()
        );

        public void InsertRangeCore(int index, IEnumerable<TVisibleItem> items) => Items.InsertRangeCore(
            index,
            ReverseCastOrdinalItemsToVisibleItems(items).ToArray()
        );

        public void OverwriteCore(int index, IEnumerable<TVisibleItem> items) => Items.OverwriteCore(
            index,
            ReverseCastOrdinalItemsToVisibleItems(items).ToArray()
        );

        public void MoveRangeCore(int oldIndex, int newIndex, int count)
            => Items.MoveRangeCore(oldIndex, newIndex, count);

        public void RemoveRangeCore(int index, int count) => Items.RemoveRangeCore(index, count);
        public void AdjustLengthCore(int length) => Items.AdjustLengthCore(length);

        public void ResetCore(IEnumerable<TVisibleItem> items)
            => Items.Reset(ReverseCastOrdinalItemsToVisibleItems(items).ToArray());

        public void ClearCore() => Items.Clear();

        public bool ItemEquals(IExtendedList<TVisibleItem>? other)
            => ItemEquals((IEnumerable<TVisibleItem>?)other);

        public override bool ItemEquals(ConvertibleExtendedList<TVisibleItem, TOrdinalItem>? other)
            => ItemEquals((IEnumerable<TVisibleItem>?)other);

        public bool ItemEquals(IEnumerable<TVisibleItem>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var otherItemArray = other.ToArray();
            return Count == otherItemArray.Length
                   && this.Zip(otherItemArray)
                       .All(
                           zip => zip.Item1 is IEqualityComparable equalityComparable
                               ? equalityComparable.ItemEquals(zip.Item2)
                               : zip.Item1!.Equals(zip.Item2)
                       );
        }

        public override bool ItemEquals(object? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other is IEnumerable<TVisibleItem> enumerable)
            {
                return ItemEquals(enumerable);
            }

            return Equals(other);
        }

        public override ConvertibleExtendedList<TVisibleItem, TOrdinalItem> DeepClone() => new(this);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implementation
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region GetEnumerator

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     <see cref="Items"/> の各要素を <typeparamref name="TVisibleItem"/> にキャストした
        ///     <see cref="IEnumerable{T}"/> を取得する。
        /// </summary>
        /// <returns>キャストした要素の列挙</returns>
        private IEnumerable<TVisibleItem> GetCastedItems()
            => Items.Select(CastOrdinalItemToVisibleItem);

        /// <summary>
        ///     <typeparamref name="TOrdinalItem"/> を <typeparamref name="TVisibleItem"/> にキャストする。
        /// </summary>
        /// <param name="src">キャスト元</param>
        /// <returns>キャストした結果</returns>
        private IEnumerable<TVisibleItem> CastOrdinalItemsToVisibleItems(IEnumerable<TOrdinalItem> src)
            => src.Select(CastOrdinalItemToVisibleItem);

        /// <summary>
        ///     <typeparamref name="TOrdinalItem"/> を <typeparamref name="TVisibleItem"/> にキャストする。
        /// </summary>
        /// <param name="src">キャスト元</param>
        /// <returns>キャストした結果</returns>
        private TVisibleItem CastOrdinalItemToVisibleItem(TOrdinalItem src)
        {
            if (ItemConverter != null)
            {
                return ItemConverter(src);
            }

            return (TVisibleItem)(src as object)!;
        }

        /// <summary>
        ///     <typeparamref name="TVisibleItem"/> を <typeparamref name="TOrdinalItem"/> にキャストする。
        /// </summary>
        /// <param name="src">キャスト元</param>
        /// <returns>キャストした結果</returns>
        private IEnumerable<TOrdinalItem> ReverseCastOrdinalItemsToVisibleItems(IEnumerable<TVisibleItem> src)
            => src.Select(ReverseCastOrdinalItemToVisibleItem);

        /// <summary>
        ///     <typeparamref name="TVisibleItem"/> を <typeparamref name="TOrdinalItem"/> にキャストする。
        /// </summary>
        /// <param name="src">キャスト元</param>
        /// <returns>キャストした結果</returns>
        private TOrdinalItem ReverseCastOrdinalItemToVisibleItem(TVisibleItem src)
        {
            if (ItemReverseConverter != null)
            {
                return ItemReverseConverter(src);
            }

            return (TOrdinalItem)(src as object)!;
        }
    }
}
