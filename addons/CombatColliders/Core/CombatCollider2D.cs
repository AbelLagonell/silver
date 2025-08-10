using System;
using System.Linq;
using Godot;
using Godot.Collections;
using Microsoft.VisualBasic;

public abstract partial class CombatCollider2D<[MustBeVariant] T> : Area2D
    where T : HurtboxShape2D, new()
{
    
    public enum ShapeType
    {
        Circle,
        Rectangle,
        Capsule
    }

    [Export(PropertyHint.Range, "0,10,or_greater,hide_slider")]
    protected int CurrentFrame = 0;

    protected Dictionary<int, Array<T>> Frames = new();

    //----------------
    //---Shape Type---
    //----------------

    [Export]
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

    [Export]
    protected bool IsEnemy
    {
        get => _isEnemy;
        set
        {
            _isEnemy = value;
            ChangeCollision();
        }
    }


    [ExportGroup("Collision Layers")]
    [Export(PropertyHint.Range, "1,32,1")]
    private int _enemyHurtboxLayer
    {
        get => EnemyHurtboxLayer;
        set => EnemyHurtboxLayer = value;
    }

    protected static int EnemyHurtboxLayer = 3;

    [Export(PropertyHint.Range, "1,32,1")]
    private int _playerHurtboxLayer
    {
        get => PlayerHurtboxLayer;
        set => PlayerHurtboxLayer = value;
    }

    protected int EnemyHitboxLayer = 7;

    [Export(PropertyHint.Range, "1,32,1")]
    private int _enemyHitboxLayer
    {
        get => EnemyHitboxLayer;
        set => EnemyHitboxLayer = value;
    }

    protected int PlayerHitboxLayer = 8;

    [Export(PropertyHint.Range, "1,32,1")]
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
        if (actingFrame + 1 > Frames.Count)
        {
            GD.PrintErr($"Tried to make frame {actingFrame} but its not in sequence placing new frame in frame {Frames.Count + 1}");
            actingFrame = Frames.Count;
        }

        if (actingFrame == Frames.Count)
            // frames.Add(new Frame<T>());

            CurrentFrame = actingFrame;
    }

    private void AddFrame() { }

    private void AddCollisionCurrent()
    {
        AddCollisionToFrame(CurrentFrame);
    }

    private void AddCollisionNext()
    {
        AddCollisionToFrame(++CurrentFrame);
    }

    private void ResetCurrentFrame()
    {
        if (!Frames.TryGetValue(CurrentFrame, out var actingFrame)) return;
        foreach (var collider in actingFrame)
            collider.QueueFree();
        
        actingFrame.Clear();
    }

    private void RemoveCurrentFrame()
    {
        ResetCurrentFrame();
        // frames[CurrentFrame].ResetShapes();
        // frames.RemoveAt(CurrentFrame);
    }

    private void ResetAllFrames()
    {
        // foreach (var frame in frames)
        // frame.ResetShapes();
    }

    private void RemoveAllFrames()
    {
        for (int i = 0; i < Frames.Count(); i++)
        {
            // frames[i].ResetShapes();
        }

        Frames.Clear();
        CurrentFrame = 0;
    }

    public override Array<Dictionary> _GetPropertyList()
    {
        var propList = new Array<Dictionary>();

        propList.Add(new()
        {
            { "name", "Debug" },
            { "type", (int)Variant.Type.String },
            { "usage", (int)PropertyUsageFlags.Group }
        });

        propList.Add(new()
        {
            { "name", "_alwaysVisible" },
            { "type", (int)Variant.Type.Bool },
        });
        propList.Add(new()
        {
            { "name", "_alwaysActive" },
            { "type", (int)Variant.Type.Bool },
        });

        return propList;
    }
}