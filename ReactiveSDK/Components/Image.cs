using UnityEngine;

namespace Reactive.Components {
    internal class Image : DrivingReactiveComponent {
        public Sprite? Sprite {
            get => _image.sprite;
            set {
                _image.sprite = value;
                NotifyPropertyChanged();
            }
        }

        public Color Color {
            get => _image.color;
            set {
                _image.color = value;
                NotifyPropertyChanged();
            }
        }

        public Material? Material {
            get => _image.material;
            set {
                _image.material = value;
                NotifyPropertyChanged();
            }
        }

        public bool PreserveAspect {
            get => _image.preserveAspect;
            set {
                _image.preserveAspect = value;
                NotifyPropertyChanged();
            }
        }

        public UnityEngine.UI.Image.Type ImageType {
            get => _image.type;
            set {
                _image.type = value;
                NotifyPropertyChanged();
            }
        }

        public UnityEngine.UI.Image.FillMethod FillMethod {
            get => _image.fillMethod;
            set {
                _image.fillMethod = value;
                NotifyPropertyChanged();
            }
        }

        public float FillAmount {
            get => _image.fillAmount;
            set {
                _image.fillAmount = value;
                NotifyPropertyChanged();
            }
        }

        public float PixelsPerUnit {
            get => _image.pixelsPerUnitMultiplier;
            set {
                ImageType = UnityEngine.UI.Image.Type.Sliced;
                _image.pixelsPerUnitMultiplier = value;
                NotifyPropertyChanged();
            }
        }

        public bool RaycastTarget {
            get => _image.raycastTarget;
            set => _image.raycastTarget = value;
        }

        private UnityEngine.UI.Image _image = null!;

        protected override void Construct(RectTransform rect) {
            _image = rect.gameObject.AddComponent<UnityEngine.UI.Image>();
        }
    }
}