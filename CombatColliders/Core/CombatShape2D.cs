using Godot;
using Godot.Collections;

public abstract partial class CombatShape2D : CollisionShape2D
{
    public int Frame;

    protected abstract void PlaceInFrameBefore();

    protected abstract void PlaceInCurrentFrame();

    protected abstract void PlaceInNextFrame();

    protected abstract void RemoveCurrentCollider();

    public override Array<Dictionary> _GetPropertyList()
    {
        var propList = new Array<Dictionary>();

        if (Frame != 0)
            propList.Add(
                         new Dictionary
                         {
                             { "name", "PlaceInFrameBefore" },
                             { "type", (int)Variant.Type.Callable },
                             { "usage", (int)PropertyUsageFlags.Default },
                             { "hint", (int)PropertyHint.ToolButton },
                             { "hint_string", "Add Collider in Previous Frame" }
                         }
                        );


        propList.AddRange([
            new()
            {
                { "name", "PlaceInCurrentFrame" },
                { "type", (int)Variant.Type.Callable },
                { "usage", (int)PropertyUsageFlags.Default },
                { "hint", (int)PropertyHint.ToolButton },
                { "hint_string", "Add Collider in Current Frame" }
            },
            new()
            {
                { "name", "PlaceInNextFrame" },
                { "type", (int)Variant.Type.Callable },
                { "usage", (int)PropertyUsageFlags.Default },
                { "hint", (int)PropertyHint.ToolButton },
                { "hint_string", "Add Collider in Next Frame" }
            },
            new()
            {
                { "name", "RemoveCurrentCollider" },
                { "type", (int)Variant.Type.Callable },
                { "usage", (int)PropertyUsageFlags.Default },
                { "hint", (int)PropertyHint.ToolButton },
                { "hint_string", "Remove This Collider" }
            },
            new()
            {
                { "name", "Frame" },
                { "type", (int)Variant.Type.Int },
                { "usage", (int)(PropertyUsageFlags.Default | PropertyUsageFlags.ReadOnly) },
                { "hint", (int)PropertyHint.Range },
                { "hint_string", "0,100,1,hide_slider" }
            }
        ]);

        return propList;
    }
}