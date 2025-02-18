using System.Linq;
using Godot;


[GlobalClass]
public partial class Action : Node2D {
    [Export] protected ActionHolder Parent;
    [Export] protected Animation Animation;
    [Export] protected Sprite2D Sprite;

    public override void _Ready() {
        Parent ??= GetNode<ActionHolder>("..");
        Animation.SetName(Util.GetFileName(Animation));
    }
}