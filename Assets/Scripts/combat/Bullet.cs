using System;
using UnityEngine;

public class Bullet: MonoBehaviour{
    public BulletType type = BulletType.NORMAL;
    [NonSerialized] public float speed = 10;

    [NonSerialized] public GameUnit owner;
    [NonSerialized] public Vector3 forward;

    private Rigidbody2D rb;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 60);
    }

    void Update(){
        rb.velocity = forward * speed;
    }

    public virtual void OnTriggerEnter2D(Collider2D collider){
        int layerMask = owner.getOwnSquad().config.enemyMask | owner.getOwnSquad().config.wallMask;
        if(owner.getOwnSquad().isExcludedFromView(collider.gameObject)) return;
        if((1 << collider.gameObject.layer & layerMask) == 0) return;

        dealDamageToEnemy(collider);
        Destroy(this.gameObject);
    }

    protected void dealDamageToEnemy(Collider2D collider){
        CombatManager combatManager = owner.getCombatManager();
        if(collider.TryGetComponent(out GameBuilding building)){
            combatManager.attack(building);
        }else if(collider.TryGetComponent(out GameUnit unit)){
            if(unit is TankUnit){
                if(type == BulletType.ANTI_TANK)
                    combatManager.attack(unit);
            }else{
                combatManager.attack(unit);
            }
        }
    }
}

public enum BulletType{
    NORMAL,
    ANTI_TANK
}
