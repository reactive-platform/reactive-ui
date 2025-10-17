using UnityEngine;

namespace Reactive.Compiler.Sample;

public class Sample : ReactiveComponent {
    protected override GameObject Construct() {
        var text = Remember("");
        var color = RememberAnimated(Color.blue, 200.ms());

        return new Label {
            sText = text.Map(x => $"Hello, {x}!"),
            sColor = color
        }.Use();
    }
}

public class Sample2 : ReactiveComponent {
    private State<string> _text = null!;

    // Try changing to state_} and {}: it won't compile
    [StateGen(Patterns = ["state_{}", "st{}", "s_{}"])]
    protected override GameObject Construct() {
        _text = Remember("");

        return new Label {
            state_Text = _text.Map(x => $"Hello, {x}!"),
            stColor = _text.Map(_ => Color.blue),
        }.Use();
    }
}