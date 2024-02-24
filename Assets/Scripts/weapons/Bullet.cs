using UnityEngine;

public class Bullet: MonoBehaviour{
    public float speed = 2;
    public Soldier owner;
    public Vector3 forward;

    private Rigidbody2D rb;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        rb.velocity = forward * speed;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.TryGetComponent(out Soldier soldier)){
            int layerMask = owner.ownSquad.config.enemyMask | owner.ownSquad.config.wallMask;
            if((1 << collider.gameObject.layer & layerMask) > 0)
                Destroy(this.gameObject);
        }
    }
}