using Reactive;
using UnityEngine;

public class Button : ReactiveComponent, ILayoutDriver {
    private Layout _layout = null!;
    
    protected override GameObject Construct() {
        return new Layout {
            Children = {
                new Layout()
            }
        }.Bind(ref _layout).Use();
    }

    public ILayoutController? LayoutController { get; set; }
    public void AppendChild(ILayoutItem comp) => _layout.AppendChild(comp);
    public void TruncateChild(ILayoutItem comp) => _layout.TruncateChild(comp);
    public void RecalculateLayout() => _layout.RecalculateLayout();
}