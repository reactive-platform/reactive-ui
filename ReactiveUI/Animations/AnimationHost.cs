using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Animations = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<Reactive.ComponentState, Reactive.IAnimation>>;

namespace Reactive {
    [PublicAPI]
    public class AnimationHost : IStateAnimationHost {
        #region Impl

        public IReadOnlyCollection<IAnimation> Animations => _animations.Keys;

        public void AddStateAnimation(ComponentState state, IAnimation anim) {
            AddAnimationInternal(state, anim, _hashedAnimations);
        }

        public void AddStateTransition(ComponentState state, IAnimation anim) {
            AddAnimationInternal(state, anim, _hashedTransitions);
        }

        public void AddAnimation(IAnimation anim) {
            _animations[anim] = new();
        }

        public void PauseAnimation(IAnimation anim) {
            ValidateAnimation(anim);
            _animations[anim].Paused = true;
        }

        public void ResumeAnimation(IAnimation anim) {
            ValidateAnimation(anim);
            _animations[anim].Paused = false;
        }

        private void AddAnimationInternal(ComponentState state, IAnimation anim, Animations animations) {
            if (!state.RepresentedIn(AvailableStates)) {
                throw new InvalidOperationException($"The component does not have a state \"{state}\"");
            }
            _animations[anim] = new() {
                GraphicState = state
            };
            var hash = GetAnimationHash(anim);
            if (!animations.TryGetValue(hash, out var states)) {
                states = new();
                animations[hash] = states;
            }
            states[state] = anim;
        }

        #endregion

        #region Interaction

        public readonly HashSet<ComponentState> AvailableStates = new() {
            ComponentState.Default,
            ComponentState.Hovered,
            ComponentState.Pressed,
            ComponentState.Focused
        };
        
        public void UpdateState(ComponentState state) {
            _graphicState = state;
        }

        public void Update() {
            UpdateAnimations();
        }

        #endregion

        #region Update

        private class AnimationData {
            public ComponentState? GraphicState;
            public bool Paused;
        }

        private readonly Dictionary<IAnimation, AnimationData> _animations = new();
        private readonly Animations _hashedAnimations = new();
        private readonly Animations _hashedTransitions = new();
        private readonly List<IAnimation> _animationsBuffer = new();
        private readonly HashSet<int> _transitionsBuffer = new();
        private ComponentState _prevGraphicState;
        private ComponentState _graphicState;

        public ComponentState? GetAnimationState(IAnimation anim) {
            ValidateAnimation(anim);
            return _animations[anim].GraphicState;
        }

        private void UpdateAnimations() {
            var delta = Time.deltaTime;
            // processing default animations
            foreach (var (anim, data) in _animations) {
                if (data.GraphicState != null || data.Paused) continue;
                // removing on the next update to let external logic
                // handle finished animations if needed
                if (anim.IsFinished) {
                    _animationsBuffer.Add(anim);
                    continue;
                }
                anim.Evaluate(delta);
            }
            // removing finished animations
            foreach (var anim in _animationsBuffer) {
                _animations.Remove(anim);
            }
            _animationsBuffer.Clear();
            // processing state transitions
            foreach (var (hash, states) in _hashedTransitions) {
                ResetStateAnimation(states);
                var hasTransition = EvaluateStateAnimation(delta, states, true);
                if (hasTransition) {
                    _transitionsBuffer.Add(hash);
                }
            }
            // processing state animations
            foreach (var (hash, states) in _hashedAnimations) {
                ResetStateAnimation(states);
                if (_transitionsBuffer.Contains(hash)) continue;
                EvaluateStateAnimation(delta, states, false);
            }
            _transitionsBuffer.Clear();
            _prevGraphicState = _graphicState;
        }

        private void ResetStateAnimation(Dictionary<ComponentState, IAnimation> animations) {
            var shouldResetAnimations = _prevGraphicState != _graphicState;
            if (shouldResetAnimations && TryGetAnimation(_prevGraphicState, animations, true, out var prevAnim)) {
                prevAnim.Reset();
            }
        }

        private bool EvaluateStateAnimation(float delta, Dictionary<ComponentState, IAnimation> animations, bool allowDefault) {
            if (TryGetAnimation(_graphicState, animations, allowDefault, out var anim)) {
                anim.Evaluate(delta);
                return !anim.IsFinished;
            }
            return false;
        }

        #endregion

        #region Tools

        private void ValidateAnimation(IAnimation anim) {
            if (_animations.ContainsKey(anim)) return;
            throw new ArgumentException("The host does not control the specified animation");
        }

        private static int GetAnimationHash(IAnimation anim) {
            return anim.Target?.GetHashCode() + anim.PropertyName?.GetHashCode() ?? 0;
        }

        private static bool TryGetAnimation(ComponentState state, Dictionary<ComponentState, IAnimation> states, bool allowDefault, out IAnimation anim) {
            if (states.TryGetValue(state, out anim)) {
                return true;
            }
            foreach (var (st, animation) in states) {
                if (!st.EligibleForState(state)) continue;
                anim = animation;
                return true;
            }
            if (allowDefault && states.TryGetValue(ComponentState.Default, out anim)) {
                return true;
            }
            anim = default!;
            return false;
        }

        #endregion
    }
}