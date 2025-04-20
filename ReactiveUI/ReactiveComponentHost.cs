using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Reactive {
    public partial class ReactiveComponent {
        [RequireComponent(typeof(RectTransform))]
        private class ReactiveHost : MonoBehaviour, ILayoutItem, IEffectBinder, IReactiveModuleBinder {
            #region LayoutItem

            public ILayoutDriver? LayoutDriver {
                get => _layoutDriver;
                set {
                    if (_layoutDriver == value) return;
                    // do not change! truncation before assignment can cause deadlock!
                    var oldDriver = _layoutDriver;
                    _layoutDriver = value;
                    oldDriver?.Children.Remove(this);
                    _layoutDriver?.Children.Add(this);
                }
            }

            public ILayoutModifier? LayoutModifier {
                get => _modifier;
                set {
                    if (_modifier != null) {
                        _modifier.ExposeLayoutItem(null);
                        _modifier.ModifierUpdatedEvent -= HandleModifierUpdated;
                        ReleaseContextMember(_modifier);
                    }

                    _modifier = value;

                    if (_modifier != null) {
                        _modifier.ModifierUpdatedEvent += HandleModifierUpdated;
                        InsertContextMember(_modifier);

                        // Normally the main component is the component that was created the last
                        var primaryComponent = _components.LastOrDefault();
                        _modifier.ExposeLayoutItem(primaryComponent);
                    }

                    HandleModifierUpdated();
                }
            }

            public bool WithinLayout {
                get => Enabled || WithinLayoutIfDisabled;
                set {
                    if (WithinLayoutIfDisabled) return;
                    Enabled = value;
                }
            }

            public bool WithinLayoutIfDisabled { get; set; }

            public event Action<ILayoutItem>? ModifierUpdatedEvent;

            private ILayoutDriver? _layoutDriver;
            private ILayoutModifier? _modifier;
            private bool _beingRecalculated;

            public RectTransform BeginApply() {
                if (_beingRecalculated) {
                    throw new InvalidOperationException("Cannot begin layout application as it's already started");
                }

                _beingRecalculated = true;
                return _rectTransform;
            }

            public void EndApply() {
                _beingRecalculated = false;
                _components.ForEach(static x => x.OnLayoutApply());
            }

            // TODO: Introduce dict with item hashes. Add GetItemHash to ILayoutItem
            public bool Equals(ILayoutItem other) {
                return ReferenceEquals(other, this) || _components.Any((ILayoutItem x) => x == other);
            }

            public override bool Equals(object? other) {
                return other is ILayoutItem item && Equals(item);
            }

            public override int GetHashCode() {
                return base.GetHashCode();
            }

            #endregion

            #region Layout

            private bool _recalculationScheduled;

            public void ScheduleLayoutRecalculation() {
                _recalculationScheduled = true;
            }

            private void HandleModifierUpdated() {
                ModifierUpdatedEvent?.Invoke(this);
                _components.ForEach(static x => x.OnModifierUpdated());
            }

            #endregion

            #region Effects

            private abstract class EffectBindingBase {
                public abstract void UnbindAll();
            }

            private class EffectBinding<T> : EffectBindingBase {
                public EffectBinding(INotifyValueChanged<T> value) {
                    _value = value;
                }

                private readonly INotifyValueChanged<T> _value;
                private readonly HashSet<IEffect<T>> _effects = new();

                public void Bind(IEffect<T> effect) {
                    _value.ValueChangedEvent += effect.Invoke;
                    _effects.Add(effect);
                }

                public void Unbind(IEffect<T> effect) {
                    if (!_effects.Contains(effect)) return;
                    _value.ValueChangedEvent -= effect.Invoke;
                    _effects.Remove(effect);
                }

                public override void UnbindAll() {
                    foreach (var effect in _effects) {
                        _value.ValueChangedEvent -= effect.Invoke;
                    }
                    _effects.Clear();
                }
            }

            private readonly Dictionary<object, EffectBindingBase> _effects = new();

            // TODO: Remove all this crap and use pure delegates
            public void BindEffect<T>(INotifyValueChanged<T> value, IEffect<T> effect) {
                EffectBinding<T> binding;
                if (!_effects.TryGetValue(value, out var bin)) {
                    binding = new EffectBinding<T>(value);
                    _effects[value] = binding;
                } else {
                    binding = (EffectBinding<T>)bin;
                }
                binding.Bind(effect);
            }

            public void UnbindEffect<T>(INotifyValueChanged<T> value, IEffect<T> effect) {
                if (!_effects.TryGetValue(value, out var bin)) return;
                var binding = (EffectBinding<T>)bin;
                binding.Unbind(effect);
            }

            private void UnbindAllEffects() {
                foreach (var (_, binding) in _effects) {
                    binding.UnbindAll();
                }
            }

            #endregion

            #region Modules

            private readonly HashSet<IReactiveModule> _modules = new();

            public void BindModule(IReactiveModule module) {
                _modules.Add(module);
            }

            public void UnbindModule(IReactiveModule module) {
                _modules.Remove(module);
            }

            #endregion

            #region Contexts

            private readonly Dictionary<Type, (object context, int members)> _contexts = new();

            public void InsertContextMember(IContextMember member) {
                var type = member.ContextType;
                if (type == null) return;
                object context;
                var members = 1;
                if (_contexts.TryGetValue(type, out var tuple)) {
                    context = tuple.context;
                    members += tuple.members;
                } else {
                    context = member.CreateContext();
                }
                member.ProvideContext(context);
                _contexts[type] = (context, members);
            }

            public void ReleaseContextMember(IContextMember member) {
                var type = member.ContextType;
                if (type == null || !_contexts.TryGetValue(type, out var tuple)) return;
                var members = tuple.members - 1;
                if (members == 0) {
                    _contexts.Remove(type);
                } else {
                    tuple.members = members;
                    _contexts[type] = tuple;
                }
                member.ProvideContext(null);
            }

            #endregion

            #region Components

            public bool Enabled {
                get => gameObject.activeInHierarchy;
                set => gameObject.SetActive(value);
            }

            public bool IsStarted { get; private set; }
            public bool IsDestroyed { get; private set; }

            private readonly List<ReactiveComponent> _components = new();

            public void AddComponent(ReactiveComponent comp) {
                _components.Add(comp);
                if (IsStarted) comp.OnStart();
            }

            public void RemoveComponent(ReactiveComponent comp) {
                _components.Remove(comp);
            }

            #endregion

            #region Events

            private RectTransform _rectTransform = null!;

            private void Awake() {
                _rectTransform = GetComponent<RectTransform>();
            }

            private void Start() {
                _components.ForEach(static x => x.OnStart());
                IsStarted = true;
            }

            private void Update() {
                _components.ForEach(static x => x.OnUpdate());
                _modules.ForEach(static x => x.OnUpdate());
            }

            private void LateUpdate() {
                if (_recalculationScheduled) {
                    if (LayoutDriver != null) {
                        // If a layout driver is presented, start recalculation from this point
                        LayoutDriver.RecalculateLayout();
                    } else {
                        // If not, tell own components to start recalculation (if there is a Layout or a custom layout controller) 
                        _components.ForEach(static x => x.OnRecalculateLayoutSelf());
                    }

                    _recalculationScheduled = false;
                }
                _components.ForEach(static x => x.OnLateUpdate());
            }

            private void OnDestroy() {
                UnbindAllEffects();
                _components.ForEach(static x => x.DestroyInternal());
                _modules.ForEach(static x => x.OnDestroy());
                IsDestroyed = true;
            }

            private void OnEnable() {
                ScheduleLayoutRecalculation();
                _components.ForEach(static x => x.OnEnable());
            }

            private void OnDisable() {
                ScheduleLayoutRecalculation();
                _components.ForEach(static x => x.OnDisable());
            }

            private void OnRectTransformDimensionsChange() {
                // if we look at how the reactive layout system works
                // we'll see that it goes to the root driver and starts recalculation from there.
                // it means that layout controller will be able to modify rect properties
                // which is not allowed to be performed on the OnRectTransformDimensionsChange stack.
                // that's why we schedule recalculation to the end of this frame
                if (!_beingRecalculated) {
                    _recalculationScheduled = true;
                }
                _components.ForEach(static x => x.OnRectDimensionsChanged());
            }

            #endregion
        }
    }
}