using Godot;
using Godot.Collections;

[GlobalClass]
public partial class HitboxSet : CombatCollider2D<HitboxShape2D>
{    

    [Export]
    private Color _debugColor = new Color("Red", 0.3f);
    
    protected override void ChangeCollision()
    {
        SetCollisionLayerValue(EnemyHitboxLayer, IsEnemy);
        SetCollisionMaskValue(PlayerHurtboxLayer, IsEnemy);
        
        SetCollisionLayerValue(PlayerHitboxLayer, !IsEnemy);
        SetCollisionMaskValue(EnemyHurtboxLayer, !IsEnemy);
    }
}