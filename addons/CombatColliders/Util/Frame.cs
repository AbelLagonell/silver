using System.Collections.Generic;
using Godot;

struct Frame<[MustBeVariant] T> where T : HurtboxShape2D
{
    public bool AllDisabled { get; private set; }
    public System.Collections.Generic.List<T> Shapes { get; private set; }

    public Frame()
    {
        AllDisabled = true;
        Shapes = new List<T>();
    }

    public void SetDisable(bool isDisabled)
    {
        foreach (var shape in Shapes)
            shape.SetDisabled(isDisabled);
        AllDisabled = isDisabled;
    }

    // public T AddShape(Shape3D shape)
    // {
    // }
}