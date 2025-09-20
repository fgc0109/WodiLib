// ========================================
// Project Name : WodiLib
// File Name    : Publisher.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    ///     値を保持し、値が変化したときに通知を行うクラス
    /// </summary>
    internal class Publisher<T>
    {
        #region Properties

        #region public

        public T Value { get; private set; }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private readonly List<Subscriber> subscribers = new();

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        public Publisher(T initValue)
        {
            Value = initValue;
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        public void Publish(T newValue)
        {
            var updated = UpdateValueIfNeed(newValue);
            if (updated)
            {
                _Publish();
            }
        }

        public Publisher<T> Subscribe(Subscriber subscriber)
        {
            subscribers.Add(subscriber);
            return this;
        }

        public void UnSubscribe(Subscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }

        #endregion

        #region private

        private bool UpdateValueIfNeed(T newValue)
        {
            if (EqualsHelper.NullableEquals(Value, newValue))
            {
                return false;
            }

            Value = newValue;
            return true;
        }

        private void _Publish()
        {
            foreach (var subscriber in subscribers)
            {
                subscriber.Invoke(Value);
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Delegate

        #region public

        public delegate void Subscriber(T newValue);

        #endregion

        #endregion
    }
}
