using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetSolver{
    public TargetMode mode = TargetMode.Closest;

    private Squad ownSquad;
    private HashSet<Soldier> targetedEnemies = new HashSet<Soldier>();

    public TargetSolver(Squad ownSquad){
        this.ownSquad = ownSquad;
    }

    // remember to clearTarget() from "member"
    public Soldier assignTargetFrom(Squad enemySquad, Soldier member){
        Soldier[] enemies = enemySquad.getSoldiers();
        if(enemies.Length == 0) return null;

        Soldier target = null;
        if(mode == TargetMode.Closest){
            float closestDist = float.MaxValue;
            foreach(Soldier soldier in enemies){
                float dist = Vector3.Distance(member.transform.position, soldier.transform.position);
                if(dist < closestDist){
                    closestDist = dist;
                    target = soldier;
                }
            }
        }else if(mode == TargetMode.Distributed){
            foreach(Soldier soldier in enemies){
                if(targetedEnemies.Contains(soldier)) continue;
                target = soldier;
                break;
            }
        }else{
            target = enemies[0];
        }
        if(target == null) return target;

        targetedEnemies.Add(target);
        return target;
    }

    public void clearTarget(Soldier member){
        if(!member.target) return;
        targetedEnemies.Remove(member.target);
        member.target = null;
    }
    public void clearSquadTargets(){
        if(targetedEnemies.Count == 0) return;
        foreach(Soldier member in ownSquad.getSoldiers()){
            clearTarget(member);
        }
    }

    public void setTargetMode(TargetMode mode){
        this.mode = mode;
    }

    public static bool canSeeTarget(Soldier from, Soldier to){
        int layerMask = from.ownSquad.config.enemyMask | from.ownSquad.config.wallMask;
        RaycastHit2D hit = Physics2D.Raycast(from.transform.position, to.transform.position - from.transform.position, float.PositiveInfinity, layerMask);
        return (1 << hit.collider.gameObject.layer & from.ownSquad.config.enemyMask) > 0;
    }

    // hit.collider == null if not found
    public static RaycastHit2D canSeeTargetDetailed(Soldier from, Soldier to){
        int layerMask = from.ownSquad.config.enemyMask | from.ownSquad.config.wallMask;
        RaycastHit2D hit = Physics2D.Raycast(from.transform.position, to.transform.position - from.transform.position, float.PositiveInfinity, layerMask);
        return (1 << hit.collider.gameObject.layer & from.ownSquad.config.enemyMask) > 0? hit : default;
    }
}

public enum TargetMode{
    Closest,
    Distributed,
    FocusFire
};