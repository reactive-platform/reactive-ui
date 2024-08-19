using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public struct ComponentState {
        public static readonly ComponentState Default;
        public static readonly ComponentState Hovered = "hovered";
        public static readonly ComponentState Pressed = "pressed";
        public static readonly ComponentState Focused = "focused";

        #region Logic

        private string[] Names => _names ?? Array.Empty<string>();

        private string[]? _names;
        private int _hash;

        public bool RepresentedIn(IEnumerable<ComponentState> states) {
            return Names.All(x => states.Contains(x));
        }

        public bool EligibleForState(ComponentState state) {
            if (Names.Length == 0) {
                return false;
            }
            var arr = state._names;
            if (_names == null || arr == null) {
                return true;
            }
            foreach (var name in _names) {
                if (!Contains(arr, name)) return false;
            }
            return true;
        }

        public ComponentState Add(ComponentState state) {
            var arr = Names.Concat(state.Names).ToArray();
            return new ComponentState {
                _names = arr,
                _hash = CalculateHash(arr)
            };
        }

        public ComponentState Except(ComponentState state) {
            var arr = Names.Except(state.Names).ToArray();
            return new ComponentState {
                _names = arr,
                _hash = CalculateHash(arr)
            };
        }

        #endregion

        #region Operators

        public static ComponentState operator |(ComponentState one, ComponentState second) {
            return one.Add(second);
        }

        public static ComponentState operator ^(ComponentState one, ComponentState second) {
            return one.Except(second);
        }

        public static bool operator !=(ComponentState one, ComponentState second) {
            return !(one == second);
        }

        public static bool operator ==(ComponentState one, ComponentState second) {
            return one.Equals(second);
        }

        public static implicit operator ComponentState(string name) {
            return new ComponentState {
                _names = new[] { name },
                _hash = name.GetHashCode()
            };
        }

        #endregion

        #region Object Overrides

        public override int GetHashCode() {
            return _hash;
        }

        public override string ToString() {
            return Names.Aggregate("", (x, y) => $"{x} {y}");
        }

        public override bool Equals(object? obj) {
            return obj is ComponentState state && Equals(state);
        }

        public bool Equals(ComponentState state) {
            return CompareArr(Names, state.Names);
        }

        #endregion

        #region Tools

        private static int CalculateHash(string[] arr) {
            return arr.Aggregate(0, static (x, y) => x + y.GetHashCode());
        }

        private static bool Contains(string[] arr1, string item) {
            // using foreach instead of linq to avoid additional allocations
            // since this method is called multiple times every frame.
            // it still allocates enumerator, but disposes it once loop is finished
            foreach (var t in arr1) {
                if (t == item) return true;
            }
            return false;
        }
        
        private static bool CompareArr(string[] arr1, string[] arr2) {
            if (arr1.Length != arr2.Length) {
                return false;
            }
            // using for instead of linq to avoid additional allocations
            // since this method is called multiple times every frame
            for (var i = 0; i < arr1.Length; i++) {
                if (arr1[i] != arr2[i]) {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}