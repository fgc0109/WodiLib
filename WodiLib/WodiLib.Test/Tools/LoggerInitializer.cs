using System;
using System.Collections.Generic;
using Commons;
using WodiLib.Sys.Cmn;

namespace WodiLib.Test.Tools
{
    public static class LoggerInitializer
    {
        public static readonly string KeyNameForDebug = "forDebug";
        public static readonly string KeyNameForProjectTest = "forProjectTest";

        /// <summary>
        ///     デバッグ用Loggerのセットを行う
        /// </summary>
        public static void SetupLoggerForDebug()
        {
            var logHandler = new LogHandler(
                Console.WriteLine,
                Console.WriteLine,
                Console.WriteLine,
                Console.WriteLine,
                ExceptionAction
            );

            Logger.ChangeTargetKey(KeyNameForDebug);
            Logger.SetLogHandler(logHandler);
        }

        /// <summary>
        ///     Project テスト用のロガーインスタンスを生成する。
        /// </summary>
        /// <returns></returns>
        public static void SetupLoggerForProjectTest()
        {
            var logHandler = new LogHandler(
                Console.WriteLine,
                Console.WriteLine,
                exceptionAction: ExceptionAction
            );

            Logger.ChangeTargetKey(KeyNameForProjectTest);
            Logger.SetLogHandler(logHandler);
        }

        public static WodiLibLogHandlerContainer SetupWodiLibLogHandler(string keyName)
        {
            var handlerContainer = new WodiLibLogHandlerContainer();
            WodiLibLogger.SetLogHandler(handlerContainer.Handler, keyName);
            return handlerContainer;
        }

        /// <summary>
        ///     エラー情報を出力する。
        /// </summary>
        /// <param name="ex">例外</param>
        private static void ExceptionAction(Exception ex)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
        }

        public class WodiLibLogHandlerContainer
        {
            public WodiLibLogHandler Handler;

            public IReadOnlyList<string> ErrorLogs => errorLogs;
            public IReadOnlyList<string> WarningLogs => warningLogs;
            public IReadOnlyList<string> InfoLogs => infoLogs;
            public IReadOnlyList<string> DebugLogs => debugLogs;

            private readonly List<string> errorLogs = new();
            private readonly List<string> warningLogs = new();
            private readonly List<string> infoLogs = new();
            private readonly List<string> debugLogs = new();

            public WodiLibLogHandlerContainer()
            {
                Handler = new WodiLibLogHandler(
                    errorAction: makeHandleAction(errorLogs),
                    warningAction: makeHandleAction(warningLogs),
                    infoAction: makeHandleAction(infoLogs),
                    debugAction: makeHandleAction(debugLogs)
                );
            }

            private Action<string?> makeHandleAction(List<string> logs)
                => new(msg =>
                    {
                        if (msg is not null)
                        {
                            logs.Add(msg);
                        }
                    }
                );

            public void ClearAllLogs()
            {
                errorLogs.Clear();
                warningLogs.Clear();
                infoLogs.Clear();
                debugLogs.Clear();
            }
        }
    }
}
