using System.Runtime.InteropServices;

namespace Reactive.Yoga;

[StructLayout(LayoutKind.Sequential)]
internal struct YogaSize {
    public float width;
    public float height;
}