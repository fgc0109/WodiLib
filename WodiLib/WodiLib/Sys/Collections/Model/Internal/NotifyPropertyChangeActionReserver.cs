// ========================================
// Project Name : WodiLib
// File Name    : NotifyPropertyChangeActionReserver.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     <see cref="INotifyPropertyChanged"/> イベントを一時保留してまとめて放出するための処理を扱うクラス
    /// </summary>
    internal class NotifyPropertyChangeActionReserver
    {
        private bool reserving = false;
        private readonly List<string> notifiedPropertyNames;

        private readonly Action<string> actionNotifyPropertyChange;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="actionNotifyPropertyChange">プロパティ変更通知処理</param>
        public NotifyPropertyChangeActionReserver(Action<string> actionNotifyPropertyChange)
        {
            this.actionNotifyPropertyChange = actionNotifyPropertyChange;
            notifiedPropertyNames = new List<string>();
        }

        /// <summary>
        ///     プロパティ変更通知を行う。
        /// </summary>
        /// <remarks>
        ///     通知保留中の場合、即座に通知は行わず <see cref="FinishReserve"/> が呼ばれるまで通知を待つ。
        /// </remarks>
        /// <param name="propertyName"></param>
        public void Notify(string propertyName)
        {
            if (!reserving)
            {
                actionNotifyPropertyChange(propertyName);
                return;
            }

            // 重複しないものだけを溜め込んでおく
            if (notifiedPropertyNames.Contains(propertyName)) return;

            notifiedPropertyNames.Add(propertyName);
        }

        /// <summary>
        ///     プロパティ変更通知の保留を開始する。
        /// </summary>
        public void StartReserve()
        {
            reserving = true;
        }

        /// <summary>
        ///     保留しているプロパティ変更通知を通知する。
        /// </summary>
        public void Release()
        {
            notifiedPropertyNames.ForEach(actionNotifyPropertyChange);
            notifiedPropertyNames.Clear();
        }

        /// <summary>
        ///     プロパティ変更通知の保留を終了する。
        /// </summary>
        public void FinishReserve()
        {
            reserving = false;
        }
    }
}
