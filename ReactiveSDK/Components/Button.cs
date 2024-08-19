using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.EventSystems;

namespace Reactive.Components {
    [PublicAPI]
    public class ButtonBase : DrivingReactiveComponentBase {
        #region UI Properties

        public bool Interactable {
            get => _interactable;
            set {
                if (value == _interactable) return;
                _interactable = value;
                SetStateEnabled(NonInteractableState, !value);
                OnInteractableChange(value);
                NotifyPropertyChanged();
            }
        }

        public bool Latching {
            get => _latching;
            set {
                _latching = value;
                NotifyPropertyChanged();
            }
        }

        public bool Active => _active && Interactable;

        public Action? ClickEvent { get; set; }

        private bool _interactable;
        private bool _latching;
        private bool _active;

        #endregion

        #region States

        public static ComponentState DisabledState = "disabled";
        public static ComponentState NonInteractableState = "non-interactable";

        protected override IEnumerable<ComponentState> ExtraStates { get; } = new[] {
            DisabledState,
            NonInteractableState
        };

        #endregion

        #region Button

        /// <summary>
        /// Emulates UI button click.
        /// </summary>
        /// <param name="state">Determines the toggle state. Valid only if <c>Sticky</c> is turned on</param>
        /// <param name="notifyListeners">Determines should event be invoked or not</param>
        /// <param name="force">Determines should the state be changed or not even if it is the same</param>
        public void Click(bool state = false, bool notifyListeners = false, bool force = false) {
            if (!_interactable) return;
            if (_latching) {
                if (!force && state == _active) return;
                _active = state;
            } else {
                _active = true;
            }
            HandleButtonClick(notifyListeners);
            if (!_latching) {
                _active = false;
            }
        }

        private void HandleButtonClick(bool notifyListeners) {
            OnButtonStateChange(Active);
            if (notifyListeners) {
                NotifyPropertyChanged(nameof(Active));
                ClickEvent?.Invoke();
            }
        }

        protected virtual void OnButtonStateChange(bool state) { }
        protected virtual void OnInteractableChange(bool interactable) { }

        #endregion

        #region Callbacks

        protected override void OnPointerDown(PointerEventData data) {
            _active = !_latching || !_active;
            HandleButtonClick(true);
        }

        protected override void OnPointerUp(PointerEventData data) {
            if (!_latching) {
                _active = false;
            }
        }

        #endregion
    }
}