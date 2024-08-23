namespace Reactive;

public interface ITransitionBuilder {
    ComponentState BuildingForState { get; }

    IAnimation Build();
}