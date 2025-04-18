using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Reactive {
    [PublicAPI]
    public partial class ReactiveComponent : IReactiveComponent, IObservableHost, IEffectBinder, IReactiveModuleBinder {
        #region Factory

        [UsedImplicitly]
        private ReactiveComponent(bool _) { }

        public ReactiveComponent() {
            ConstructAndInit();
        }

        private static ConstructorInfo? _lazyConstructor;
        private static readonly object[] dummyParams = { false };

        public static T Lazy<T>() where T : ReactiveComponent, new() {
            _lazyConstructor ??= typeof(ReactiveComponent).GetConstructor(
                ReflectionUtils.DefaultFlags,
                null,
                new[] { typeof(bool) },
                null
            );
            return (T)_lazyConstructor!.Invoke(dummyParams);
        }

        #endregion

        #region UI Props

        /// <summary>
        /// Gets or sets state of the transform.
        /// </summary>
        public bool Enabled {
            get => Host.Enabled;
            set => Host.Enabled = value;
        }

        /// <summary>
        /// Gets or sets name of the content game object.
        /// </summary>
        public string Name {
            get => Content.name;
            set => Content.name = value;
        }

        #endregion

        #region Observable

        private ObservableHost _observableHost = null!;

        public void AddCallback<T>(string propertyName, Action<T> callback) {
            _observableHost.AddCallback(propertyName, callback);
        }

        public void RemoveCallback<T>(string propertyName, Action<T> callback) {
            _observableHost.RemoveCallback(propertyName, callback);
        }

        public void NotifyPropertyChanged([CallerMemberName] string? name = null) {
            _observableHost.NotifyPropertyChanged(name);
        }

        #endregion

        #region Context

        protected void InsertContextMember(IContextMember member) => Host.InsertContextMember(member);

        protected void ReleaseContextMember(IContextMember member) => Host.ReleaseContextMember(member);

        #endregion

        #region Layout Item

        public ILayoutDriver? LayoutDriver {
            get => Host.LayoutDriver;
            set => Host.LayoutDriver = value;
        }

        public ILayoutModifier? LayoutModifier {
            get => Host.LayoutModifier;
            set => Host.LayoutModifier = value;
        }

        bool ILayoutItem.WithinLayout {
            get => Enabled || WithinLayoutIfDisabled;
            set {
                if (WithinLayoutIfDisabled) return;
                Enabled = value;
            }
        }

        public bool WithinLayoutIfDisabled {
            get => _reactiveHost!.WithinLayoutIfDisabled;
            set => _reactiveHost!.WithinLayoutIfDisabled = value;
        }

        public event Action<ILayoutItem>? ModifierUpdatedEvent {
            add => Host.ModifierUpdatedEvent += value;
            remove => Host.ModifierUpdatedEvent -= value;
        }

        public RectTransform BeginApply() {
            return Host.BeginApply();
        }

        public void EndApply() {
            Host.EndApply();
        }

        bool IEquatable<ILayoutItem>.Equals(ILayoutItem other) {
            return Host.Equals(other);
        }

        protected void RefreshLayout() {
            Host.ScheduleLayoutRecalculation();
        }

        public override bool Equals(object? obj) {
            return Host.Equals(obj);
        }

        public override int GetHashCode() {
            return Host.GetHashCode();
        }

        #endregion

        #region Coroutines

        /// <summary>
        /// Starts a coroutine on the <see cref="ReactiveHost"/> instance.
        /// </summary>
        /// <param name="coroutine">The coroutine to start.</param>
        protected void StartCoroutine(IEnumerator coroutine) {
            _reactiveHost!.StartCoroutine(coroutine);
        }

        /// <summary>
        /// Stops a coroutine on the <see cref="ReactiveHost"/> instance.
        /// </summary>
        /// <param name="coroutine">The coroutine to stop.</param>
        protected void StopCoroutine(IEnumerator coroutine) {
            _reactiveHost!.StopCoroutine(coroutine);
        }

        /// <summary>
        /// Stops all coroutines on the <see cref="ReactiveHost"/> instance.
        /// </summary>
        protected void StopAllCoroutines() {
            _reactiveHost!.StopAllCoroutines();
        }

        #endregion

        #region Effects

        public void BindEffect<T>(INotifyValueChanged<T> value, IEffect<T> effect) {
            Host.BindEffect(value, effect);
        }

        public void UnbindEffect<T>(INotifyValueChanged<T> value, IEffect<T> effect) {
            Host.UnbindEffect(value, effect);
        }

        #endregion

        #region Modules

        private HashSet<IReactiveModule>? _modules;

        public void BindModule(IReactiveModule module) {
            if (LoadModules()) {
                _modules!.Add(module);
            } else {
                Host.BindModule(module);
            }
        }

        public void UnbindModule(IReactiveModule module) {
            if (LoadModules()) {
                _modules!.Remove(module);
            } else {
                Host.UnbindModule(module);
            }
        }

        private bool LoadModules() {
            if (!IsInitialized) {
                _modules = HashSetPool<IReactiveModule>.Get();
            }
            return !IsInitialized;
        }

        private void TransferModules() {
            if (_modules == null) return;
            foreach (var module in _modules) {
                Host.BindModule(module);
            }
            HashSetPool<IReactiveModule>.Release(_modules);
        }

        #endregion

        #region Construct

        public RectTransform ContentTransform => _contentTransform ?? throw new UninitializedComponentException();
        public GameObject Content => _content ?? throw new UninitializedComponentException();

        public bool IsInitialized { get; private set; }
        public bool IsDestroyed { get; private set; }

        protected Canvas? Canvas {
            get {
                if (!_canvas) {
                    _canvas = Content.GetComponentInParent<Canvas>();
                }
                return _canvas;
            }
        }

        private ReactiveHost Host => _reactiveHost ?? throw new UninitializedComponentException();

        private Canvas? _canvas;
        private GameObject? _content;
        private RectTransform? _contentTransform;
        private ReactiveHost? _reactiveHost;

        /// <summary>
        /// Constructs and reparents the component if needed.
        /// </summary>
        public GameObject Use(Transform? parent = null) {
            ValidateExternalInteraction();
            if (!IsInitialized) ConstructAndInit();
            ContentTransform.SetParent(parent, false);
            if (parent == null) LayoutDriver = null;
            return Content;
        }

        private void ConstructAndInit() {
            if (IsInitialized) {
                throw new InvalidOperationException();
            }

            _observableHost = new(this);
            OnInstantiate();

            _content = Construct();
            _content.name = GetType().Name;
            _contentTransform = _content.GetOrAddComponent<RectTransform>();

            _reactiveHost = _content.GetOrAddComponent<ReactiveHost>();
            IsInitialized = true;
            _reactiveHost.AddComponent(this);
            TransferModules();

            OnInitialize();
        }

        private void ValidateExternalInteraction() {
            if (IsDestroyed) throw new InvalidOperationException("Unable to manage the object since it's destroyed");
        }

        protected virtual void Construct(RectTransform rect) { }

        protected virtual GameObject Construct() {
            var go = new GameObject();
            var rect = go.AddComponent<RectTransform>();
            Construct(rect);
            return go;
        }

        #endregion

        #region Destroy

        /// <summary>
        /// Destroys the component.
        /// </summary>
        public void Destroy() {
            Object.Destroy(Content);
            DestroyInternal();
        }

        private void DestroyInternal() {
            IsDestroyed = true;
            OnDestroy();
        }

        #endregion

        #region Events

        protected virtual void OnInstantiate() { }
        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnLateUpdate() { }
        protected virtual void OnStart() { }
        protected virtual void OnDestroy() { }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void OnRectDimensionsChanged() { }
        
        /// <summary>
        /// Called when modifier is updated.
        /// </summary>
        protected virtual void OnModifierUpdated() { }

        /// <summary>
        /// Called when host doesn't have a driver and needs a layout recalculation.
        /// </summary>
        protected virtual void OnRecalculateLayoutSelf() { }

        /// <summary>
        /// Called when layout application is finished.
        /// </summary>
        protected virtual void OnLayoutApply() { }

        #endregion
    }
}