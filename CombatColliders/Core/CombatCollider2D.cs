using System;
using System.Linq;
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

    protected Dictionary<int, Array<T>> Frames = new();

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
        SetCollisionLayerValue(1, false);
        SetCollisionMaskValue(1, false);
        ChangeCollision();

        // AreaShapeEntered += OnAreaShapeEntered;
    }

    protected void Start() { }
    protected void NextFrame() { }
    protected void End() { }

    //-------------
    //---Utility---
    //-------------
    protected abstract void ChangeCollision();

    /// <summary>
    /// Activates the acting frame from the possible frames in this Set
    /// </summary>
    /// <param name="actingFrame">The frame that needs activation</param>
    private void ActivateFrame(int actingFrame = 0)
    {
        //frames[actingFrame].SetDisable(false);
    }

    /// <summary>
    /// Deactivates All frames within this set
    /// </summary>
    private void DeactivateAllFrames()
    {
        // foreach (var frame in frames)
        // frame.SetDisable(true);
    }


    protected virtual void AddCollisionToFrame(int actingFrame)
    {
        if (actingFrame < 0)
            throw new ArgumentException("Acting Frame is less than zero");
        if (!Frames.ContainsKey(actingFrame))
            Frames.Add(actingFrame, new Array<T>());

        CurrentFrame = actingFrame;
        NotifyPropertyListChanged();
    }

    private void AddFrame() { }

    protected T AddShape(Shape2D shape, string name, Color debugColor)
    {
        T collider = new();
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

    private Callable ResetCurrentFrame()
    {
        if (!Frames.TryGetValue(CurrentFrame, out var actingFrame)) return default;
        foreach (var collider in actingFrame)
            collider.QueueFree();

        actingFrame.Clear();
        NotifyPropertyListChanged();
        return default;
    }

    private Callable RemoveCurrentFrame()
    {
        ResetCurrentFrame();
        Frames.Remove(CurrentFrame);
        CurrentFrame--;
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

    public override Array<Dictionary> _GetPropertyList()
    {
        var propList = new Array<Dictionary>();

        propList.AddRange([
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
                {"name", "_debugColor"},
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

        if (Frames.TryGetValue(_viewFrame, out var viewFrame) && viewFrame.Count != 0)
        {
            _viewArray = Frames[_viewFrame];
            propList.Add(new()
            {
                { "name", "_viewArray" },
                { "type", (int)Variant.Type.Array },
                { "usage", (int)(PropertyUsageFlags.ReadOnly | PropertyUsageFlags.Editor) },
                { "hint", (int)PropertyHint.TypeString },
                { "hint_string", $"{Variant.Type.Object}/{PropertyHint.NodeType:D}:CollisionShape2D" },
            });
        }

        return propList;
    }
}