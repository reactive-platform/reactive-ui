using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reactive {
    public partial class ReactiveComponent {
        [RequireComponent(typeof(RectTransform))]
        private class ReactiveHost : MonoBehaviour, ILayoutItem, IReactiveModuleBinder {
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
                        ReloadLayoutFirstComponent();
                    }

                    HandleModifierUpdated();
                }
            }

            public bool WithinLayout {
                get => gameObject.activeSelf || WithinLayoutIfDisabled;
                set {
                    if (WithinLayoutIfDisabled) return;
                    gameObject.SetActive(value);
                }
            }

            public bool WithinLayoutIfDisabled { get; set; }

            public event Action<ILayoutItem>? ModifierUpdatedEvent;
            public event Action<ILayoutItem>? StateUpdatedEvent;

            private ILayoutDriver? _layoutDriver;
            private ILayoutModifier? _modifier;
            private bool _beingApplied;

            public int GetLayoutItemHashCode() {
                return base.GetHashCode();
            }

            public bool EqualsToLayoutItem(ILayoutItem item) {
                return item.GetLayoutItemHashCode() == GetLayoutItemHashCode();
            }

            public RectTransform BeginApply() {
                if (_beingApplied) {
                    throw new InvalidOperationException("Cannot begin layout application as it's already started");
                }

                _beingApplied = true;
                return _rectTransform;
            }

            public void EndApply() {
                _beingApplied = false;
                _components.ForEach(static x => x.OnLayoutApply());
            }

            #endregion

            #region Layout

            private bool _recalculationScheduled;
            private bool _beingRecalculated;

            public void ScheduleLayoutRecalculation() {
                _recalculationScheduled = true;
            }

            public void RecalculateLayoutImmediate() {
                if (_beingRecalculated) {
                    throw new InvalidOperationException("Calling RecalculateLayoutImmediate in a layout cycle is not allowed");
                }

                _beingRecalculated = true;

                if (LayoutModifier != null && LayoutDriver?.LayoutController != null) {
                    // If a layout driver is presented, start recalculation from this point
                    LayoutDriver.RecalculateLayoutImmediate();
                } else {
                    // If not, tell own components to start recalculation (if there is a Layout or a custom layout controller) 
                    _components.ForEach(static x => x.OnRecalculateLayoutSelf());
                }

                _beingRecalculated = false;
            }

            private void ScheduleLayoutRecalculationAfterStateChange(bool newState) {
                if (newState) {
                    ScheduleLayoutRecalculation();
                } else if (LayoutDriver != null) {
                    // The current object is disabled and LateUpdate won't be called, so we try to 
                    // move the recalculation responsibility to the driver if possible. 
                    // If this object doesn't have a driver, recalculation is omitted as it will
                    // happen anyway when the object will get enabled back.
                    LayoutDriver.ScheduleLayoutRecalculation();
                }
            }

            private void HandleModifierUpdated() {
                ModifierUpdatedEvent?.Invoke(this);
                _components.ForEach(static x => x.OnModifierUpdated());
            }

            #endregion

            #region Modules

            public IReadOnlyCollection<IReactiveModule> Modules {
                get {
                    // Do not change as _modules ?? [] will produce a hashset instance instead of using Array.Empty<>
                    if (_modules == null) {
                        return [];
                    }

                    return _modules;
                }
            }

            private HashSet<IReactiveModule>? _modules;

            public void BindModule(IReactiveModule module) {
                _modules ??= new();
                _modules.Add(module);
                module.OnBind();
            }

            public void UnbindModule(IReactiveModule module) {
                _modules?.Remove(module);
                module.OnUnbind();
            }

            #endregion

            #region Contexts

            private Dictionary<Type, (object context, int members)>? _contexts;

            public void InsertContextMember(IContextMember member) {
                _contexts ??= new(1);

                var type = member.ContextType;
                if (type == null) {
                    return;
                }

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
                if (_contexts == null) {
                    return;
                }

                var type = member.ContextType;
                if (type == null || !_contexts.TryGetValue(type, out var tuple)) {
                    return;
                }

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
                set {
                    gameObject.SetActive(value);

                    if (!gameObject.activeInHierarchy) {
                        // When turning the object off\on in a disabled hierarchy,
                        // OnEnable and OnDisable events are not called, so we notify listeners manually
                        StateUpdatedEvent?.Invoke(this);
                        // To force layout recalculation when enabled
                        _wasActuallyDisabled = true;
                    }
                }
            }

            public bool IsStarted { get; private set; }
            public bool IsDestroyed { get; private set; }

            private readonly List<ReactiveComponent> _components = new();
            private ReactiveComponent? _definedExposeComponent;
            private ReactiveComponent? _lastExposeComponent;

            public void ExposeLayoutFirstComponent(ReactiveComponent? comp) {
                if (comp != null && !_components.Contains(comp)) {
                    throw new InvalidOperationException("Cannot expose a component that does not belong to the host");
                }

                _definedExposeComponent = comp;
                ReloadLayoutFirstComponent();
            }

            private void ReloadLayoutFirstComponent() {
                // Normally the main component is a component that was created the last
                var comp = _definedExposeComponent ?? _components.LastOrDefault();

                if (comp != null && comp != _lastExposeComponent) {
                    _lastExposeComponent = comp;
                    _modifier?.ExposeLayoutItem(comp);
                }
            }

            public void AddComponent(ReactiveComponent comp) {
                _components.Add(comp);
                ReloadLayoutFirstComponent();

                if (IsStarted) {
                    comp.OnStart();
                }
            }

            public void RemoveComponent(ReactiveComponent comp) {
                _components.Remove(comp);
            }

            #endregion

            #region Events

            private RectTransform _rectTransform = null!;
            private bool _wasActuallyDisabled = true;

            private void Awake() {
                _rectTransform = GetComponent<RectTransform>();
            }

            private void Start() {
                _components.ForEach(static x => x.OnStart());
                IsStarted = true;
            }

            private void Update() {
                _components.ForEach(static x => x.OnUpdate());
                _modules?.ForEach(static x => x.OnUpdate());
            }

            private void LateUpdate() {
                if (_recalculationScheduled) {
                    RecalculateLayoutImmediate();

                    _recalculationScheduled = false;
                }
                _components.ForEach(static x => x.OnLateUpdate());
            }

            private void OnDestroy() {
                _components.ForEach(static x => x.DestroyInternal());
                IsDestroyed = true;
            }

            private void OnEnable() {
                if (!_wasActuallyDisabled) {
                    return;
                }

                StateUpdatedEvent?.Invoke(this);
                ScheduleLayoutRecalculationAfterStateChange(true);

                _components.ForEach(static x => x.OnEnable());
                _wasActuallyDisabled = false;
            }

            private void OnDisable() {
                if (gameObject.activeSelf) {
                    return;
                }

                StateUpdatedEvent?.Invoke(this);
                ScheduleLayoutRecalculationAfterStateChange(false);

                _components.ForEach(static x => x.OnDisable());
                _wasActuallyDisabled = true;
            }

            private void OnRectTransformDimensionsChange() {
                // if we look at how the reactive layout system works
                // we'll see that it goes to the root driver and starts recalculation from there.
                // it means that layout controller will be able to modify rect properties
                // which is not allowed to be performed on the OnRectTransformDimensionsChange stack.
                // that's why we schedule recalculation to the end of this frame
                if (!_beingApplied) {
                    _recalculationScheduled = true;
                }
                _components.ForEach(static x => x.OnRectDimensionsChanged());
            }

            #endregion
        }
    }
}