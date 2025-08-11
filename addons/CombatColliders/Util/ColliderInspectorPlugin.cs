using Godot;
using System;

public partial class ColliderInspectorPlugin : EditorInspectorPlugin
{
    private GodotObject _object;

    private StringName[] _buttonsBefore =
    [
        //Combat Collider Proper
        "AddCollisionCurrent",
        "AddCollisionNext",
        "ResetCurrentFrame",
        "RemoveCurrentFrame",
        //Collider Specific
        "PlaceInFrameBefore",
        "PlaceInCurrentFrame",
        "PlaceInNextFrame",
    ];
    
    private StringName[] _buttonsAfter =
    [
        //Combat Collider Proper
        "ResetAllFrames",
        "RemoveAllFrames",
        //Collider Specific
        "RemoveCurrentCollider"
    ];

    public override bool _CanHandle(GodotObject @object)
    {
        if (@object is not (HurtboxSet or HitboxSet or HurtboxShape2D or HitboxShape2D)) return false;
        _object = @object;
        return true;
    }

    public override void _ParseBegin(GodotObject @object)
    {
        foreach (var methodName in _buttonsBefore)
        {
            if (!@object.HasMethod(methodName)) continue;
            var buttonName = "";
            switch (methodName)
            {
                case "AddCollisionCurrent":
                    buttonName = "Add Collider to Current Frame";
                    break;
                case "AddCollisionNext":
                    buttonName = "Add Collider to Next Frame";
                    break;
                case "ResetCurrentFrame":
                    buttonName = "Reset Current Frame";
                    break;
                case "RemoveCurrentFrame":
                    buttonName = "Remove Current Frame";
                    break;
                case "PlaceInFrameBefore":
                    buttonName = "Place In Frame Before";
                    break;
                case "PlaceInCurrentFrame":
                    buttonName = "Place In Current Frame";
                    break;
                case "PlaceInNextFrame":
                    buttonName = "Place In Next Frame";
                    break;
            }
        
            var btn = new Button();
            btn.Text = buttonName;
            btn.Pressed += () =>
            {
                if (_object.HasMethod(methodName))
                    _object.Call(methodName);
                else
                    GD.PrintErr(@object);
            };
        
            AddCustomControl(btn);
        }
    }

    public override void _ParseEnd(GodotObject @object)
    {
        foreach (var methodName in _buttonsAfter)
        {
            if (!@object.HasMethod(methodName)) continue;
            var buttonName = "";
            switch (methodName)
            {
                case "ResetAllFrames":
                    buttonName = "Reset All Frames";
                    break;
                case "RemoveAllFrames":
                    buttonName = "Remove All Frames";
                    break;
                case "RemoveCurrentCollider":
                    buttonName = "Remove Current Collider";
                    break;
            }
        
            var btn = new Button();
            btn.Text = buttonName;
            var style = new StyleBoxFlat();
            style.BgColor = new Color(0.5f, 0, 0, 0.7f);
            btn.AddThemeStyleboxOverride("normal", style);
            btn.Pressed += () =>
            {
                if (_object.HasMethod(methodName))
                    _object.CallDeferred(methodName);
                else
                    GD.PrintErr(@object);
            };
        
            AddCustomControl(btn);
        }
    }
}