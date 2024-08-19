﻿using Reactive.Yoga;

namespace Reactive {
    internal class YogaContext {
        public YogaNode YogaNode {
            get {
                _yogaNode.Touch();
                return _yogaNode;
            }
        }

        private YogaNode _yogaNode;
    }
}