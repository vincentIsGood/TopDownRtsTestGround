using UnityEngine;

public class Bullet: MonoBehaviour{
    public float speed = 2;
    public Soldier owner;
    public Vector3 forward;

    private Rigidbody2D rb;
    private IntervalActionUtils deleteCounter;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        deleteCounter = new IntervalActionUtils(()=>Destroy(this.gameObject), 60);
    }

    void Update(){
        rb.velocity = forward * speed;
    }

    void OnTriggerEnter2D(Collider2D collider){
        Soldier soldier;
        int layerMask = owner.ownSquad.config.enemyMask | owner.ownSquad.config.wallMask;
        if((1 << collider.gameObject.layer & layerMask) > 0){
            if(collider.TryGetComponent(out soldier))
                owner.combatManager.attack(soldier);
            Destroy(this.gameObject);
        }
    }
}