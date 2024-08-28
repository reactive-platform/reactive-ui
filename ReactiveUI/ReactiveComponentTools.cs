namespace Reactive;

public partial class ReactiveComponentBase {
    protected static ObservableValue<TValue> Remember<TValue>(TValue initialValue) {
        return new ObservableValue<TValue>(initialValue);
    }
}