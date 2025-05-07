﻿using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface IReactiveModule {
        void OnUpdate();
        void OnBind();
        void OnUnbind();
    }
}