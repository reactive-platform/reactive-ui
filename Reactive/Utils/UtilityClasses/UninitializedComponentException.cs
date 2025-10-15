using System;

namespace Reactive {
    public class UninitializedComponentException(string message) : Exception(message) {
        public UninitializedComponentException() : this("The component was not initialized") {}
    }
}