using System;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class PublisherTest
    {
        private static Logger logger = null!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [Test]
        public static void PublishAndSubscribeTest1_struct()
        {
            /*
             * 構造体の値の変更が通知されること
             */
            const int initValue = 0;
            const int updateValue = 5;

            var publisher = new Publisher<int>(initValue);

            var published = false;

            var subscriber = BuildSubscriber<int>(notifiedValue =>
                {
                    // 更新後の値が通知されること
                    Assert.AreEqual(updateValue, notifiedValue);

                    published = true;
                }
            );
            publisher.Subscribe(subscriber);

            publisher.Publish(updateValue);

            // Subscriber が呼ばれていること
            Assert.IsTrue(published);

            logger.Info("Passed 1.");

            // --------------------
            /*
             * 値が同じ時、通知されないこと
             */
            published = false;

            publisher.UnSubscribe(subscriber);

            publisher.Publish(updateValue);

            // Subscriber が呼ばれていないこと
            Assert.IsFalse(published);

            logger.Info("Passed 2.");

            // --------------------
            /*
             * Subscriber 解除した後通知されないこと
             */
            published = false;

            publisher.UnSubscribe(subscriber);

            publisher.Publish(initValue);

            // Subscriber が呼ばれていないこと
            Assert.IsFalse(published);
        }

        [Test]
        public static void PublishAndSubscribeTest2_class()
        {
            /*
             * クラスの値の変更が通知されること
             */
            var initValue = new DummyClass(0);
            var updateValue = new DummyClass(5);

            var publisher = new Publisher<DummyClass?>(initValue);

            var published = false;

            var subscriber = BuildSubscriber<DummyClass?>(_ => { published = true; });
            publisher.Subscribe(subscriber);

            publisher.Publish(updateValue);

            // Subscriber が呼ばれていること
            Assert.IsTrue(published);

            logger.Info("Passed 1.");

            // --------------------
            /*
             * 非null の状態から null を設定したとき、通知されること
             */
            published = false;

            publisher.Publish(null);

            // Subscriber が呼ばれていること
            Assert.IsTrue(published);

            logger.Info("Passed 2.");

            // --------------------
            /*
             * null の状態から 非null を設定したとき、通知されること
             */
            published = false;

            publisher.Publish(updateValue);

            // Subscriber が呼ばれていること
            Assert.IsTrue(published);
        }

        [Test]
        public static void PublishAndSubscribeTest3_model()
        {
            /*
             * モデルクラスの値の変更が通知されること
             */
            var initValue = new DummyModelClass(0);
            var updateValue = new DummyModelClass(5);

            var publisher = new Publisher<DummyModelClass>(initValue);

            var published = false;

            var subscriber = BuildSubscriber<DummyModelClass>(_ => { published = true; });
            publisher.Subscribe(subscriber);

            publisher.Publish(updateValue);

            // Subscriber が呼ばれていること
            Assert.IsTrue(published);
        }

        private static Publisher<T>.Subscriber BuildSubscriber<T>(Action<T> callback)
        {
            return callback.Invoke;
        }

        private class DummyClass : IEquatable<DummyClass>
        {
            private readonly int value;

            public DummyClass(int value)
            {
                this.value = value;
            }

            public bool Equals(DummyClass? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return value == other.value;
            }

            public static bool operator ==(DummyClass? l, DummyClass? r)
            {
                return l?.Equals(r) ?? false;
            }

            public static bool operator !=(DummyClass? l, DummyClass? r)
            {
                return !(l?.Equals(r) ?? false);
            }

            public override int GetHashCode()
            {
                return value.GetHashCode();
            }

            public override bool Equals(object? obj) => Equals(obj as DummyClass);
        }

        private class DummyModelClass : IEqualityComparable<DummyModelClass>
        {
            private readonly int value;

            public DummyModelClass(int value)
            {
                this.value = value;
            }

            public bool ItemEquals(DummyModelClass? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return value == other.value;
            }

            public bool ItemEquals(object? other) => ItemEquals(other as DummyModelClass);
        }
    }
}
