using System;
using Godot;
using Godot.Collections;

[Tool]
public abstract partial class CombatCollider2D<[MustBeVariant] T> : Area2D
    where T : HurtboxShape2D, new()
{
    public enum ShapeType
    {
        Circle,
        Rectangle,
        Capsule
    }

    protected int CurrentFrame = 0;
    protected Color _debugColor;

    protected Dictionary<int, Array<NodePath>> Frames = new();

    private int ViewFrame
    {
        get => _viewFrame;
        set
        {
            _viewFrame = value;
            NotifyPropertyListChanged();
        }
    }

    private int _viewFrame = 0;

    private Array<T> _viewArray = null;

    //----------------
    //---Shape Type---
    //----------------

    private ShapeType Shape
    {
        get => _shape;
        set
        {
            _shape = value;
            Shape2D = value switch
            {
                ShapeType.Circle    => new CircleShape2D(),
                ShapeType.Rectangle => new RectangleShape2D(),
                ShapeType.Capsule   => new CapsuleShape2D(),
                _                   => Shape2D
            };
        }
    }

    private ShapeType _shape = ShapeType.Capsule;
    protected Shape2D Shape2D = new CapsuleShape2D();

    //----------------------
    //---Collision Layers---
    //----------------------
    private bool _isEnemy = false;

    protected bool IsEnemy
    {
        get => _isEnemy;
        set
        {
            _isEnemy = value;
            ChangeCollision();
        }
    }


    private int _enemyHurtboxLayer
    {
        get => EnemyHurtboxLayer;
        set => EnemyHurtboxLayer = value;
    }

    protected static int EnemyHurtboxLayer = 3;

    private int _playerHurtboxLayer
    {
        get => PlayerHurtboxLayer;
        set => PlayerHurtboxLayer = value;
    }

    protected static int EnemyHitboxLayer = 7;

    private int _enemyHitboxLayer
    {
        get => EnemyHitboxLayer;
        set => EnemyHitboxLayer = value;
    }

    protected static int PlayerHitboxLayer = 8;

    private int _playerHitboxLayer
    {
        get => PlayerHitboxLayer;
        set => PlayerHitboxLayer = value;
    }

    protected static int PlayerHurtboxLayer = 4;

    //----------------------
    //----Debug Features----
    //----------------------

    private bool _alwaysVisible;
    private bool _alwaysActive;

    //-----------------
    //----Functions----
    //----------------- 

    public override void _Ready()
    {
        if (!Engine.IsEditorHint()) ResetDictionary();
        SetCollisionLayerValue(1, false);
        SetCollisionMaskValue(1, false);
        ChangeCollision();

        End();
        GD.Print($"READY {Name}");
    }

    private void ResetDictionary()
    {
        foreach (var child in GetChildren())
        {
            if (child is not HurtboxShape2D hurt) continue;
            if (!Frames.ContainsKey(hurt.Frame))
                Frames.Add(hurt.Frame, new Array<NodePath>());
            Frames[hurt.Frame].Add(GetPathTo(hurt));
        }
    }

    protected void Start()
    {
        CurrentFrame = 0;
        ActivateFrame(CurrentFrame);
    }

    protected void NextFrame()
    {
        ActivateFrame(++CurrentFrame);
    }

    protected void End()
    {
        DeactivateAllFrames();
    }

    //-------------
    //---Utility---
    //-------------
    protected abstract void ChangeCollision();

    /// <summary>
    /// Sets the acting frame in this set to the value inputted
    /// </summary>
    /// <param name="actingFrame">The frame that needs activation</param>
    /// <param name="deactivate">Whether the frame is to be on or off</param>
    private void SetFrame(int actingFrame = 0, bool deactivate = false, bool visible = false)
    {
        foreach (var nodePath in Frames[actingFrame])
        {
            T node = GetNode<T>(nodePath);
            node.SetDisabled(deactivate);
            node.SetVisible(visible);
        }
    }

    /// <summary>
    /// Deactivates All frames within this set
    /// </summary>
    private void DeactivateAllFrames()
    {
        foreach (var frame in Frames.Keys)
            SetFrame(frame, deactivate: !_alwaysActive, visible: _alwaysVisible || _alwaysActive);
    }

    private void ActivateFrame(int actingFrame)
    {
        foreach (var frame in Frames.Keys)
            SetFrame(frame,
                  (frame != actingFrame) && !_alwaysActive,
                  (frame == actingFrame) || _alwaysVisible || _alwaysActive);
    }


    protected virtual void AddCollisionToFrame(int actingFrame)
    {
        if (actingFrame < 0)
            throw new ArgumentException("Acting Frame is less than zero");
        if (!Frames.ContainsKey(actingFrame))
            Frames.Add(actingFrame, new Array<NodePath>());

        CurrentFrame = actingFrame;
        NotifyPropertyListChanged();
    }

    protected T AddShape(Shape2D shape, string name, Color debugColor)
    {
        T collider = new();
        collider.Frame = CurrentFrame;
        collider.Shape = shape;
        collider.Name = name;
        collider.DebugColor = debugColor;
        collider.SetDisabled(false);

        return collider;
    }


    private Callable AddCollisionToCurrent()
    {
        AddCollisionToFrame(CurrentFrame);
        return default;
    }

    private Callable AddCollisionNext()
    {
        AddCollisionToFrame(++CurrentFrame);
        return default;
    }

    private Callable AddEmptyFrame()
    {
        Frames.Add(++CurrentFrame, new Array<NodePath>());
        return default;
    }

    private Callable ResetCurrentFrame()
    {
        if (!Frames.TryGetValue(CurrentFrame, out var actingFrame)) return default;
        foreach (var nodePath in actingFrame)
            GetNodeOrNull(nodePath)?.QueueFree();

        actingFrame.Clear();
        NotifyPropertyListChanged();
        return default;
    }

    private Callable RemoveCurrentFrame()
    {
        ResetCurrentFrame();
        Frames.Remove(CurrentFrame);
        if (CurrentFrame > 0) CurrentFrame--;
        NotifyPropertyListChanged();
        return default;
    }

    private Callable ResetAllFrames()
    {
        CurrentFrame = 0;
        for (int i = 0; i < Frames.Count; i++)
        {
            ResetCurrentFrame();
            CurrentFrame++;
        }

        CurrentFrame = 0;
        NotifyPropertyListChanged();
        return default;
    }

    private Callable RemoveAllFrames()
    {
        ResetAllFrames();
        Frames.Clear();
        NotifyPropertyListChanged();
        return default;
    }

    private Callable Test()
    {
        GD.Print($"Frames Dictionary: {Frames}\nFrame Keys: {Frames.Keys}");
        return default;
    }

    public override Array<Dictionary> _GetPropertyList()
    {
        var propList = new Array<Dictionary>();

        propList.AddRange([
            new()
            {
                { "name", "Test" },
                { "type", (int)Variant.Type.Callable },
                { "usage", (int)PropertyUsageFlags.Default },
                { "hint", (int)PropertyHint.ToolButton },
                { "hint_string", "Print Frame" }
            },
            new()
            {
                { "name", "AddCollisionToCurrent" },
                { "type", (int)Variant.Type.Callable },
                { "usage", (int)PropertyUsageFlags.Default },
                { "hint", (int)PropertyHint.ToolButton },
                { "hint_string", "Add Collider To Current Frame" }
            },
            new()
            {
                { "name", "AddCollisionNext" },
                { "type", (int)Variant.Type.Callable },
                { "usage", (int)PropertyUsageFlags.Default },
                { "hint", (int)PropertyHint.ToolButton },
                { "hint_string", "Add Collider To Next Frame" }
            },
            new()
            {
                { "name", "AddEmptyFrame" },
                { "type", (int)Variant.Type.Callable },
                { "usage", (int)PropertyUsageFlags.Default },
                { "hint", (int)PropertyHint.ToolButton },
                { "hint_string", "Add an Empty Frame" }
            },
            new()
            {
                { "name", "ResetCurrentFrame" },
                { "type", (int)Variant.Type.Callable },
                { "usage", (int)PropertyUsageFlags.Default },
                { "hint", (int)PropertyHint.ToolButton },
                { "hint_string", "Reset Current Frame" }
            },
            new()
            {
                { "name", "RemoveCurrentFrame" },
                { "type", (int)Variant.Type.Callable },
                { "usage", (int)PropertyUsageFlags.Default },
                { "hint", (int)PropertyHint.ToolButton },
                { "hint_string", "Remove Current Frame" }
            },
            new()
            {
                { "name", "Settings" },
                { "type", (int)Variant.Type.String },
                { "usage", (int)PropertyUsageFlags.Group },
            },
            new()
            {
                { "name", "Shape" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.Enum },
                { "hint_string", "Circle, Rectangle, Capsule" },
            },
            new()
            {
                { "name", "_debugColor" },
                { "type", (int)Variant.Type.Color },
                { "usage", (int)PropertyUsageFlags.Default },
            },
            new()
            {
                { "name", "CurrentFrame" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.Range },
                { "hint_string", $"0,{(Frames.Count != 0 ? Frames.Count + 1 : 1)},or_greater,hide_slider" },
            },
            new()
            {
                { "name", "_isEnemy" },
                { "type", (int)Variant.Type.Bool },
            },
            new()
            {
                { "name", "Collision Layers" },
                { "usage", (int)(PropertyUsageFlags.Group | PropertyUsageFlags.Default) }
            },
            new()
            {
                { "name", "_enemyHurtboxLayer" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.Range },
                { "hint_string", "1,32,1" },
            },
            new()
            {
                { "name", "_playerHurtboxLayer" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.Range },
                { "hint_string", "1,32,1" },
            },
            new()
            {
                { "name", "_enemyHitboxLayer" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.Range },
                { "hint_string", "1,32,1" },
            },
            new()
            {
                { "name", "_playerHitboxLayer" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.Range },
                { "hint_string", "1,32,1" },
            },
            new()
            {
                { "name", "Debug" },
                { "type", (int)Variant.Type.String },
                { "usage", (int)PropertyUsageFlags.Group }
            },
            new()
            {
                { "name", "_alwaysVisible" },
                { "type", (int)Variant.Type.Bool },
            },
            new()
            {
                { "name", "_alwaysActive" },
                { "type", (int)Variant.Type.Bool },
            },
            new()
            {
                { "name", "ViewFrame" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.Range },
                { "hint_string", $"0,{(Frames.Count != 0 ? Frames.Count - 1 : 0)},1" },
            },
        ]);

        if (Frames.TryGetValue(_viewFrame, out var viewList))
        {
            _viewArray = new Array<T>();
            foreach (var nodePath in viewList)
                _viewArray.Add(GetNode<T>(nodePath));
            propList.Add(new()
            {
                { "name", "_viewArray" },
                { "type", (int)Variant.Type.Array },
                { "usage", (int)(PropertyUsageFlags.ReadOnly | PropertyUsageFlags.Editor) },
                { "hint", (int)PropertyHint.TypeString },
                { "hint_string", $"{Variant.Type.Object}/{PropertyHint.NodeType:D}:CollisionShape2D" },
            });
        }

        propList.AddRange([
            new()
            {
                { "name", "ResetAllFrames" },
                { "type", (int)Variant.Type.Callable },
                { "usage", (int)PropertyUsageFlags.Default },
                { "hint", (int)PropertyHint.ToolButton },
                { "hint_string", "Reset All Frames" }
            },
            new()
            {
                { "name", "RemoveAllFrames" },
                { "type", (int)Variant.Type.Callable },
                { "usage", (int)PropertyUsageFlags.Default },
                { "hint", (int)PropertyHint.ToolButton },
                { "hint_string", "Remove All Frames" }
            },
        ]);

        return propList;
    }
}