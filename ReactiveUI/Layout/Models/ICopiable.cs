namespace Reactive {
    public interface ICopiable<T> {
        void CopyFrom(T mod);
        T CreateCopy();
    }
}