using System;
using UnityEngine;

namespace Reactive;

internal class Image : ReactiveComponent {
    public Color Color { get; set; }
}

internal class Button : ReactiveComponent {
    public Action? OnClick { get; set; }
}

internal class Modal : ReactiveComponent {
    public ISharedAnimation? OpenAnimation { get; set; }
    public ISharedAnimation? CloseAnimation { get; set; }
}

internal class Sample : ReactiveComponent {
    protected override GameObject Construct() {
        var color = RememberAnimated(Color.red, 200.ms(), AnimationCurve.EaseInOut);
        var size = RememberAnimated(10f, 400.ms());
        var state = Remember(false);

        // Unowned state
        var modalScaleX = Animated(1f, 350.ms(), AnimationCurve.EaseInOut);
        var modalScaleY = Animated(1f, 300.ms(), AnimationCurve.Exponential);

        return new Layout {
            Children = {
                // Animations with shared control
                new Modal {
                    OpenAnimation = Animation(
                        () => {
                            modalScaleX.Value = 1.2f;
                            modalScaleY.Value = 1.2f;
                        },
                        modalScaleX,
                        modalScaleY
                    ),
                    
                    CloseAnimation = Animation(
                        () => {
                            modalScaleX.Value = 1f;
                            modalScaleY.Value = 1f;
                        },
                        modalScaleX,
                        modalScaleY
                    ),
                },

                // Simple animation via local values
                new Button {
                    OnClick = () => {
                        size.Value = state ? 10f : 20f;
                        color.Value = state ? Color.red : Color.green;

                        state.Value = !state;
                    }
                },

                new Image()
                    .AsFlexItem(out var modifier, flex: size)
                    .Animate(size, () => modifier.Flex = size)
                    .Animate(color, x => x.Color)
            }
        }.Use();
    }
}