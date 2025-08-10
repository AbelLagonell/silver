using Godot;
using Godot.Collections;

public partial class Frame<[MustBeVariant] T> : Resource
    where T : HurtboxShape2D, new()
{
    private int frame;

    public bool AllDisabled { get; private set; }
    [Export] public Array<T> Shapes { get; private set; }

    public Frame()
    {
        AllDisabled = true;
        Shapes = new Array<T>();
    }

    public void SetDisable(bool isDisabled)
    {
        foreach (var shape in Shapes)
            shape.SetDisabled(isDisabled);
        AllDisabled = isDisabled;
    }

    public T AddShape(Shape2D shape, string name, Color debugColor)
    {
        T collider = new();
        collider.Shape = shape;
        collider.Name = name;
        collider.DebugColor = debugColor;
        collider.SetDisabled(false);

        Shapes.Add(collider);

        return collider;
    }

    public void ResetShapes()
    {
        var temp = Shapes;
        Shapes = [];

        foreach (var hurtboxShape2D in temp)
        {
            hurtboxShape2D.QueueFree();
        }
    }
}