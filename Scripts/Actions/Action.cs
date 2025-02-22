using System.Linq;
using Godot;
using Godot.Collections;


namespace silver.Scripts.Actions;

[GlobalClass, Tool]
public partial class Action : Node2D {
    private ActionManager _parent;

    //Animation things
    private Animation _backAnimation;

    private Animation Animation {
        get => _backAnimation;
        set {
            _backAnimation = value;
            //_backAnimation.SetName(Util.GetFileName(_backAnimation));
        }
    }

    private Sprite2D _sprite;

    private StringName[] _backInputs;

    /// <summary>
    /// What inputs can activate this Action
    /// </summary>
    private StringName[] Inputs {
        get => _backInputs;
        set {
            _backInputs = value;
            NotifyPropertyListChanged();
        }
    }

    private bool _isMultiKey;


    /// <summary>
    /// When does the activation of the action happen<br/>
    /// OnPress = 0
    /// OnRelease = 1
    /// </summary>
    private int _onActivation = 0;

    /// <summary>
    /// Does the input need to be held?<br/>
    /// Press = 0
    /// Held = 1
    /// </summary>
    private int _inputType = 0;

    public override void _EnterTree() {
        _parent ??= GetNode<ActionManager>("..");
    }

    public override void _PhysicsProcess(double delta) {
        if (_inputType == 0) { }
    }

    public bool InputIsPressedOR() {
        return true;
    }

    public override Array<Dictionary> _GetPropertyList() {
        Array<Dictionary> propList = [
            new() {
                { "name", "Action" },
                { "usage", Variant.From(PropertyUsageFlags.Category) },
                { "type", Variant.From(Variant.Type.String) }
            },


            new() {
                { "name", "Animation" },
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
                { "name", "Input" },
                { "usage", Variant.From(PropertyUsageFlags.Subgroup) },
                { "type", Variant.From(Variant.Type.String) }
            },


            new() {
                { "name", "Inputs" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.Array) },
                { "hint", Variant.From(PropertyHint.TypeString) }, {
                    "hint_string",
                    $"{Variant.Type.String:D}/{PropertyHint.Enum:D}:{string.Join(",", _parent.InputList)}"
                }
            }
        ];

        if (Inputs.Length > 0) {
            propList.Add(new Dictionary {
                { "name", "_isMultiKey" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.Bool) },
                { "hint", Variant.From(PropertyHint.None) }
            });

            propList.Add(new Dictionary() {
                { "name", "_onActivation" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.Int) },
                { "hint", Variant.From(PropertyHint.Enum) },
                { "hint_string", "OnPress, OnRelease" }
            });
        }

        return propList;
    }
}