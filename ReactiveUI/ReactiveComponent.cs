﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Reactive {
    [PublicAPI]
    public partial class ReactiveComponent : IReactiveComponent, IObservableHost, IReactiveModuleBinder {
        #region Factory

        [UsedImplicitly]
        private ReactiveComponent(int _) { }

        protected ReactiveComponent(bool constructImmediate) {
            if (constructImmediate) {
                ConstructAndInit();
            }
        }

        public ReactiveComponent() {
            ConstructAndInit();
        }

        private static ConstructorInfo? _lazyConstructor;
        private static readonly object[] dummyParams = { 0 };

        public static T LazyComponent<T>() where T : ReactiveComponent, new() {
            _lazyConstructor ??= typeof(ReactiveComponent).GetConstructor(
                ReflectionUtils.DefaultFlags,
                null,
                new[] { typeof(int) },
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

        public event Action<ILayoutItem>? StateUpdatedEvent {
            add => Host.StateUpdatedEvent += value;
            remove => Host.StateUpdatedEvent -= value;
        }

        public int GetLayoutItemHashCode() {
            return Host.GetLayoutItemHashCode();
        }

        public bool EqualsToLayoutItem(ILayoutItem item) {
            return Host.EqualsToLayoutItem(item);
        }

        public RectTransform BeginApply() {
            return Host.BeginApply();
        }

        public void EndApply() {
            Host.EndApply();
        }

        void ILayoutRecalculationSource.RecalculateLayoutImmediate() {
            Host.RecalculateLayoutImmediate();
        }

        void ILayoutRecalculationSource.ScheduleLayoutRecalculation() {
            Host.ScheduleLayoutRecalculation();
        }

        protected void RecalculateLayoutImmediate() {
            Host.RecalculateLayoutImmediate();
        }

        protected void ScheduleLayoutRecalculation() {
            Host.ScheduleLayoutRecalculation();
        }

        /// <summary>
        /// Exposes the specified component as the layout-first component.
        /// This call is required in case you want to preserve leaf layout.
        /// The component won't be overridden until you call this method with null.
        /// </summary>
        /// <param name="component">A component to expose.</param>
        protected void ExposeLayoutFirstComponent(ReactiveComponent? component) {
            Host.ExposeLayoutFirstComponent(component);
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

        #region Modules

        IReadOnlyCollection<IReactiveModule> IReactiveModuleBinder.Modules => Host.Modules;
        
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
            if (!IsInitialized && _modules == null) {
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
            if (IsDestroyed) {
                throw new InvalidOperationException("Unable to manage the object since it's destroyed");
            }

            if (!IsInitialized) {
                ConstructAndInit();
            }

            LayoutDriver = null;
            ContentTransform.SetParent(parent, false);

            return Content;
        }

        protected void ConstructAndInit() {
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