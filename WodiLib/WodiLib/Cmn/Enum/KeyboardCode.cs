// ========================================
// Project Name : WodiLib
// File Name    : KeyboardCode.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     キーボードコード
    /// </summary>
    public class KeyboardCode : TypeSafeEnum<KeyboardCode>
    {
        /// <summary>ESC</summary>
        public static readonly KeyboardCode Esc;

        /// <summary>1</summary>
        public static readonly KeyboardCode One;

        /// <summary>2</summary>
        public static readonly KeyboardCode Two;

        /// <summary>3</summary>
        public static readonly KeyboardCode Three;

        /// <summary>4</summary>
        public static readonly KeyboardCode Four;

        /// <summary>5</summary>
        public static readonly KeyboardCode Five;

        /// <summary>6</summary>
        public static readonly KeyboardCode Six;

        /// <summary>7</summary>
        public static readonly KeyboardCode Seven;

        /// <summary>8</summary>
        public static readonly KeyboardCode Eight;

        /// <summary>9</summary>
        public static readonly KeyboardCode Nine;

        /// <summary>0</summary>
        public static readonly KeyboardCode Zero;

        /// <summary>-</summary>
        public static readonly KeyboardCode Hyphen;

        /// <summary>BS</summary>
        public static readonly KeyboardCode BackSpace;

        /// <summary>TAB</summary>
        public static readonly KeyboardCode Tab;

        /// <summary>Q</summary>
        public static readonly KeyboardCode Q;

        /// <summary>W</summary>
        public static readonly KeyboardCode W;

        /// <summary>E</summary>
        public static readonly KeyboardCode E;

        /// <summary>R</summary>
        public static readonly KeyboardCode R;

        /// <summary>T</summary>
        public static readonly KeyboardCode T;

        /// <summary>Y</summary>
        public static readonly KeyboardCode Y;

        /// <summary>U</summary>
        public static readonly KeyboardCode U;

        /// <summary>I</summary>
        public static readonly KeyboardCode I;

        /// <summary>O</summary>
        public static readonly KeyboardCode O;

        /// <summary>P</summary>
        public static readonly KeyboardCode P;

        /// <summary>[</summary>
        public static readonly KeyboardCode LeftBracket;

        /// <summary>]</summary>
        public static readonly KeyboardCode RightBracket;

        /// <summary>Enter</summary>
        public static readonly KeyboardCode Enter;

        /// <summary>左Ctrl</summary>
        public static readonly KeyboardCode LeftCtrl;

        /// <summary>A</summary>
        public static readonly KeyboardCode A;

        /// <summary>S</summary>
        public static readonly KeyboardCode S;

        /// <summary>D</summary>
        public static readonly KeyboardCode D;

        /// <summary>F</summary>
        public static readonly KeyboardCode F;

        /// <summary>G</summary>
        public static readonly KeyboardCode G;

        /// <summary>H</summary>
        public static readonly KeyboardCode H;

        /// <summary>J</summary>
        public static readonly KeyboardCode J;

        /// <summary>K</summary>
        public static readonly KeyboardCode K;

        /// <summary>L</summary>
        public static readonly KeyboardCode L;

        /// <summary>;</summary>
        public static readonly KeyboardCode Semicolon;

        /// <summary>左Shift</summary>
        public static readonly KeyboardCode LeftShift;

        /// <summary>＼</summary>
        public static readonly KeyboardCode BackSlash;

        /// <summary>Z</summary>
        public static readonly KeyboardCode Z;

        /// <summary>X</summary>
        public static readonly KeyboardCode X;

        /// <summary>C</summary>
        public static readonly KeyboardCode C;

        /// <summary>V</summary>
        public static readonly KeyboardCode V;

        /// <summary>B</summary>
        public static readonly KeyboardCode B;

        /// <summary>N</summary>
        public static readonly KeyboardCode N;

        /// <summary>M</summary>
        public static readonly KeyboardCode M;

        /// <summary>,</summary>
        public static readonly KeyboardCode Comma;

        /// <summary>.</summary>
        public static readonly KeyboardCode Period;

        /// <summary>/</summary>
        public static readonly KeyboardCode Slash;

        /// <summary>右Shift</summary>
        public static readonly KeyboardCode RightShift;

        /// <summary>テンキー *</summary>
        public static readonly KeyboardCode NumericKeypadAsterisk;

        /// <summary>左Alt</summary>
        public static readonly KeyboardCode LeftAlt;

        /// <summary>Space</summary>
        public static readonly KeyboardCode Space;

        /// <summary>Caps</summary>
        public static readonly KeyboardCode Caps;

        /// <summary>F1</summary>
        public static readonly KeyboardCode F1;

        /// <summary>F2</summary>
        public static readonly KeyboardCode F2;

        /// <summary>F3</summary>
        public static readonly KeyboardCode F3;

        /// <summary>F4</summary>
        public static readonly KeyboardCode F4;

        /// <summary>F5</summary>
        public static readonly KeyboardCode F5;

        /// <summary>F6</summary>
        public static readonly KeyboardCode F6;

        /// <summary>F7</summary>
        public static readonly KeyboardCode F7;

        /// <summary>F8</summary>
        public static readonly KeyboardCode F8;

        /// <summary>F9</summary>
        public static readonly KeyboardCode F9;

        /// <summary>F10</summary>
        public static readonly KeyboardCode F10;

        /// <summary>Scroll</summary>
        public static readonly KeyboardCode Scroll;

        /// <summary>テンキー7</summary>
        public static readonly KeyboardCode NumericKeypadSeven;

        /// <summary>テンキー8</summary>
        public static readonly KeyboardCode NumericKeypadEight;

        /// <summary>テンキー9</summary>
        public static readonly KeyboardCode NumericKeypadNine;

        /// <summary>テンキー -</summary>
        public static readonly KeyboardCode NumericKeypadHyphen;

        /// <summary>テンキー4</summary>
        public static readonly KeyboardCode NumericKeypadFour;

        /// <summary>テンキー5</summary>
        public static readonly KeyboardCode NumericKeypadFive;

        /// <summary>テンキー6</summary>
        public static readonly KeyboardCode NumericKeypadSix;

        /// <summary>テンキー +</summary>
        public static readonly KeyboardCode NumericKeypadPlus;

        /// <summary>テンキー1</summary>
        public static readonly KeyboardCode NumericKeypadOne;

        /// <summary>テンキー2</summary>
        public static readonly KeyboardCode NumericKeypadTwo;

        /// <summary>テンキー3</summary>
        public static readonly KeyboardCode NumericKeypadThree;

        /// <summary>テンキー0</summary>
        public static readonly KeyboardCode NumericKeypadZero;

        /// <summary>テンキー .</summary>
        public static readonly KeyboardCode NumericKeypadPeriod;

        /// <summary>F11</summary>
        public static readonly KeyboardCode F11;

        /// <summary>F12</summary>
        public static readonly KeyboardCode F12;

        /// <summary>ｶﾀｶﾀ・ひらがな</summary>
        public static readonly KeyboardCode KanaToggle;

        /// <summary>変換[切替]</summary>
        public static readonly KeyboardCode Henkan;

        /// <summary>無変換[切替]</summary>
        public static readonly KeyboardCode Muhenkan;

        /// <summary>\\</summary>
        public static readonly KeyboardCode Yen;

        /// <summary>^</summary>
        public static readonly KeyboardCode Caret;

        /// <summary>@</summary>
        public static readonly KeyboardCode AtSign;

        /// <summary>:</summary>
        public static readonly KeyboardCode Colon;

        /// <summary>半角/全角[切替]</summary>
        public static readonly KeyboardCode Kanji;

        /// <summary>右Ctrl</summary>
        public static readonly KeyboardCode RightCtrl;

        /// <summary>テンキー /</summary>
        public static readonly KeyboardCode NumericKeypadSlash;

        /// <summary>PrintScreen</summary>
        public static readonly KeyboardCode PrintScreen;

        /// <summary>右Alt</summary>
        public static readonly KeyboardCode RightAlt;

        /// <summary>Pause</summary>
        public static readonly KeyboardCode Pause;

        /// <summary>HOME</summary>
        public static readonly KeyboardCode Home;

        /// <summary>↑</summary>
        public static readonly KeyboardCode Up;

        /// <summary>PageUp</summary>
        public static readonly KeyboardCode PageUp;

        /// <summary>←</summary>
        public static readonly KeyboardCode Left;

        /// <summary>→</summary>
        public static readonly KeyboardCode Right;

        /// <summary>END</summary>
        public static readonly KeyboardCode End;

        /// <summary>↓</summary>
        public static readonly KeyboardCode Down;

        /// <summary>PageDown</summary>
        public static readonly KeyboardCode PageDown;

        /// <summary>DEL</summary>
        public static readonly KeyboardCode Del;

        private const string NotFound = "非対応";

        private const int KeyCodeMin = 100;
        private const int KeyCodeMax = 355;

        /// <summary>キーコード値</summary>
        public int KeyCode { get; }

        /// <summary>キー名称</summary>
        public string KeyName { get; }

        static KeyboardCode()
        {
            Esc = new KeyboardCode(101, "ESC");
            One = new KeyboardCode(102, "1");
            Two = new KeyboardCode(103, "2");
            Three = new KeyboardCode(104, "3");
            Four = new KeyboardCode(105, "4");
            Five = new KeyboardCode(106, "5");
            Six = new KeyboardCode(107, "6");
            Seven = new KeyboardCode(108, "7");
            Eight = new KeyboardCode(109, "8");
            Nine = new KeyboardCode(110, "9");
            Zero = new KeyboardCode(111, "0");
            Hyphen = new KeyboardCode(112, "-");
            BackSpace = new KeyboardCode(114, "BS");
            Tab = new KeyboardCode(115, "TAB");
            Q = new KeyboardCode(116, "Q");
            W = new KeyboardCode(117, "W");
            E = new KeyboardCode(118, "E");
            R = new KeyboardCode(119, "R");
            T = new KeyboardCode(120, "T");
            Y = new KeyboardCode(121, "Y");
            U = new KeyboardCode(122, "U");
            I = new KeyboardCode(123, "I");
            O = new KeyboardCode(124, "O");
            P = new KeyboardCode(125, "P");
            LeftBracket = new KeyboardCode(126, "[");
            RightBracket = new KeyboardCode(127, "]");
            Enter = new KeyboardCode(128, "Enter");
            LeftCtrl = new KeyboardCode(129, "左Ctrl");
            A = new KeyboardCode(130, "A");
            S = new KeyboardCode(131, "S");
            D = new KeyboardCode(132, "D");
            F = new KeyboardCode(133, "F");
            G = new KeyboardCode(134, "G");
            H = new KeyboardCode(135, "H");
            J = new KeyboardCode(136, "J");
            K = new KeyboardCode(137, "K");
            L = new KeyboardCode(138, "L");
            Semicolon = new KeyboardCode(139, ";");
            LeftShift = new KeyboardCode(142, "左Shift");
            BackSlash = new KeyboardCode(143, "＼");
            Z = new KeyboardCode(144, "Z");
            X = new KeyboardCode(145, "X");
            C = new KeyboardCode(146, "C");
            V = new KeyboardCode(147, "V");
            B = new KeyboardCode(148, "B");
            N = new KeyboardCode(149, "N");
            M = new KeyboardCode(150, "M");
            Comma = new KeyboardCode(151, ",");
            Period = new KeyboardCode(152, ".");
            Slash = new KeyboardCode(153, "/");
            RightShift = new KeyboardCode(154, "右Shift");
            NumericKeypadAsterisk = new KeyboardCode(155, "テンキー *");
            LeftAlt = new KeyboardCode(156, "左Alt");
            Space = new KeyboardCode(157, "Space");
            Caps = new KeyboardCode(158, "Caps");
            F1 = new KeyboardCode(159, "F1");
            F2 = new KeyboardCode(160, "F2");
            F3 = new KeyboardCode(161, "F3");
            F4 = new KeyboardCode(162, "F4");
            F5 = new KeyboardCode(163, "F5");
            F6 = new KeyboardCode(164, "F6");
            F7 = new KeyboardCode(165, "F7");
            F8 = new KeyboardCode(166, "F8");
            F9 = new KeyboardCode(167, "F9");
            F10 = new KeyboardCode(168, "F10");
            Scroll = new KeyboardCode(170, "Scroll");
            NumericKeypadSeven = new KeyboardCode(171, "テンキー7");
            NumericKeypadEight = new KeyboardCode(172, "テンキー8");
            NumericKeypadNine = new KeyboardCode(173, "テンキー9");
            NumericKeypadHyphen = new KeyboardCode(174, "テンキー -");
            NumericKeypadFour = new KeyboardCode(175, "テンキー4");
            NumericKeypadFive = new KeyboardCode(176, "テンキー5");
            NumericKeypadSix = new KeyboardCode(177, "テンキー6");
            NumericKeypadPlus = new KeyboardCode(178, "テンキー +");
            NumericKeypadOne = new KeyboardCode(179, "テンキー1");
            NumericKeypadTwo = new KeyboardCode(180, "テンキー2");
            NumericKeypadThree = new KeyboardCode(181, "テンキー3");
            NumericKeypadZero = new KeyboardCode(182, "テンキー0");
            NumericKeypadPeriod = new KeyboardCode(183, "テンキー .");
            F11 = new KeyboardCode(187, "F11");
            F12 = new KeyboardCode(188, "F12");
            KanaToggle = new KeyboardCode(212, "ｶﾀｶﾀ・ひらがな");
            Henkan = new KeyboardCode(221, "変換[切替]");
            Muhenkan = new KeyboardCode(223, "無変換[切替]");
            Yen = new KeyboardCode(225, "\\");
            Caret = new KeyboardCode(244, "^");
            AtSign = new KeyboardCode(245, "@");
            Colon = new KeyboardCode(246, ":");
            Kanji = new KeyboardCode(248, "半角/全角[切替]");
            RightCtrl = new KeyboardCode(257, "右Ctrl");
            NumericKeypadSlash = new KeyboardCode(281, "テンキー /");
            PrintScreen = new KeyboardCode(283, "PrintScreen");
            RightAlt = new KeyboardCode(284, "右Alt");
            Pause = new KeyboardCode(297, "Pause");
            Home = new KeyboardCode(299, "HOME");
            Up = new KeyboardCode(300, "↑");
            PageUp = new KeyboardCode(301, "PageUp");
            Left = new KeyboardCode(303, "←");
            Right = new KeyboardCode(305, "→");
            End = new KeyboardCode(307, "END");
            Down = new KeyboardCode(308, "↓");
            PageDown = new KeyboardCode(309, "PageDown");
            Del = new KeyboardCode(311, "DEL");
        }


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="keyCode">キーコード値</param>
        /// <param name="keyName">キー名称</param>
        private KeyboardCode(int keyCode, string keyName) : base(keyCode.ToString())
        {
            KeyCode = keyCode;
            KeyName = keyName;
        }

        /// <summary>
        ///     指定したint値がキーコード値であるかどうかを判定して返す。
        /// </summary>
        /// <param name="value">判定対象値</param>
        /// <returns>キーコード値の場合 <see langword="true"/></returns>
        public static bool IsKeyCode(int value)
        {
            return value.IsBetween(KeyCodeMin, KeyCodeMax);
        }

        /// <summary>
        ///     キーコード値からキー名を取得する。
        /// </summary>
        /// <param name="keyCode">キーコード</param>
        /// <returns>インスタンス</returns>
        public static string GetKeyName(int keyCode)
        {
            return AllItems.FirstOrDefault(x => x.KeyCode == keyCode)?.KeyName ?? NotFound;
        }
    }
}
