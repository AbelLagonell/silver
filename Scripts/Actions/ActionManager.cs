using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Godot;
using Godot.Collections;

namespace silver.Scripts.Actions;

[GlobalClass, Tool]
public partial class ActionManager : Node2D {
    private AnimationPlayer _animationPlayer;
    private AnimationLibrary _library;
    private string _defaultAnimation;
    public string[] InputList;


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

    public ActionResource[] UseAction(string action) {
        var nodes = GetChildren();
        foreach (Node node in nodes) {
            if (node is Action actionNode) {
                if (actionNode.GetName() == action) return actionNode.getAttributes();
            }
        }

        GD.PushError($"Action {action} not found");
        return null;
    }


    public override Array<Dictionary> _GetPropertyList() {
        Array<Dictionary> propList = new Array<Dictionary> {
            new() {
                { "name", "Action Manager" },
                { "usage", Variant.From(PropertyUsageFlags.Category) },
                { "type", Variant.From(Variant.Type.String) }
            },

            new() {
                { "name", "InputList" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.Array) },
                { "hint", Variant.From(PropertyHint.ArrayType) },
                { "hint_string", "string" }
            },

            new() {
                { "name", "_animationPlayer" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.Object) },
                { "hint", Variant.From(PropertyHint.NodeType) },
                { "hint_string", "AnimationPlayer" }
            },

            new() {
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
                { "type", Variant.From(Variant.Type.String) },
                { "hint", Variant.From(PropertyHint.Enum) },
                { "hint_string", animationList }
            });
        }


        return propList;
    }
}