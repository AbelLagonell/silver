#if TOOLS
using Godot;
using System;

[Tool]
public partial class CombatCollidersPlugin : EditorPlugin
{
    private ColliderInspectorPlugin _colliderInspectorPlugin;

    public override void _EnterTree()
    {
        _colliderInspectorPlugin = new ColliderInspectorPlugin();
        AddInspectorPlugin(_colliderInspectorPlugin);
    }

    public override void _ExitTree()
    {
        if (!IsInstanceValid(_colliderInspectorPlugin)) return;
        RemoveInspectorPlugin(_colliderInspectorPlugin);
        _colliderInspectorPlugin = null;
    }
}
#endif