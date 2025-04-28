using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// Represents an animation made of other animations.
/// </summary>
[PublicAPI]
public class SequentialAnimation : ISharedAnimation {
    public SequentialAnimation(Action onStart, IEnumerable<ISharedAnimation> animations) {
        _animations = animations;
        _onStart = onStart;
    }

    public bool IsFinished => _isFinished;

    public event Action? AnimationFinishedEvent;
    
    private readonly IEnumerable<ISharedAnimation> _animations;
    private readonly Action _onStart;
    private bool _isFinished;
    private bool _isPlaying;

    public void FinishToEnd() {
        _isPlaying = false;
        _isFinished = true;
        
        foreach (var animation in _animations) {
            animation.FinishToEnd();
        }
    }

    public void Finish() {
        _isPlaying = false;
        _isFinished = true;
        
        foreach (var animation in _animations) {
            animation.Finish();
        }
    }

    public void Play() {
        _isFinished = false;
        _isPlaying = true;
        _onStart();
    }

    public void OnUpdate() {
        if (!_isPlaying) {
            return;
        }
        
        _isFinished = true;

        foreach (var animation in _animations) {
            animation.OnUpdate();
            
            if (!animation.IsFinished) {
                _isFinished = false;
            }
        }
    }
}