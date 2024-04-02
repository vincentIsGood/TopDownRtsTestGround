using System;
using UnityEditor;
using UnityEngine;

public class TankAmmo: Bullet{
    private static Collider2D[] hits = new Collider2D[8];
    [NonSerialized] public float impactRadius;

#if UNITY_EDITOR
    void OnDrawGizmos(){
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.forward, impactRadius);
    }
#endif

    public override void OnTriggerEnter2D(Collider2D collider){
        int layerMask = owner.getOwnSquad().config.enemyMask | owner.getOwnSquad().config.wallMask;
        if(owner.getOwnSquad().isExcludedFromView(collider.gameObject)) return;
        if((1 << collider.gameObject.layer & layerMask) == 0) return;

        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, impactRadius, hits, owner.getOwnSquad().config.enemyMask);
        if(hitCount > 0){
            GameVisionUtils.loopThroughVisibleTargetsFromPoint(
                transform.position, hits, hitCount, 
                layerMask, owner.getOwnSquad().config.wallMask, (c)=>{
                if(c != collider && c.TryGetComponent(out GameUnit _))
                    dealDamageToEnemy(c);
            });
        }
        
        dealDamageToEnemy(collider);
        Destroy(this.gameObject);
    }
}