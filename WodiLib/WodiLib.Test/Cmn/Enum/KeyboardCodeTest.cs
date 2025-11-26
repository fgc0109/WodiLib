using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.Enum
{
    [TestFixture]
    public class KeyboardCodeTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constants

        /// <summary>
        ///     Id が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_Id()
        {
            Assert.AreEqual(KeyboardCode.Esc.Id, "101");
            Assert.AreEqual(KeyboardCode.One.Id, "102");
            Assert.AreEqual(KeyboardCode.Two.Id, "103");
            Assert.AreEqual(KeyboardCode.Three.Id, "104");
            Assert.AreEqual(KeyboardCode.Four.Id, "105");
            Assert.AreEqual(KeyboardCode.Five.Id, "106");
            Assert.AreEqual(KeyboardCode.Six.Id, "107");
            Assert.AreEqual(KeyboardCode.Seven.Id, "108");
            Assert.AreEqual(KeyboardCode.Eight.Id, "109");
            Assert.AreEqual(KeyboardCode.Nine.Id, "110");
            Assert.AreEqual(KeyboardCode.Zero.Id, "111");
            Assert.AreEqual(KeyboardCode.Hyphen.Id, "112");
            Assert.AreEqual(KeyboardCode.BackSpace.Id, "114");
            Assert.AreEqual(KeyboardCode.Tab.Id, "115");
            Assert.AreEqual(KeyboardCode.Q.Id, "116");
            Assert.AreEqual(KeyboardCode.W.Id, "117");
            Assert.AreEqual(KeyboardCode.E.Id, "118");
            Assert.AreEqual(KeyboardCode.R.Id, "119");
            Assert.AreEqual(KeyboardCode.T.Id, "120");
            Assert.AreEqual(KeyboardCode.Y.Id, "121");
            Assert.AreEqual(KeyboardCode.U.Id, "122");
            Assert.AreEqual(KeyboardCode.I.Id, "123");
            Assert.AreEqual(KeyboardCode.O.Id, "124");
            Assert.AreEqual(KeyboardCode.P.Id, "125");
            Assert.AreEqual(KeyboardCode.LeftBracket.Id, "126");
            Assert.AreEqual(KeyboardCode.RightBracket.Id, "127");
            Assert.AreEqual(KeyboardCode.Enter.Id, "128");
            Assert.AreEqual(KeyboardCode.LeftCtrl.Id, "129");
            Assert.AreEqual(KeyboardCode.A.Id, "130");
            Assert.AreEqual(KeyboardCode.S.Id, "131");
            Assert.AreEqual(KeyboardCode.D.Id, "132");
            Assert.AreEqual(KeyboardCode.F.Id, "133");
            Assert.AreEqual(KeyboardCode.G.Id, "134");
            Assert.AreEqual(KeyboardCode.H.Id, "135");
            Assert.AreEqual(KeyboardCode.J.Id, "136");
            Assert.AreEqual(KeyboardCode.K.Id, "137");
            Assert.AreEqual(KeyboardCode.L.Id, "138");
            Assert.AreEqual(KeyboardCode.Semicolon.Id, "139");
            Assert.AreEqual(KeyboardCode.LeftShift.Id, "142");
            Assert.AreEqual(KeyboardCode.BackSlash.Id, "143");
            Assert.AreEqual(KeyboardCode.Z.Id, "144");
            Assert.AreEqual(KeyboardCode.X.Id, "145");
            Assert.AreEqual(KeyboardCode.C.Id, "146");
            Assert.AreEqual(KeyboardCode.V.Id, "147");
            Assert.AreEqual(KeyboardCode.B.Id, "148");
            Assert.AreEqual(KeyboardCode.N.Id, "149");
            Assert.AreEqual(KeyboardCode.M.Id, "150");
            Assert.AreEqual(KeyboardCode.Comma.Id, "151");
            Assert.AreEqual(KeyboardCode.Period.Id, "152");
            Assert.AreEqual(KeyboardCode.Slash.Id, "153");
            Assert.AreEqual(KeyboardCode.RightShift.Id, "154");
            Assert.AreEqual(KeyboardCode.NumericKeypadAsterisk.Id, "155");
            Assert.AreEqual(KeyboardCode.LeftAlt.Id, "156");
            Assert.AreEqual(KeyboardCode.Space.Id, "157");
            Assert.AreEqual(KeyboardCode.Caps.Id, "158");
            Assert.AreEqual(KeyboardCode.F1.Id, "159");
            Assert.AreEqual(KeyboardCode.F2.Id, "160");
            Assert.AreEqual(KeyboardCode.F3.Id, "161");
            Assert.AreEqual(KeyboardCode.F4.Id, "162");
            Assert.AreEqual(KeyboardCode.F5.Id, "163");
            Assert.AreEqual(KeyboardCode.F6.Id, "164");
            Assert.AreEqual(KeyboardCode.F7.Id, "165");
            Assert.AreEqual(KeyboardCode.F8.Id, "166");
            Assert.AreEqual(KeyboardCode.F9.Id, "167");
            Assert.AreEqual(KeyboardCode.F10.Id, "168");
            Assert.AreEqual(KeyboardCode.Scroll.Id, "170");
            Assert.AreEqual(KeyboardCode.NumericKeypadSeven.Id, "171");
            Assert.AreEqual(KeyboardCode.NumericKeypadEight.Id, "172");
            Assert.AreEqual(KeyboardCode.NumericKeypadNine.Id, "173");
            Assert.AreEqual(KeyboardCode.NumericKeypadHyphen.Id, "174");
            Assert.AreEqual(KeyboardCode.NumericKeypadFour.Id, "175");
            Assert.AreEqual(KeyboardCode.NumericKeypadFive.Id, "176");
            Assert.AreEqual(KeyboardCode.NumericKeypadSix.Id, "177");
            Assert.AreEqual(KeyboardCode.NumericKeypadPlus.Id, "178");
            Assert.AreEqual(KeyboardCode.NumericKeypadOne.Id, "179");
            Assert.AreEqual(KeyboardCode.NumericKeypadTwo.Id, "180");
            Assert.AreEqual(KeyboardCode.NumericKeypadThree.Id, "181");
            Assert.AreEqual(KeyboardCode.NumericKeypadZero.Id, "182");
            Assert.AreEqual(KeyboardCode.NumericKeypadPeriod.Id, "183");
            Assert.AreEqual(KeyboardCode.F11.Id, "187");
            Assert.AreEqual(KeyboardCode.F12.Id, "188");
            Assert.AreEqual(KeyboardCode.KanaToggle.Id, "212");
            Assert.AreEqual(KeyboardCode.Henkan.Id, "221");
            Assert.AreEqual(KeyboardCode.Muhenkan.Id, "223");
            Assert.AreEqual(KeyboardCode.Yen.Id, "225");
            Assert.AreEqual(KeyboardCode.Caret.Id, "244");
            Assert.AreEqual(KeyboardCode.AtSign.Id, "245");
            Assert.AreEqual(KeyboardCode.Colon.Id, "246");
            Assert.AreEqual(KeyboardCode.Kanji.Id, "248");
            Assert.AreEqual(KeyboardCode.RightCtrl.Id, "257");
            Assert.AreEqual(KeyboardCode.NumericKeypadSlash.Id, "281");
            Assert.AreEqual(KeyboardCode.PrintScreen.Id, "283");
            Assert.AreEqual(KeyboardCode.RightAlt.Id, "284");
            Assert.AreEqual(KeyboardCode.Pause.Id, "297");
            Assert.AreEqual(KeyboardCode.Home.Id, "299");
            Assert.AreEqual(KeyboardCode.Up.Id, "300");
            Assert.AreEqual(KeyboardCode.PageUp.Id, "301");
            Assert.AreEqual(KeyboardCode.Left.Id, "303");
            Assert.AreEqual(KeyboardCode.Right.Id, "305");
            Assert.AreEqual(KeyboardCode.End.Id, "307");
            Assert.AreEqual(KeyboardCode.Down.Id, "308");
            Assert.AreEqual(KeyboardCode.PageDown.Id, "309");
            Assert.AreEqual(KeyboardCode.Del.Id, "311");
        }

        /// <summary>
        ///     KeyCode が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_KeyCode()
        {
            Assert.AreEqual(KeyboardCode.Esc.KeyCode, 101);
            Assert.AreEqual(KeyboardCode.One.KeyCode, 102);
            Assert.AreEqual(KeyboardCode.Two.KeyCode, 103);
            Assert.AreEqual(KeyboardCode.Three.KeyCode, 104);
            Assert.AreEqual(KeyboardCode.Four.KeyCode, 105);
            Assert.AreEqual(KeyboardCode.Five.KeyCode, 106);
            Assert.AreEqual(KeyboardCode.Six.KeyCode, 107);
            Assert.AreEqual(KeyboardCode.Seven.KeyCode, 108);
            Assert.AreEqual(KeyboardCode.Eight.KeyCode, 109);
            Assert.AreEqual(KeyboardCode.Nine.KeyCode, 110);
            Assert.AreEqual(KeyboardCode.Zero.KeyCode, 111);
            Assert.AreEqual(KeyboardCode.Hyphen.KeyCode, 112);
            Assert.AreEqual(KeyboardCode.BackSpace.KeyCode, 114);
            Assert.AreEqual(KeyboardCode.Tab.KeyCode, 115);
            Assert.AreEqual(KeyboardCode.Q.KeyCode, 116);
            Assert.AreEqual(KeyboardCode.W.KeyCode, 117);
            Assert.AreEqual(KeyboardCode.E.KeyCode, 118);
            Assert.AreEqual(KeyboardCode.R.KeyCode, 119);
            Assert.AreEqual(KeyboardCode.T.KeyCode, 120);
            Assert.AreEqual(KeyboardCode.Y.KeyCode, 121);
            Assert.AreEqual(KeyboardCode.U.KeyCode, 122);
            Assert.AreEqual(KeyboardCode.I.KeyCode, 123);
            Assert.AreEqual(KeyboardCode.O.KeyCode, 124);
            Assert.AreEqual(KeyboardCode.P.KeyCode, 125);
            Assert.AreEqual(KeyboardCode.LeftBracket.KeyCode, 126);
            Assert.AreEqual(KeyboardCode.RightBracket.KeyCode, 127);
            Assert.AreEqual(KeyboardCode.Enter.KeyCode, 128);
            Assert.AreEqual(KeyboardCode.LeftCtrl.KeyCode, 129);
            Assert.AreEqual(KeyboardCode.A.KeyCode, 130);
            Assert.AreEqual(KeyboardCode.S.KeyCode, 131);
            Assert.AreEqual(KeyboardCode.D.KeyCode, 132);
            Assert.AreEqual(KeyboardCode.F.KeyCode, 133);
            Assert.AreEqual(KeyboardCode.G.KeyCode, 134);
            Assert.AreEqual(KeyboardCode.H.KeyCode, 135);
            Assert.AreEqual(KeyboardCode.J.KeyCode, 136);
            Assert.AreEqual(KeyboardCode.K.KeyCode, 137);
            Assert.AreEqual(KeyboardCode.L.KeyCode, 138);
            Assert.AreEqual(KeyboardCode.Semicolon.KeyCode, 139);
            Assert.AreEqual(KeyboardCode.LeftShift.KeyCode, 142);
            Assert.AreEqual(KeyboardCode.BackSlash.KeyCode, 143);
            Assert.AreEqual(KeyboardCode.Z.KeyCode, 144);
            Assert.AreEqual(KeyboardCode.X.KeyCode, 145);
            Assert.AreEqual(KeyboardCode.C.KeyCode, 146);
            Assert.AreEqual(KeyboardCode.V.KeyCode, 147);
            Assert.AreEqual(KeyboardCode.B.KeyCode, 148);
            Assert.AreEqual(KeyboardCode.N.KeyCode, 149);
            Assert.AreEqual(KeyboardCode.M.KeyCode, 150);
            Assert.AreEqual(KeyboardCode.Comma.KeyCode, 151);
            Assert.AreEqual(KeyboardCode.Period.KeyCode, 152);
            Assert.AreEqual(KeyboardCode.Slash.KeyCode, 153);
            Assert.AreEqual(KeyboardCode.RightShift.KeyCode, 154);
            Assert.AreEqual(KeyboardCode.NumericKeypadAsterisk.KeyCode, 155);
            Assert.AreEqual(KeyboardCode.LeftAlt.KeyCode, 156);
            Assert.AreEqual(KeyboardCode.Space.KeyCode, 157);
            Assert.AreEqual(KeyboardCode.Caps.KeyCode, 158);
            Assert.AreEqual(KeyboardCode.F1.KeyCode, 159);
            Assert.AreEqual(KeyboardCode.F2.KeyCode, 160);
            Assert.AreEqual(KeyboardCode.F3.KeyCode, 161);
            Assert.AreEqual(KeyboardCode.F4.KeyCode, 162);
            Assert.AreEqual(KeyboardCode.F5.KeyCode, 163);
            Assert.AreEqual(KeyboardCode.F6.KeyCode, 164);
            Assert.AreEqual(KeyboardCode.F7.KeyCode, 165);
            Assert.AreEqual(KeyboardCode.F8.KeyCode, 166);
            Assert.AreEqual(KeyboardCode.F9.KeyCode, 167);
            Assert.AreEqual(KeyboardCode.F10.KeyCode, 168);
            Assert.AreEqual(KeyboardCode.Scroll.KeyCode, 170);
            Assert.AreEqual(KeyboardCode.NumericKeypadSeven.KeyCode, 171);
            Assert.AreEqual(KeyboardCode.NumericKeypadEight.KeyCode, 172);
            Assert.AreEqual(KeyboardCode.NumericKeypadNine.KeyCode, 173);
            Assert.AreEqual(KeyboardCode.NumericKeypadHyphen.KeyCode, 174);
            Assert.AreEqual(KeyboardCode.NumericKeypadFour.KeyCode, 175);
            Assert.AreEqual(KeyboardCode.NumericKeypadFive.KeyCode, 176);
            Assert.AreEqual(KeyboardCode.NumericKeypadSix.KeyCode, 177);
            Assert.AreEqual(KeyboardCode.NumericKeypadPlus.KeyCode, 178);
            Assert.AreEqual(KeyboardCode.NumericKeypadOne.KeyCode, 179);
            Assert.AreEqual(KeyboardCode.NumericKeypadTwo.KeyCode, 180);
            Assert.AreEqual(KeyboardCode.NumericKeypadThree.KeyCode, 181);
            Assert.AreEqual(KeyboardCode.NumericKeypadZero.KeyCode, 182);
            Assert.AreEqual(KeyboardCode.NumericKeypadPeriod.KeyCode, 183);
            Assert.AreEqual(KeyboardCode.F11.KeyCode, 187);
            Assert.AreEqual(KeyboardCode.F12.KeyCode, 188);
            Assert.AreEqual(KeyboardCode.KanaToggle.KeyCode, 212);
            Assert.AreEqual(KeyboardCode.Henkan.KeyCode, 221);
            Assert.AreEqual(KeyboardCode.Muhenkan.KeyCode, 223);
            Assert.AreEqual(KeyboardCode.Yen.KeyCode, 225);
            Assert.AreEqual(KeyboardCode.Caret.KeyCode, 244);
            Assert.AreEqual(KeyboardCode.AtSign.KeyCode, 245);
            Assert.AreEqual(KeyboardCode.Colon.KeyCode, 246);
            Assert.AreEqual(KeyboardCode.Kanji.KeyCode, 248);
            Assert.AreEqual(KeyboardCode.RightCtrl.KeyCode, 257);
            Assert.AreEqual(KeyboardCode.NumericKeypadSlash.KeyCode, 281);
            Assert.AreEqual(KeyboardCode.PrintScreen.KeyCode, 283);
            Assert.AreEqual(KeyboardCode.RightAlt.KeyCode, 284);
            Assert.AreEqual(KeyboardCode.Pause.KeyCode, 297);
            Assert.AreEqual(KeyboardCode.Home.KeyCode, 299);
            Assert.AreEqual(KeyboardCode.Up.KeyCode, 300);
            Assert.AreEqual(KeyboardCode.PageUp.KeyCode, 301);
            Assert.AreEqual(KeyboardCode.Left.KeyCode, 303);
            Assert.AreEqual(KeyboardCode.Right.KeyCode, 305);
            Assert.AreEqual(KeyboardCode.End.KeyCode, 307);
            Assert.AreEqual(KeyboardCode.Down.KeyCode, 308);
            Assert.AreEqual(KeyboardCode.PageDown.KeyCode, 309);
            Assert.AreEqual(KeyboardCode.Del.KeyCode, 311);
        }

        /// <summary>
        ///     ID が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_KeyName()
        {
            Assert.AreEqual(KeyboardCode.Esc.KeyName, "ESC");
            Assert.AreEqual(KeyboardCode.One.KeyName, "1");
            Assert.AreEqual(KeyboardCode.Two.KeyName, "2");
            Assert.AreEqual(KeyboardCode.Three.KeyName, "3");
            Assert.AreEqual(KeyboardCode.Four.KeyName, "4");
            Assert.AreEqual(KeyboardCode.Five.KeyName, "5");
            Assert.AreEqual(KeyboardCode.Six.KeyName, "6");
            Assert.AreEqual(KeyboardCode.Seven.KeyName, "7");
            Assert.AreEqual(KeyboardCode.Eight.KeyName, "8");
            Assert.AreEqual(KeyboardCode.Nine.KeyName, "9");
            Assert.AreEqual(KeyboardCode.Zero.KeyName, "0");
            Assert.AreEqual(KeyboardCode.Hyphen.KeyName, "-");
            Assert.AreEqual(KeyboardCode.BackSpace.KeyName, "BS");
            Assert.AreEqual(KeyboardCode.Tab.KeyName, "TAB");
            Assert.AreEqual(KeyboardCode.Q.KeyName, "Q");
            Assert.AreEqual(KeyboardCode.W.KeyName, "W");
            Assert.AreEqual(KeyboardCode.E.KeyName, "E");
            Assert.AreEqual(KeyboardCode.R.KeyName, "R");
            Assert.AreEqual(KeyboardCode.T.KeyName, "T");
            Assert.AreEqual(KeyboardCode.Y.KeyName, "Y");
            Assert.AreEqual(KeyboardCode.U.KeyName, "U");
            Assert.AreEqual(KeyboardCode.I.KeyName, "I");
            Assert.AreEqual(KeyboardCode.O.KeyName, "O");
            Assert.AreEqual(KeyboardCode.P.KeyName, "P");
            Assert.AreEqual(KeyboardCode.LeftBracket.KeyName, "[");
            Assert.AreEqual(KeyboardCode.RightBracket.KeyName, "]");
            Assert.AreEqual(KeyboardCode.Enter.KeyName, "Enter");
            Assert.AreEqual(KeyboardCode.LeftCtrl.KeyName, "左Ctrl");
            Assert.AreEqual(KeyboardCode.A.KeyName, "A");
            Assert.AreEqual(KeyboardCode.S.KeyName, "S");
            Assert.AreEqual(KeyboardCode.D.KeyName, "D");
            Assert.AreEqual(KeyboardCode.F.KeyName, "F");
            Assert.AreEqual(KeyboardCode.G.KeyName, "G");
            Assert.AreEqual(KeyboardCode.H.KeyName, "H");
            Assert.AreEqual(KeyboardCode.J.KeyName, "J");
            Assert.AreEqual(KeyboardCode.K.KeyName, "K");
            Assert.AreEqual(KeyboardCode.L.KeyName, "L");
            Assert.AreEqual(KeyboardCode.Semicolon.KeyName, ";");
            Assert.AreEqual(KeyboardCode.LeftShift.KeyName, "左Shift");
            Assert.AreEqual(KeyboardCode.BackSlash.KeyName, "＼");
            Assert.AreEqual(KeyboardCode.Z.KeyName, "Z");
            Assert.AreEqual(KeyboardCode.X.KeyName, "X");
            Assert.AreEqual(KeyboardCode.C.KeyName, "C");
            Assert.AreEqual(KeyboardCode.V.KeyName, "V");
            Assert.AreEqual(KeyboardCode.B.KeyName, "B");
            Assert.AreEqual(KeyboardCode.N.KeyName, "N");
            Assert.AreEqual(KeyboardCode.M.KeyName, "M");
            Assert.AreEqual(KeyboardCode.Comma.KeyName, ",");
            Assert.AreEqual(KeyboardCode.Period.KeyName, ".");
            Assert.AreEqual(KeyboardCode.Slash.KeyName, "/");
            Assert.AreEqual(KeyboardCode.RightShift.KeyName, "右Shift");
            Assert.AreEqual(KeyboardCode.NumericKeypadAsterisk.KeyName, "テンキー *");
            Assert.AreEqual(KeyboardCode.LeftAlt.KeyName, "左Alt");
            Assert.AreEqual(KeyboardCode.Space.KeyName, "Space");
            Assert.AreEqual(KeyboardCode.Caps.KeyName, "Caps");
            Assert.AreEqual(KeyboardCode.F1.KeyName, "F1");
            Assert.AreEqual(KeyboardCode.F2.KeyName, "F2");
            Assert.AreEqual(KeyboardCode.F3.KeyName, "F3");
            Assert.AreEqual(KeyboardCode.F4.KeyName, "F4");
            Assert.AreEqual(KeyboardCode.F5.KeyName, "F5");
            Assert.AreEqual(KeyboardCode.F6.KeyName, "F6");
            Assert.AreEqual(KeyboardCode.F7.KeyName, "F7");
            Assert.AreEqual(KeyboardCode.F8.KeyName, "F8");
            Assert.AreEqual(KeyboardCode.F9.KeyName, "F9");
            Assert.AreEqual(KeyboardCode.F10.KeyName, "F10");
            Assert.AreEqual(KeyboardCode.Scroll.KeyName, "Scroll");
            Assert.AreEqual(KeyboardCode.NumericKeypadSeven.KeyName, "テンキー7");
            Assert.AreEqual(KeyboardCode.NumericKeypadEight.KeyName, "テンキー8");
            Assert.AreEqual(KeyboardCode.NumericKeypadNine.KeyName, "テンキー9");
            Assert.AreEqual(KeyboardCode.NumericKeypadHyphen.KeyName, "テンキー -");
            Assert.AreEqual(KeyboardCode.NumericKeypadFour.KeyName, "テンキー4");
            Assert.AreEqual(KeyboardCode.NumericKeypadFive.KeyName, "テンキー5");
            Assert.AreEqual(KeyboardCode.NumericKeypadSix.KeyName, "テンキー6");
            Assert.AreEqual(KeyboardCode.NumericKeypadPlus.KeyName, "テンキー +");
            Assert.AreEqual(KeyboardCode.NumericKeypadOne.KeyName, "テンキー1");
            Assert.AreEqual(KeyboardCode.NumericKeypadTwo.KeyName, "テンキー2");
            Assert.AreEqual(KeyboardCode.NumericKeypadThree.KeyName, "テンキー3");
            Assert.AreEqual(KeyboardCode.NumericKeypadZero.KeyName, "テンキー0");
            Assert.AreEqual(KeyboardCode.NumericKeypadPeriod.KeyName, "テンキー .");
            Assert.AreEqual(KeyboardCode.F11.KeyName, "F11");
            Assert.AreEqual(KeyboardCode.F12.KeyName, "F12");
            Assert.AreEqual(KeyboardCode.KanaToggle.KeyName, "ｶﾀｶﾀ・ひらがな");
            Assert.AreEqual(KeyboardCode.Henkan.KeyName, "変換[切替]");
            Assert.AreEqual(KeyboardCode.Muhenkan.KeyName, "無変換[切替]");
            Assert.AreEqual(KeyboardCode.Yen.KeyName, "\\");
            Assert.AreEqual(KeyboardCode.Caret.KeyName, "^");
            Assert.AreEqual(KeyboardCode.AtSign.KeyName, "@");
            Assert.AreEqual(KeyboardCode.Colon.KeyName, ":");
            Assert.AreEqual(KeyboardCode.Kanji.KeyName, "半角/全角[切替]");
            Assert.AreEqual(KeyboardCode.RightCtrl.KeyName, "右Ctrl");
            Assert.AreEqual(KeyboardCode.NumericKeypadSlash.KeyName, "テンキー /");
            Assert.AreEqual(KeyboardCode.PrintScreen.KeyName, "PrintScreen");
            Assert.AreEqual(KeyboardCode.RightAlt.KeyName, "右Alt");
            Assert.AreEqual(KeyboardCode.Pause.KeyName, "Pause");
            Assert.AreEqual(KeyboardCode.Home.KeyName, "HOME");
            Assert.AreEqual(KeyboardCode.Up.KeyName, "↑");
            Assert.AreEqual(KeyboardCode.PageUp.KeyName, "PageUp");
            Assert.AreEqual(KeyboardCode.Left.KeyName, "←");
            Assert.AreEqual(KeyboardCode.Right.KeyName, "→");
            Assert.AreEqual(KeyboardCode.End.KeyName, "END");
            Assert.AreEqual(KeyboardCode.Down.KeyName, "↓");
            Assert.AreEqual(KeyboardCode.PageDown.KeyName, "PageDown");
            Assert.AreEqual(KeyboardCode.Del.KeyName, "DEL");
        }

        #endregion

        #region Methods

        [TestCase(99, false)]
        [TestCase(100, true)]
        [TestCase(355, true)]
        [TestCase(356, false)]
        public static void IsKeyCodeTest(int value, bool expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => KeyboardCode.IsKeyCode(value),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #region GetKeyName

        private static readonly object[][] FromCodeTest_Success_TestCaseSource = new[]
        {
            // [code, expected]
            new object[] { KeyboardCode.Esc.KeyCode, KeyboardCode.Esc.KeyName },
            new object[] { KeyboardCode.One.KeyCode, KeyboardCode.One.KeyName },
            new object[] { KeyboardCode.Two.KeyCode, KeyboardCode.Two.KeyName },
            new object[] { KeyboardCode.Three.KeyCode, KeyboardCode.Three.KeyName },
            new object[] { KeyboardCode.Four.KeyCode, KeyboardCode.Four.KeyName },
            new object[] { KeyboardCode.Five.KeyCode, KeyboardCode.Five.KeyName },
            new object[] { KeyboardCode.Six.KeyCode, KeyboardCode.Six.KeyName },
            new object[] { KeyboardCode.Seven.KeyCode, KeyboardCode.Seven.KeyName },
            new object[] { KeyboardCode.Eight.KeyCode, KeyboardCode.Eight.KeyName },
            new object[] { KeyboardCode.Nine.KeyCode, KeyboardCode.Nine.KeyName },
            new object[] { KeyboardCode.Zero.KeyCode, KeyboardCode.Zero.KeyName },
            new object[] { KeyboardCode.Hyphen.KeyCode, KeyboardCode.Hyphen.KeyName },
            new object[] { KeyboardCode.BackSpace.KeyCode, KeyboardCode.BackSpace.KeyName },
            new object[] { KeyboardCode.Tab.KeyCode, KeyboardCode.Tab.KeyName },
            new object[] { KeyboardCode.Q.KeyCode, KeyboardCode.Q.KeyName },
            new object[] { KeyboardCode.W.KeyCode, KeyboardCode.W.KeyName },
            new object[] { KeyboardCode.E.KeyCode, KeyboardCode.E.KeyName },
            new object[] { KeyboardCode.R.KeyCode, KeyboardCode.R.KeyName },
            new object[] { KeyboardCode.T.KeyCode, KeyboardCode.T.KeyName },
            new object[] { KeyboardCode.Y.KeyCode, KeyboardCode.Y.KeyName },
            new object[] { KeyboardCode.U.KeyCode, KeyboardCode.U.KeyName },
            new object[] { KeyboardCode.I.KeyCode, KeyboardCode.I.KeyName },
            new object[] { KeyboardCode.O.KeyCode, KeyboardCode.O.KeyName },
            new object[] { KeyboardCode.P.KeyCode, KeyboardCode.P.KeyName },
            new object[] { KeyboardCode.LeftBracket.KeyCode, KeyboardCode.LeftBracket.KeyName },
            new object[] { KeyboardCode.RightBracket.KeyCode, KeyboardCode.RightBracket.KeyName },
            new object[] { KeyboardCode.Enter.KeyCode, KeyboardCode.Enter.KeyName },
            new object[] { KeyboardCode.LeftCtrl.KeyCode, KeyboardCode.LeftCtrl.KeyName },
            new object[] { KeyboardCode.A.KeyCode, KeyboardCode.A.KeyName },
            new object[] { KeyboardCode.S.KeyCode, KeyboardCode.S.KeyName },
            new object[] { KeyboardCode.D.KeyCode, KeyboardCode.D.KeyName },
            new object[] { KeyboardCode.F.KeyCode, KeyboardCode.F.KeyName },
            new object[] { KeyboardCode.G.KeyCode, KeyboardCode.G.KeyName },
            new object[] { KeyboardCode.H.KeyCode, KeyboardCode.H.KeyName },
            new object[] { KeyboardCode.J.KeyCode, KeyboardCode.J.KeyName },
            new object[] { KeyboardCode.K.KeyCode, KeyboardCode.K.KeyName },
            new object[] { KeyboardCode.L.KeyCode, KeyboardCode.L.KeyName },
            new object[] { KeyboardCode.Semicolon.KeyCode, KeyboardCode.Semicolon.KeyName },
            new object[] { KeyboardCode.LeftShift.KeyCode, KeyboardCode.LeftShift.KeyName },
            new object[] { KeyboardCode.BackSlash.KeyCode, KeyboardCode.BackSlash.KeyName },
            new object[] { KeyboardCode.Z.KeyCode, KeyboardCode.Z.KeyName },
            new object[] { KeyboardCode.X.KeyCode, KeyboardCode.X.KeyName },
            new object[] { KeyboardCode.C.KeyCode, KeyboardCode.C.KeyName },
            new object[] { KeyboardCode.V.KeyCode, KeyboardCode.V.KeyName },
            new object[] { KeyboardCode.B.KeyCode, KeyboardCode.B.KeyName },
            new object[] { KeyboardCode.N.KeyCode, KeyboardCode.N.KeyName },
            new object[] { KeyboardCode.M.KeyCode, KeyboardCode.M.KeyName },
            new object[] { KeyboardCode.Comma.KeyCode, KeyboardCode.Comma.KeyName },
            new object[] { KeyboardCode.Period.KeyCode, KeyboardCode.Period.KeyName },
            new object[] { KeyboardCode.Slash.KeyCode, KeyboardCode.Slash.KeyName },
            new object[] { KeyboardCode.RightShift.KeyCode, KeyboardCode.RightShift.KeyName },
            new object[] { KeyboardCode.NumericKeypadAsterisk.KeyCode, KeyboardCode.NumericKeypadAsterisk.KeyName },
            new object[] { KeyboardCode.LeftAlt.KeyCode, KeyboardCode.LeftAlt.KeyName },
            new object[] { KeyboardCode.Space.KeyCode, KeyboardCode.Space.KeyName },
            new object[] { KeyboardCode.Caps.KeyCode, KeyboardCode.Caps.KeyName },
            new object[] { KeyboardCode.F1.KeyCode, KeyboardCode.F1.KeyName },
            new object[] { KeyboardCode.F2.KeyCode, KeyboardCode.F2.KeyName },
            new object[] { KeyboardCode.F3.KeyCode, KeyboardCode.F3.KeyName },
            new object[] { KeyboardCode.F4.KeyCode, KeyboardCode.F4.KeyName },
            new object[] { KeyboardCode.F5.KeyCode, KeyboardCode.F5.KeyName },
            new object[] { KeyboardCode.F6.KeyCode, KeyboardCode.F6.KeyName },
            new object[] { KeyboardCode.F7.KeyCode, KeyboardCode.F7.KeyName },
            new object[] { KeyboardCode.F8.KeyCode, KeyboardCode.F8.KeyName },
            new object[] { KeyboardCode.F9.KeyCode, KeyboardCode.F9.KeyName },
            new object[] { KeyboardCode.F10.KeyCode, KeyboardCode.F10.KeyName },
            new object[] { KeyboardCode.Scroll.KeyCode, KeyboardCode.Scroll.KeyName },
            new object[] { KeyboardCode.NumericKeypadSeven.KeyCode, KeyboardCode.NumericKeypadSeven.KeyName },
            new object[] { KeyboardCode.NumericKeypadEight.KeyCode, KeyboardCode.NumericKeypadEight.KeyName },
            new object[] { KeyboardCode.NumericKeypadNine.KeyCode, KeyboardCode.NumericKeypadNine.KeyName },
            new object[] { KeyboardCode.NumericKeypadHyphen.KeyCode, KeyboardCode.NumericKeypadHyphen.KeyName },
            new object[] { KeyboardCode.NumericKeypadFour.KeyCode, KeyboardCode.NumericKeypadFour.KeyName },
            new object[] { KeyboardCode.NumericKeypadFive.KeyCode, KeyboardCode.NumericKeypadFive.KeyName },
            new object[] { KeyboardCode.NumericKeypadSix.KeyCode, KeyboardCode.NumericKeypadSix.KeyName },
            new object[] { KeyboardCode.NumericKeypadPlus.KeyCode, KeyboardCode.NumericKeypadPlus.KeyName },
            new object[] { KeyboardCode.NumericKeypadOne.KeyCode, KeyboardCode.NumericKeypadOne.KeyName },
            new object[] { KeyboardCode.NumericKeypadTwo.KeyCode, KeyboardCode.NumericKeypadTwo.KeyName },
            new object[] { KeyboardCode.NumericKeypadThree.KeyCode, KeyboardCode.NumericKeypadThree.KeyName },
            new object[] { KeyboardCode.NumericKeypadZero.KeyCode, KeyboardCode.NumericKeypadZero.KeyName },
            new object[] { KeyboardCode.NumericKeypadPeriod.KeyCode, KeyboardCode.NumericKeypadPeriod.KeyName },
            new object[] { KeyboardCode.F11.KeyCode, KeyboardCode.F11.KeyName },
            new object[] { KeyboardCode.F12.KeyCode, KeyboardCode.F12.KeyName },
            new object[] { KeyboardCode.KanaToggle.KeyCode, KeyboardCode.KanaToggle.KeyName },
            new object[] { KeyboardCode.Henkan.KeyCode, KeyboardCode.Henkan.KeyName },
            new object[] { KeyboardCode.Muhenkan.KeyCode, KeyboardCode.Muhenkan.KeyName },
            new object[] { KeyboardCode.Yen.KeyCode, KeyboardCode.Yen.KeyName },
            new object[] { KeyboardCode.Caret.KeyCode, KeyboardCode.Caret.KeyName },
            new object[] { KeyboardCode.AtSign.KeyCode, KeyboardCode.AtSign.KeyName },
            new object[] { KeyboardCode.Colon.KeyCode, KeyboardCode.Colon.KeyName },
            new object[] { KeyboardCode.Kanji.KeyCode, KeyboardCode.Kanji.KeyName },
            new object[] { KeyboardCode.RightCtrl.KeyCode, KeyboardCode.RightCtrl.KeyName },
            new object[] { KeyboardCode.NumericKeypadSlash.KeyCode, KeyboardCode.NumericKeypadSlash.KeyName },
            new object[] { KeyboardCode.PrintScreen.KeyCode, KeyboardCode.PrintScreen.KeyName },
            new object[] { KeyboardCode.RightAlt.KeyCode, KeyboardCode.RightAlt.KeyName },
            new object[] { KeyboardCode.Pause.KeyCode, KeyboardCode.Pause.KeyName },
            new object[] { KeyboardCode.Home.KeyCode, KeyboardCode.Home.KeyName },
            new object[] { KeyboardCode.Up.KeyCode, KeyboardCode.Up.KeyName },
            new object[] { KeyboardCode.PageUp.KeyCode, KeyboardCode.PageUp.KeyName },
            new object[] { KeyboardCode.Left.KeyCode, KeyboardCode.Left.KeyName },
            new object[] { KeyboardCode.Right.KeyCode, KeyboardCode.Right.KeyName },
            new object[] { KeyboardCode.End.KeyCode, KeyboardCode.End.KeyName },
            new object[] { KeyboardCode.Down.KeyCode, KeyboardCode.Down.KeyName },
            new object[] { KeyboardCode.PageDown.KeyCode, KeyboardCode.PageDown.KeyName },
            new object[] { KeyboardCode.Del.KeyCode, KeyboardCode.Del.KeyName },
        };

        /// <summary>
        ///     KeyCode から KeyName が正しく取得できること。
        /// </summary>
        [TestCaseSource(nameof(FromCodeTest_Success_TestCaseSource))]
        public static void GetKeyNameTest_Success_DefinedKeyCode(int keyCode, string expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => KeyboardCode.GetKeyName(keyCode),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     keyCode が定義されていない値の場合、
        ///     "非対応" が返却されること。
        /// </summary>
        [Test]
        public static void GetKeyNameTest_Success_UndefinedKeyCode()
        {
            const int keyCode = 200;
            const string expected = "非対応";
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => KeyboardCode.GetKeyName(keyCode),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion
    }
}
