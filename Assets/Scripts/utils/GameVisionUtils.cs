using UnityEngine;

public class GameVisionUtils{
    public static bool canSeeTarget(Soldier from, Soldier to){
        int layerMask = from.ownSquad.config.enemyMask | from.ownSquad.config.wallMask;
        Vector3 direction = (to.transform.position - from.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(from.transform.position, direction, float.PositiveInfinity, layerMask);
        return hit.collider != null && hit.collider.TryGetComponent(out Soldier _);
    }
    public static bool canSeeTarget(Soldier from, Soldier to, out RaycastHit2D hit){
        int layerMask = from.ownSquad.config.enemyMask | from.ownSquad.config.wallMask;
        Vector3 direction = (to.transform.position - from.transform.position).normalized;
        hit = Physics2D.Raycast(from.transform.position, direction, float.PositiveInfinity, layerMask);
        return hit.collider != null && hit.collider.TryGetComponent(out Soldier _);
    }
    
    public static bool canSeeTarget(Squad from, Squad to){
        int layerMask = from.config.enemyMask | from.config.wallMask;
        Vector3 direction = (to.center - from.center).normalized;
        RaycastHit2D hit = Physics2D.Raycast(from.center, direction, float.PositiveInfinity, layerMask);
        return hit.collider != null && (hit.collider.transform.parent.TryGetComponent(out Squad _) || hit.collider.TryGetComponent(out Soldier _));
    }
    public static bool canSeeTarget(Squad from, Squad to, out RaycastHit2D hit){
        int layerMask = from.config.enemyMask | from.config.wallMask;
        Vector3 direction = (to.center - from.center).normalized;
        hit = Physics2D.Raycast(from.center, direction, float.PositiveInfinity, layerMask);
        return hit.collider != null && (hit.collider.transform.parent.TryGetComponent(out Squad _) || hit.collider.TryGetComponent(out Soldier _));
    }
}