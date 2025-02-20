using Godot;

namespace silver.Scripts.Actions;

[GlobalClass, Tool]
public partial class Movement : ActionResource {
    [Export] private Vector2 _speed;

    protected override void PopulateDictionary() {
        RegisterProperty("Speed", _speed);
    }
}