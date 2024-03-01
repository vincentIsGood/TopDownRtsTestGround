using System.Collections.Generic;
using UnityEngine;

// https://forum.unity.com/threads/are-raycasthit-arrays-returned-from-raycastall-in-proper-order.385131/
public class GameVisionUtils{
    public static float maxVisionLength = 10;

    private static RaycastHit2D[] hits = new RaycastHit2D[5];

    public static bool canSeeTarget(GameUnit from, GameUnit to){
        int layerMask = from.getOwnSquad().config.enemyMask | from.getOwnSquad().config.wallMask;
        Vector3 direction = (to.getTransform().position - from.getTransform().position).normalized;
        int hitCount = Physics2D.RaycastNonAlloc(from.getTransform().position, direction, hits, maxVisionLength, layerMask);
        if(hitCount == 0) return false;
        sortHits(hits, hitCount);

        for(int i = 0; i < hitCount; i++){
            RaycastHit2D hit = hits[i];
            if(isWallHit(hit.collider, from.getOwnSquad().config.wallMask))
                return false;
            if(hit.collider.TryGetComponent(out GameUnit _)){
                return true;
            }
        }
        return false;
    }
    public static bool canSeeTarget(GameUnit from, GameUnit to, out RaycastHit2D hit){
        int layerMask = from.getOwnSquad().config.enemyMask | from.getOwnSquad().config.wallMask;
        Vector3 direction = (to.getTransform().position - from.getTransform().position).normalized;
        int hitCount = Physics2D.RaycastNonAlloc(from.getTransform().position, direction, hits, maxVisionLength, layerMask);
        hit = default;
        if(hitCount == 0) return false;
        sortHits(hits, hitCount);
        
        for(int i = 0; i < hitCount; i++){
            hit = hits[i];
            if(isWallHit(hit.collider, from.getOwnSquad().config.wallMask))
                return false;
            if(hit.collider.TryGetComponent(out GameUnit _)){
                return true;
            }
        }
        return false;
    }
    
    public static bool canSeeTarget(Squad from, GameUnit to){
        int layerMask = from.config.enemyMask | from.config.wallMask;
        Vector3 direction = (to.getTransform().position - from.center).normalized;
        int hitCount = Physics2D.RaycastNonAlloc(from.center, direction, hits, maxVisionLength, layerMask);
        if(hitCount == 0) return false;
        sortHits(hits, hitCount);

        for(int i = 0; i < hitCount; i++){
            RaycastHit2D hit = hits[i];
            if(isWallHit(hit.collider, from.config.wallMask))
                return false;
            if(anyHitOnUnitOrSquad(hit.collider)){
                return true;
            }
        }
        return false;
    }
    public static bool canSeeTarget(Squad from, GameUnit to, out RaycastHit2D hit){
        int layerMask = from.config.enemyMask | from.config.wallMask;
        Vector3 direction = (to.getTransform().position - from.center).normalized;
        int hitCount = Physics2D.RaycastNonAlloc(from.center, direction, hits, maxVisionLength, layerMask);
        hit = default;
        if(hitCount == 0) return false;
        sortHits(hits, hitCount);

        for(int i = 0; i < hitCount; i++){
            hit = hits[i];
            if(isWallHit(hit.collider, from.config.wallMask))
                return false;
            if(anyHitOnUnitOrSquad(hit.collider)){
                return true;
            }
        }
        return false;
    }

    private static void sortHits(RaycastHit2D[] hits, int length){
        System.Array.Sort(hits, 0, length, RaycastDistComparer.instance);
    }

    private static bool isWallHit(Collider2D collider, int mask){
        return ((1 << collider.gameObject.layer) & mask) > 0;
    }

    private static bool anyHitOnUnitOrSquad(Collider2D collider){
        if(collider.TryGetComponent(out GameUnit _))
            return true;
        return collider.transform.parent != null && collider.transform.parent.TryGetComponent(out Squad _);
    }

    class RaycastDistComparer : IComparer<RaycastHit2D>{
        public static RaycastDistComparer instance = new RaycastDistComparer();

        public int Compare(RaycastHit2D x, RaycastHit2D y){
            return x.distance.CompareTo(y.distance);
        }
    }
}