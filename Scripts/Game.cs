using Godot;

namespace silver.Scripts;

public partial class Game: Node2D
{
    [Export] public Resource MappingResource;

    public override void _Ready() {
        base._Ready();
    }
}