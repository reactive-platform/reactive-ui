using Reactive.Yoga;

namespace Reactive {
    internal class YogaContext {
        public YogaNode YogaNode {
            get {
                _yogaNode ??= YogaNode.New();
                return _yogaNode;
            }
        }

        ~YogaContext() {
            _yogaNode?.Dispose();
        }

        private YogaNode? _yogaNode;
    }
}