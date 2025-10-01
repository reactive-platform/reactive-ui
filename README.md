## About
Reactive UI is unity library that is made specifically for modding, with performance, convenience and simplicity in mind. It's based on the meta's Yoga layout engine that allows building powerful layouts similar to those you can achieve when using css.
Each `ReactiveComponent` returns a `GameObject` instance, so you can easily wrap existing components and flawlessly integrate with another UI libraries. Read more on: https://reactiveui.beatleader.com/.

## Roadmap
This roadmap covers not just this repo, but rather the whole reactive sdk set. Here it is:
- [x] Release the first version;
- [ ] Refactor names for simplicity, so `ObservableValue<T>` becomes `State<T>`;
- [ ] Rework modal system (Already WIP);
- [ ] Remove components like `ColoredButton` and `ButtonBase` in exchange for `Clickable` and similar wrappers;
- After C# 14 release:
- [ ] Replace `Animate` method with compiler-generated property extensions, so binding states will be as easy as writing `sText = _state`;
- [ ] Add more state tools like `Map(x => x.Property)`;
- [ ] Replace current NotifyPropertyChanged pattern with compiler-generated type-safe events;
- [ ] Add API for layout animations `.AsFlexItem(onApply: x => myAnimatedSize.Value = x.Size)`.

If you feel like you have an interesting idea, let us know by opening an issue!
