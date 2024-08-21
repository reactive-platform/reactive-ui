using Reactive.Yoga;

namespace Reactive {
    internal class YogaContext {
        public YogaNode YogaNode {
            get {
                _yogaNode ??= new();
                return _yogaNode;
            }
        }

        private YogaNode? _yogaNode;
    }
}