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
        GameUnit unit;
        int layerMask = owner.getOwnSquad().config.enemyMask | owner.getOwnSquad().config.wallMask;
        if((1 << collider.gameObject.layer & layerMask) == 0) return;
        if(collider.TryGetComponent(out unit)){
            if(unit is TankUnit){
                if(type == BulletType.ANTI_TANK)
                    owner.getCombatManager().attack(unit);
            }else{
                owner.getCombatManager().attack(unit);
            }
        }
        Destroy(this.gameObject);
    }
}

public enum BulletType{
    NORMAL,
    ANTI_TANK
}