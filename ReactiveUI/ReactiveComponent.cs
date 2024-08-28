using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Reactive {
    [PublicAPI]
    public abstract class ReactiveComponent : ReactiveComponentBase {
        #region Overrides

        protected sealed override void ConstructInternal() {
            base.ConstructInternal();
        }

        protected sealed override void OnModifierUpdatedInternal() {
            base.OnModifierUpdatedInternal();
        }

        protected sealed override void OnLateUpdateInternal() {
            base.OnLateUpdateInternal();
        }

        #endregion
    }

    [PublicAPI]
    public abstract partial class ReactiveComponentBase : ILayoutItem, IObservableHost, IReactiveComponent {
        #region Factory

        [UsedImplicitly]
        private ReactiveComponentBase(bool _) { }

        protected ReactiveComponentBase() {
            ConstructAndInit();
        }

        private static ConstructorInfo? _lazyConstructor;
        private static readonly object[] dummyParams = { false };

        public static T Lazy<T>() where T : ReactiveComponent, new() {
            _lazyConstructor ??= typeof(ReactiveComponentBase).GetConstructor(
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

        float? ILayoutItem.DesiredHeight => Host.DesiredHeight;
        float? ILayoutItem.DesiredWidth => Host.DesiredWidth;

        public bool WithinLayoutIfDisabled {
            get => _reactiveHost!.WithinLayoutIfDisabled;
            set => _reactiveHost!.WithinLayoutIfDisabled = value;
        }

        protected virtual float? DesiredHeight => null;
        protected virtual float? DesiredWidth => null;

        public event Action<ILayoutItem>? ModifierUpdatedEvent {
            add => Host.ModifierUpdatedEvent += value;
            remove => Host.ModifierUpdatedEvent -= value;
        }

        bool IEquatable<ILayoutItem>.Equals(ILayoutItem other) => ((ILayoutItem)Host).Equals(other);

        void ILayoutItem.ApplyTransforms(Action<RectTransform> applicator) => Host.ApplyTransforms(applicator);

        protected void RefreshLayout() => Host.RefreshLayout();

        protected virtual void OnModifierUpdatedInternal() { }

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
        /// Constructs and reparents the component if needed
        /// </summary>
        public GameObject Use(Transform? parent = null) {
            ValidateExternalInteraction();
            if (!IsInitialized) ConstructAndInit();
            ContentTransform.SetParent(parent, false);
            if (parent == null) LayoutDriver = null;
            return Content;
        }

        private void ConstructAndInit() {
            _observableHost = new(this);
            ConstructInternal();
            OnInitialize();
        }

        protected virtual void ConstructInternal() {
            if (IsInitialized) throw new InvalidOperationException();
            OnInstantiate();

            _content = Construct();
            _content.name = GetType().Name;
            _contentTransform = _content.GetOrAddComponent<RectTransform>();

            _reactiveHost = _content.GetOrAddComponent<ReactiveHost>();
            IsInitialized = true;
            _reactiveHost.AddComponent(this);
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
        /// Destroys the component
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

        protected virtual void OnLateUpdateInternal() {
            OnLateUpdate();
        }

        protected virtual void OnInstantiate() { }
        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnLateUpdate() { }
        protected virtual void OnStart() { }
        protected virtual void OnDestroy() { }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void OnRectDimensionsChanged() { }
        protected virtual void OnLayoutRefresh() { }
        protected virtual void OnLayoutApply() { }

        #endregion
    }
}