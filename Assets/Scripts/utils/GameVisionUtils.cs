using UnityEngine;

public class GameVisionUtils{
    public static bool canSeeTarget(GameUnit from, GameUnit to){
        int layerMask = from.getOwnSquad().config.enemyMask | from.getOwnSquad().config.wallMask;
        Vector3 direction = (to.getTransform().position - from.getTransform().position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(from.getTransform().position, direction, float.PositiveInfinity, layerMask);
        return hit.collider != null && hit.collider.TryGetComponent(out GameUnit _);
    }
    public static bool canSeeTarget(GameUnit from, GameUnit to, out RaycastHit2D hit){
        int layerMask = from.getOwnSquad().config.enemyMask | from.getOwnSquad().config.wallMask;
        Vector3 direction = (to.getTransform().position - from.getTransform().position).normalized;
        hit = Physics2D.Raycast(from.getTransform().position, direction, float.PositiveInfinity, layerMask);
        return hit.collider != null && hit.collider.TryGetComponent(out GameUnit _);
    }
    
    public static bool canSeeTarget(Squad from, GameUnit to){
        int layerMask = from.config.enemyMask | from.config.wallMask;
        Vector3 direction = (to.getTransform().position - from.center).normalized;
        RaycastHit2D hit = Physics2D.Raycast(from.center, direction, float.PositiveInfinity, layerMask);
        return hit.collider != null && anyHitOnUnitOrSquad(hit.collider);
    }
    public static bool canSeeTarget(Squad from, GameUnit to, out RaycastHit2D hit){
        int layerMask = from.config.enemyMask | from.config.wallMask;
        Vector3 direction = (to.getTransform().position - from.center).normalized;
        hit = Physics2D.Raycast(from.center, direction, float.PositiveInfinity, layerMask);
        return hit.collider != null && anyHitOnUnitOrSquad(hit.collider);
    }

    private static bool anyHitOnUnitOrSquad(Collider2D collider){
        if(collider.TryGetComponent(out GameUnit _))
            return true;
        return collider.transform.parent != null && collider.transform.parent.TryGetComponent(out Squad _);
    }
}