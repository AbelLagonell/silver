using Godot;
using Godot.Collections;


namespace silver.Scripts.Actions;

[GlobalClass, Tool]
public partial class Action : Node2D {
    private ActionManager _parent;

    private Animation _backAnimation;

    private Animation _animation {
        get => _backAnimation;
        set {
            _backAnimation = value;
            _backAnimation.SetName(Util.GetFileName(_backAnimation));
        }
    }

    private Sprite2D _sprite;
    private ActionResource[] _attributes = System.Array.Empty<ActionResource>();


    public override void _Ready() {
        _parent ??= GetNode<ActionManager>("..");
    }

    public string getAnimation() {
        return _animation.GetName();
    }

    public ActionResource[] getAttributes() {
        return _attributes;
    }

    public override Array<Dictionary> _GetPropertyList() {
        var propList = new Array<Dictionary>() {
            new() {
                { "name", "Action" },
                { "usage", Variant.From(PropertyUsageFlags.Category) },
                { "type", Variant.From(Variant.Type.String) }
            },


            new() {
                { "name", "_animation" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.Object) },
                { "hint", Variant.From(PropertyHint.ResourceType) },
                { "hint_string", "Animation" }
            },

            new() {
                { "name", "_sprite" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.Object) },
                { "hint", Variant.From(PropertyHint.NodeType) },
                { "hint_string", "Sprite2D" }
            },

            new() {
                { "name", "_attributes" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.Array) },
                { "hint", Variant.From(PropertyHint.ArrayType) },
                { "hint_string", $"{Variant.Type.Object:D}/{PropertyHint.ResourceType:D}:ActionResource" }
            }
        };
        return propList;
    }
}