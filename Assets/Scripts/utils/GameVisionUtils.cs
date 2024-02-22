using UnityEngine;

public class GameVisionUtils{
    public static bool canSeeTarget(Soldier from, Soldier to){
        int layerMask = from.ownSquad.config.enemyMask | from.ownSquad.config.wallMask;
        Vector3 direction = (to.transform.position - from.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(from.transform.position, direction, float.PositiveInfinity, layerMask);
        return hit.collider != null && (1 << hit.collider.gameObject.layer & from.ownSquad.config.enemyMask) > 0;
    }

    // hit.collider == null if not found
    public static RaycastHit2D canSeeTargetDetailed(Soldier from, Soldier to){
        int layerMask = from.ownSquad.config.enemyMask | from.ownSquad.config.wallMask;
        Vector3 direction = (to.transform.position - from.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(from.transform.position, direction, float.PositiveInfinity, layerMask);
        return hit.collider != null && (1 << hit.collider.gameObject.layer & from.ownSquad.config.enemyMask) > 0? hit : default;
    }
    
    public static bool canSeeTarget(Squad from, Squad to){
        int layerMask = from.config.enemyMask | from.config.wallMask;
        Vector3 direction = (to.center - from.center).normalized;
        RaycastHit2D hit = Physics2D.Raycast(from.center, direction, float.PositiveInfinity, layerMask);
        // hit.collider == null -> vision clear (ie. can see at infinite range) || if hit? make sure it's an enemy (not a wall)
        return hit.collider == null || (1 << hit.collider.gameObject.layer & from.config.enemyMask) > 0;
    }
    
    // hit.collider == null if not found
    public static RaycastHit2D canSeeTargetDetailed(Squad from, Squad to){
        int layerMask = from.config.enemyMask | from.config.wallMask;
        Vector3 direction = (to.center - from.center).normalized;
        RaycastHit2D hit = Physics2D.Raycast(from.center, direction, float.PositiveInfinity, layerMask);
        return hit.collider == null || (1 << hit.collider.gameObject.layer & from.config.enemyMask) > 0? hit : default;
    }
}