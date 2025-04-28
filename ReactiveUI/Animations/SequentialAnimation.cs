using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// Represents an animation made of other animations.
/// </summary>
[PublicAPI]
public class SequentialAnimation : ISharedAnimation {
    public SequentialAnimation(Action onStart, IEnumerable<IAnimation> animations) {
        _animations = animations;
        _onStart = onStart;
    }

    public bool IsFinished => _isFinished;

    public event Action? AnimationFinishedEvent;
    
    private readonly IEnumerable<IAnimation> _animations;
    private readonly Action _onStart;
    private bool _isFinished;

    public void FinishToEnd() {
        foreach (var animation in _animations) {
            animation.FinishToEnd();
        }
    }

    public void Finish() {
        foreach (var animation in _animations) {
            animation.Finish();
        }
    }

    public void Play() {
        _isFinished = false;
        _onStart();
    }

    public void OnUpdate() {
        _isFinished = true;

        foreach (var animation in _animations) {
            if (!animation.IsFinished) {
                _isFinished = false;
                break;
            }
        }
    }
}