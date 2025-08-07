using System.Collections.Generic;
using Godot;
using Godot.Collections;

public abstract partial class CombatCollider2D<[MustBeVariant] T> : Area2D
    where T : HurtboxShape2D
{
    private enum ShapeType
    {
        Circle,
        Rectangle,
        Capsule
    }

    [Export(PropertyHint.Range, "0,10,or_greater,hide_slider")]
    protected int CurrentFrame = 0;

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
                ShapeType.Circle => new CircleShape2D(),
                ShapeType.Rectangle => new RectangleShape2D(),
                ShapeType.Capsule => new CapsuleShape2D(),
                _ => Shape2D
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

    private void ActivateFrame(List<Frame<T>> frames, int actingFrame = 0)
    {
        frames[actingFrame].SetDisable(false);
    }

    private void DeactivateAllFrames()
    {
    }

    private void AddFrame() { }

    private void AddCollisionCurrent() { }

    private void AddCollisionNext() { }

    private void ResetCurrentFrame() { }

    private void RemoveCurrentFrame() { }

    private void ResetAllFrames() { }

    private void RemoveAllFrames() { }

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
            { "usage", (int)(PropertyUsageFlags.Checkable | PropertyUsageFlags.Editor) }
        });
        propList.Add(new()
        {
            { "name", "_alwaysActive" },
            { "type", (int)Variant.Type.Bool },
            { "usage", (int)(PropertyUsageFlags.Checkable) }
        });

        return propList;
    }
}