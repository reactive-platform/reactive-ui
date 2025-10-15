using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive.Yoga {
    [PublicAPI]
    public struct YogaConfig {
        public static YogaConfig Default => new() {
            _ptr = YogaNative.YGConfigGetDefault()
        };

        private IntPtr _ptr;

        public void SetPointScaleFactor(float factor) {
            YogaNative.YGConfigSetPointScaleFactor(_ptr, factor);
        }

        public void SetLogger(YogaLoggerDelegate? callback) {
            YogaNative.YGConfigSetLogger(_ptr, callback);
        }

        public void SetDefaultLogger() {
            SetLogger(LogUnity);
        }

        private static void LogUnity(LogLevel logLevel, string message) {
            var logType = ToLogType(logLevel);

            message = $"Yoga Native: {message}";

            if (logLevel is LogLevel.Fatal) {
                // Fatal log usually comes before a native exception, which is not handled by the runtime and causes a crash. 
                // By throwing a handled exception, we prevent the call stack from going further and causing an unmanaged exception.
                throw new Exception($"{message}");
            } else {
                Debug.unityLogger.Log(logType, message);
            }
        }

        private static LogType ToLogType(LogLevel level) {
            return level switch {
                LogLevel.Debug => LogType.Log,
                LogLevel.Error => LogType.Error,
                LogLevel.Info => LogType.Log,
                LogLevel.Verbose => LogType.Log,
                LogLevel.Warn => LogType.Warning,
                LogLevel.Fatal => LogType.Exception,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }
    }
}