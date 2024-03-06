using UnityEngine;

public class Bullet: MonoBehaviour{
    public float speed = 10;
    public GameUnit owner;
    public Vector3 forward;

    public BulletType type = BulletType.NORMAL;
    
    private Rigidbody2D rb;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 60);
    }

    void Update(){
        rb.velocity = forward * speed;
    }

    void OnTriggerEnter2D(Collider2D collider){
        int layerMask = owner.getOwnSquad().config.enemyMask | owner.getOwnSquad().config.wallMask;
        if(owner.getOwnSquad().isExcludedFromView(collider.gameObject)) return;
        if((1 << collider.gameObject.layer & layerMask) == 0) return;
        
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
        Destroy(this.gameObject);
    }
}

public enum BulletType{
    NORMAL,
    ANTI_TANK
}