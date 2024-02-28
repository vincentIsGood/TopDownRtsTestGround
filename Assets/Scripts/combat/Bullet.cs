using UnityEngine;

public class Bullet: MonoBehaviour{
    public float speed = 10;
    public GameUnit owner;
    public Vector3 forward;
    
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
        if((1 << collider.gameObject.layer & layerMask) > 0){
            if(collider.TryGetComponent(out unit))
                owner.getCombatManager().attack(unit);
            Destroy(this.gameObject);
        }
    }
}