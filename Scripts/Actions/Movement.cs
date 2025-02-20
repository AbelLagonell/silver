using Godot;

namespace silver.Scripts.Actions;

[GlobalClass, Tool]
public partial class Movement : ActionResource {
    protected Vector2 Speed;

    protected override void PopulateDictionary() {
        RegisterProperty("Speed", Speed);
    }

    /*
     *new() {
                { "name", "_inputString" },
                { "usage", Variant.From(PropertyUsageFlags.Default) },
                { "type", Variant.From(Variant.Type.String) },
                { "hint", Variant.From(PropertyHint.Enum) },
                { "hint_string", string.Join(",", _parent.InputList) }
            }
     *
     */
}