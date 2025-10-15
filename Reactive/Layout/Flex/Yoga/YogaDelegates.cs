using System;
using System.Runtime.InteropServices;

namespace Reactive.Yoga;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void YogaLoggerDelegate(LogLevel logLevel, string format);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate YogaSize YGMeasureFunc(IntPtr node, float width, MeasureMode widthMode, float height, MeasureMode heightMode);