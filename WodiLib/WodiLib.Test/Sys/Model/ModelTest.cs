using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class ModelTest
    {
        [Test]
        public static void NotifyPropertyChangedTest()
        {
            var notified = new List<string>();

            var model = new BasicModel();
            model.PropertyChanged += (_, args) => { notified.Add(args.PropertyName!); };

            const double setValue = 0.8;

            {
                // 違う値をセットする、通知されること
                //      初期値: 1.5
                model.Eyesight = setValue;
                Assert.AreEqual(1, notified.Count);
                Assert.AreEqual(nameof(model.Eyesight), notified[0]);
            }

            notified.Clear();

            {
                // 同じ値をセットする、通知されること
                //         NotifyPropertyChanged() 単体での使用は値変化を意識せず通知する
                model.Eyesight = setValue;
                Assert.AreEqual(1, notified.Count);
                Assert.AreEqual(nameof(model.Eyesight), notified[0]);
            }
        }

        [Test]
        public static void SetFieldTest1()
        {
            var notified = new List<string>();

            var model = new BasicModel();
            model.PropertyChanged += (_, args) => { notified.Add(args.PropertyName!); };

            const string setValue = "太郎";

            {
                // 違う値をセットする、通知されること
                //      初期値: 空文字
                model.Name = setValue;
                Assert.AreEqual(1, notified.Count);
                Assert.AreEqual(nameof(model.Name), notified[0]);
            }

            notified.Clear();

            {
                // 同じ値をセットする、通知されないこと
                //      SetField を使用する場合は値の変化を意識する
                model.Name = setValue;
                Assert.AreEqual(0, notified.Count);
            }
        }

        [Test]
        public static void SetFieldTest2()
        {
            var notified = new List<string>();

            var model = new BasicModel();
            model.PropertyChanged += (_, args) => { notified.Add(args.PropertyName!); };

            const string setValue = "失敗は成功のもと";

            {
                // 違う値をセットする、通知されること
                //      初期値: 空文字
                model.Motto = setValue;
                Assert.AreEqual(1, notified.Count);
                Assert.AreEqual(nameof(model.Motto), notified[0]);
            }

            notified.Clear();

            {
                // 同じ値をセットする、通知されないこと
                //      SetField を使用する場合は値の変化を意識する
                model.Motto = setValue;
                Assert.AreEqual(0, notified.Count);
            }
        }

        [Test]
        public static void ProcessWithOtherPropertyChangeNotificationTest()
        {
            var notified = new List<string>();

            var model = new BasicModel();
            model.PropertyChanged += (_, args) => { notified.Add(args.PropertyName!); };

            {
                // 違う値をセットする、通知されること
                //      初期値: 0
                model.Age = 15;
                Assert.AreEqual(2, notified.Count);
                Assert.AreEqual(nameof(model.Age), notified[0]);
                Assert.AreEqual(nameof(model.IsTeenager), notified[1]);
            }

            notified.Clear();

            {
                // 同じ値をセットする、通知されないこと
                model.Age = 15;
                Assert.AreEqual(0, notified.Count);
            }

            notified.Clear();

            {
                // 違う値をセットする、 IsTeenager は同じ値(true)
                //      Age だけが通知されること
                model.Age = 17;
                Assert.AreEqual(1, notified.Count);
                Assert.AreEqual(nameof(model.Age), notified[0]);
            }

            notified.Clear();

            {
                // 違う値をセットする、 IsTeenager も異なる値に変化(false)
                //      Age, IsTeenager が通知されること
                model.Age = 21;
                Assert.AreEqual(2, notified.Count);
                Assert.AreEqual(nameof(model.Age), notified[0]);
                Assert.AreEqual(nameof(model.IsTeenager), notified[1]);
            }
        }

        [Test]
        public static void PropagatePropertyChangeEventTest1()
        {
            var notified = new List<string>();

            var model = new BasicModel();
            model.PropertyChanged += (_, args) => { notified.Add(args.PropertyName!); };

            const string setValue = "Foo";

            {
                // 違う値をセットする、通知されること
                // ただし内部モデル通知転送設定時に名前の変換処理をかけていないので、
                // 内部モデルのプロパティ名のまま通知されること
                //      初期値: 空文字
                model.Tag = setValue;
                Assert.AreEqual(1, notified.Count);
                Assert.AreEqual(nameof(TextModel.Text), notified[0]);
            }

            notified.Clear();

            {
                // 同じ値をセットする、通知されないこと
                //      SetField を使用する場合は値の変化を意識する
                model.Tag = setValue;
                Assert.AreEqual(0, notified.Count);
            }
        }

        [Test]
        public static void PropagatePropertyChangeEventTest2()
        {
            var notified = new List<string>();

            var model = new BasicModel();
            model.PropertyChanged += (_, args) => { notified.Add(args.PropertyName!); };

            const string setValue = "日本";

            {
                // 違う値をセットする、通知されること
                //      初期値: 空文字
                model.Country = setValue;
                // 【前提条件】Country, City が置き換わっていること
                Assert.AreEqual(setValue, model.Country);
                Assert.AreEqual(setValue, model.City);

                // Country だけが通知されること、 City は通知されないこと
                // （そのような通知転送設定を行っているため）
                Assert.AreEqual(1, notified.Count);
                Assert.AreEqual(nameof(model.Country), notified[0]);
            }
        }

        [Test]
        public static void PropagatePropertyChangeEventTest3()
        {
            var notified = new List<string>();

            var model = new BasicModel();
            model.PropertyChanged += (_, args) => { notified.Add(args.PropertyName!); };

            const string setValue = "箸";

            {
                // 【前提条件】LeftHand, RightHand, Having の値
                Assert.AreEqual(null, model.LeftHand);
                Assert.AreEqual(null, model.RightHand);
                Assert.AreEqual(false, model.Having);

                // 違う値をセットする、通知されること
                //      初期値: null
                model.LeftHand = setValue;
                // 【前提条件】LeftHand, Having が置き換わっていること
                Assert.AreEqual(setValue, model.LeftHand);
                Assert.AreEqual(null, model.RightHand);
                Assert.AreEqual(true, model.Having);

                // LeftHand だけが通知されること、 RightHand, Having は通知されないこと
                // （そのような通知転送設定を行っているため）
                Assert.AreEqual(1, notified.Count);
                Assert.AreEqual(nameof(model.LeftHand), notified[0]);
            }

            notified.Clear();

            {
                model.RightHand = "茶碗";
                // RightHand が通知されないこと
                // （そのような通知転送設定を行っているため）
                Assert.AreEqual(0, notified.Count);
            }
        }

        [Test]
        public static void PropagatePropertyChangeEventTest4()
        {
            var notified = new List<string>();

            var model = new BasicModel();
            model.PropertyChanged += (_, args) => { notified.Add(args.PropertyName!); };

            const int setValue = 80;

            {
                // 【前提条件】ScoreMath, ScoringRateMath, ScoreMax の値
                Assert.AreEqual(0, model.ScoreMath);
                Assert.AreEqual(0, model.ScoringRateMath);
                Assert.AreEqual(100, model.ScoreMax);

                // 違う値をセットする、通知されること
                //      初期値: null
                model.ScoreMath = setValue;
                // 【前提条件】ScoreMath, ScoringRateMath が置き換わっていること
                Assert.AreEqual(setValue, model.ScoreMath);
                Assert.AreEqual((double)setValue / 100, model.ScoringRateMath);
                Assert.AreEqual(100, model.ScoreMax);

                // すべて通知されていること
                Assert.AreEqual(3, notified.Count);
                Assert.AreEqual(nameof(model.ScoreMath), notified[0]);
                Assert.AreEqual(nameof(model.ScoreMax), notified[1]);
                Assert.AreEqual(nameof(model.ScoringRateMath), notified[2]);
            }
        }

        #region TestClass

        /// <summary>
        ///     テスト用モデルクラス
        /// </summary>
        public class BasicModel : ModelBase
        {
            /// <summary>
            ///     視力
            /// </summary>
            /// <remarks>
            ///     NotifyPropertyChanged の動作確認用
            /// </remarks>
            public double Eyesight
            {
                get => eyesight;
                set
                {
                    eyesight = value;
                    NotifyPropertyChanged();
                }
            }

            /// <summary>
            ///     名前
            /// </summary>
            /// <remarks>
            ///     SetField1 の動作確認用
            /// </remarks>
            public string Name
            {
                get => name;
                set => SetField(ref name, value);
            }

            /// <summary>
            ///     座右の銘
            /// </summary>
            /// <remarks>
            ///     SetField2 の動作確認用
            /// </remarks>
            public string Motto
            {
                get => motto.Text;
                set => SetField(value, () => motto.Text, v => motto.Text = v);
            }

            /// <summary>
            ///     年齢
            /// </summary>
            /// <remarks>
            ///     PropertyChangeNotificationHelper.ProcessWithOtherPropertyChangeNotification の動作確認用
            /// </remarks>
            public int Age
            {
                get => age;
                set
                {
                    PropertyChangeNotificationHelper.ProcessWithOtherPropertyChangeNotification(
                        new PropertyChangeNotificationHelper.WatchPropertyInfo[]
                        {
                            (nameof(IsTeenager), () => IsTeenager),
                        },
                        () => SetField(ref age, value),
                        NotifyPropertyChanged
                    );
                }
            }

            /// <summary>
            ///     ティーンエイジャーか
            /// </summary>
            /// <remarks>
            ///     PropertyChangeNotificationHelper.ProcessWithOtherPropertyChangeNotification の動作確認用
            /// </remarks>
            public bool IsTeenager => 13 <= age && age <= 19;

            /// <summary>
            ///     タグ
            /// </summary>
            /// <remarks>
            ///     PropagatePropertyChangeEvent(INotifyPropertyChanged) の動作確認用<br/>
            ///     値変更時は "Tag" ではなく "Text"(TextModelのプロパティ名）であることに注意（意図的にそうしている）
            /// </remarks>
            public string Tag
            {
                get => tag.Text;
                set => tag.Text = value;
            }

            /// <summary>
            ///     国
            /// </summary>
            /// <remarks>
            ///     PropagatePropertyChangeEvent(INotifyPropertyChanged,IEnumerable&lt;string&gt;) の動作確認用
            /// </remarks>
            public string Country
            {
                get => city.Country;
                set
                {
                    city.Country = value;
                    city.City = value;
                }
            }

            /// <summary>
            ///     都市
            /// </summary>
            /// <remarks>
            ///     PropagatePropertyChangeEvent(INotifyPropertyChanged,IEnumerable&lt;string&gt;) の動作確認用
            /// </remarks>
            public string City
            {
                get => city.City;
                set => city.City = value;
            }

            /// <summary>
            ///     右手の持ち物
            /// </summary>
            /// <remarks>
            ///     PropagatePropertyChangeEvent(INotifyPropertyChanged,PropertyChangeNotificationHelper.FilterNotifyPropertyName)
            ///     の作確認用
            /// </remarks>
            public string? LeftHand
            {
                get => belonging.LeftHand;
                set => belonging.LeftHand = value;
            }

            /// <summary>
            ///     左手の持ち物
            /// </summary>
            /// <remarks>
            ///     PropagatePropertyChangeEvent(INotifyPropertyChanged,PropertyChangeNotificationHelper.FilterNotifyPropertyName)
            ///     の作確認用
            /// </remarks>
            public string? RightHand
            {
                get => belonging.RightHand;
                set => belonging.RightHand = value;
            }

            /// <summary>
            ///     手になにか持っているか
            /// </summary>
            /// <remarks>
            ///     PropagatePropertyChangeEvent(INotifyPropertyChanged,PropertyChangeNotificationHelper.FilterNotifyPropertyName)
            ///     の作確認用
            /// </remarks>
            public bool Having => belonging.Having;

            /// <summary>
            ///     数学のテスト点数
            /// </summary>
            /// <remarks>
            ///     PropagatePropertyChangeEvent(INotifyPropertyChanged,PropertyChangeNotificationHelper.MapNotifyPropertyName) の動作確認用
            /// </remarks>
            public int ScoreMath
            {
                get => scoreMath.Numerator;
                set => scoreMath.Numerator = value;
            }

            /// <summary>
            ///     数学の得点率
            /// </summary>
            /// <remarks>
            ///     PropagatePropertyChangeEvent(INotifyPropertyChanged,PropertyChangeNotificationHelper.MapNotifyPropertyName) の作確認用
            /// </remarks>
            public double ScoringRateMath => scoreMath.Decimal;

            /// <summary>
            ///     テスト最大点
            /// </summary>
            /// <remarks>
            ///     PropagatePropertyChangeEvent(INotifyPropertyChanged,PropertyChangeNotificationHelper.MapNotifyPropertyName) の作確認用
            /// </remarks>
            public int ScoreMax
            {
                get => scoreMath.Denominator;
                set => scoreMath.Denominator = value;
            }

            private string name = "";
            private int age = 0;
            private double eyesight = 1.5;
            private readonly TextModel motto = new();
            private readonly TextModel tag = new();
            private readonly CityModel city = new();
            private readonly FractionModel scoreMath = new(100, 0);
            private readonly BelongingsModel belonging = new();

            public BasicModel()
            {
                // TextModel.Text が変更通知されるようにする
                PropagatePropertyChangeEvent(tag);

                // city.Country のみ通知する
                PropagatePropertyChangeEvent(city, new[] { nameof(CityModel.Country) });

                // LeftHand のみ通知し、他は通知しない
                PropagatePropertyChangeEvent(belonging, FilterNotifyPropertyChangeBelonging);

                // Decimal を ScoreMath, ScoreMax, ScoringRateMath に変換し、それ以外は変換しない
                PropagatePropertyChangeEvent(scoreMath, MapNotifyPropertyName);
            }

            private bool FilterNotifyPropertyChangeBelonging(object sender, string propertyName)
            {
                // LeftHand のみ通知し、他は通知しない
                return propertyName == nameof(BelongingsModel.LeftHand);
            }

            private string[]? MapNotifyPropertyName(object sender, string propertyName)
            {
                // Decimal を ScoreMath, ScoreMax, ScoringRateMath に変換し、それ以外は変換しない
                return propertyName == nameof(FractionModel.Decimal)
                    ? new[]
                    {
                        nameof(ScoreMath),
                        nameof(ScoreMax),
                        nameof(ScoringRateMath),
                    }
                    : null;
            }
        }

        /// <summary>
        ///     文字列を保持するModel
        /// </summary>
        /// <remarks>
        ///     モデルクラスのフィールドに別モデルクラスを持つ場合のテストに使用する。
        /// </remarks>
        public class TextModel : ModelBase,
            IEqualityComparable<TextModel>
        {
            public string Text
            {
                get => text;
                set => SetField(ref text, value);
            }

            private string text = "";

            public bool ItemEquals(TextModel? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Text == other.Text;
            }

            public bool ItemEquals(object? other) => ItemEquals(other as TextModel);
        }

        /// <summary>
        ///     都市情報モデル
        /// </summary>
        /// <remarks>
        ///     プロパティ変更通知の一部のみを親モデルに伝播するテストに使用する。
        /// </remarks>
        public class CityModel : ModelBase,
            IEqualityComparable<CityModel>
        {
            /// <summary>
            ///     国
            /// </summary>
            public string Country
            {
                get => country;
                set => SetField(ref country, value);
            }

            /// <summary>
            ///     都市
            /// </summary>
            public string City
            {
                get => city;
                set => SetField(ref city, value);
            }

            private string country = "";
            private string city = "";

            public bool ItemEquals(CityModel? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Country == other.Country
                       && city == other.city;
            }

            public bool ItemEquals(object? other) => ItemEquals(other as CityModel);
        }

        /// <summary>
        ///     持ち物
        /// </summary>
        /// <remarks>
        ///     モデルクラスのフィールドに別モデルクラスを持つ場合のテストに使用する。
        /// </remarks>
        public class BelongingsModel : ModelBase
        {
            /// <summary>
            ///     左手
            /// </summary>
            public string? LeftHand
            {
                get => leftHand;
                set
                {
                    SetField(ref leftHand, value);
                    NotifyPropertyChanged(nameof(Having));
                }
            }

            /// <summary>
            ///     右手
            /// </summary>
            public string? RightHand
            {
                get => rightHand;
                set
                {
                    SetField(ref rightHand, value);
                    NotifyPropertyChanged(nameof(Having));
                }
            }

            /// <summary>
            ///     物を持っているか
            /// </summary>
            /// <remarks>
            ///     このプロパティはあえて変更通知を出さない
            /// </remarks>
            public bool Having => LeftHand is not null || RightHand is not null;

            private string? leftHand = null;
            private string? rightHand = null;
        }

        /// <summary>
        ///     分数
        /// </summary>
        /// <remarks>
        ///     モデルクラスのフィールドに別モデルクラスを持つ場合のテストに使用する。
        /// </remarks>
        public class FractionModel : ModelBase,
            IEqualityComparable<FractionModel>
        {
            /// <summary>
            ///     分母
            /// </summary>
            public int Denominator
            {
                get => denominator;
                set
                {
                    PropertyChangeNotificationHelper.ProcessWithOtherPropertyChangeNotification(
                        new PropertyChangeNotificationHelper.WatchPropertyInfo[]
                        {
                            (nameof(Decimal), () => Decimal),
                        },
                        () => SetField(ref denominator, value),
                        NotifyPropertyChanged
                    );
                }
            }

            /// <summary>
            ///     分子
            /// </summary>
            public int Numerator
            {
                get => numerator;
                set
                {
                    PropertyChangeNotificationHelper.ProcessWithOtherPropertyChangeNotification(
                        new PropertyChangeNotificationHelper.WatchPropertyInfo[]
                        {
                            (nameof(Decimal), () => Decimal),
                        },
                        () => SetField(ref numerator, value),
                        NotifyPropertyChanged
                    );
                }
            }

            /// <summary>
            ///     小数
            /// </summary>
            public double Decimal => (double)Numerator / Denominator;

            private int denominator;
            private int numerator;

            public FractionModel(int denominator, int numerator)
            {
                this.denominator = denominator;
                this.numerator = numerator;
            }


            public bool ItemEquals(FractionModel? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Denominator == other.Denominator
                       && Numerator == other.Numerator;
            }

            public bool ItemEquals(object? other) => ItemEquals(other as FractionModel);
        }

        #endregion
    }
}
