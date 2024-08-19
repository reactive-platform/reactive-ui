namespace Reactive {
    public interface IAnimationTimeline<in T> {
        void Evaluate(float value, T target);
    }
}