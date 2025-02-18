using Godot;
using Godot.Collections;

[GlobalClass, Tool]
public partial class ActionHolder : Node2D {
    private AnimationPlayer _animationPlayer;
    private AnimationLibrary _library = null;
    private string _defaultAnimation;

    private AnimationLibrary AnimationLibrary {
        get => _library;
        set {
            _library = value;
            NotifyPropertyListChanged();
        }
    }

    public override void _Ready() {
        _library.SetName(Util.GetFileName(_library));
        _animationPlayer.Play($"{_library.GetName()}/{_defaultAnimation}");
    }

    public void PlayAnimation(Animation anim) {
        _animationPlayer.Play($"{_library.GetName()}/{anim.GetName()}");
    }

    public override Array<Dictionary> _GetPropertyList() {
        Array<Dictionary> propList = new Array<Dictionary> {
            new Dictionary() {
                { "name", "Action Holder" },
                { "usage", Variant.From(PropertyUsageFlags.Category) },
                { "type", Variant.From(Variant.Type.String) }
            },
            new Dictionary() {
                { "name", "_animationPlayer" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.Object) },
                { "hint", Variant.From(PropertyHint.NodeType) },
                { "hint_string", "AnimationPlayer" }
            },
            new Dictionary() {
                { "name", "AnimationLibrary" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.Object) },
                { "hint", Variant.From(PropertyHint.ResourceType) },
                { "hint_string", "AnimationLibrary" }
            }
        };

        if (_library != null) {
            string animationList = string.Join(",", _library.GetAnimationList());

            propList.Add(new Dictionary() {
                { "name", "_defaultAnimation" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.String) }, // Changed to String since we're storing animation name
                { "hint", Variant.From(PropertyHint.Enum) },
                { "hint_string", animationList }
            });
        }


        return propList;
    }
}