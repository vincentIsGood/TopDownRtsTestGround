using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetSolver{
    private Squad ownSquad;
    public HashSet<Soldier> targetedEnemies = new HashSet<Soldier>();

    public TargetSolver(Squad ownSquad){
        this.ownSquad = ownSquad;
    }

    // remember to clearTarget() from "member"
    public Soldier assignTargetFrom(Squad enemySquad, Soldier member, bool allowOverride = true){
        List<Soldier> enemies = enemySquad.getSoldiers();
        if(enemies.Count == 0) return null;
        
        if(member.target != null){
            if(!allowOverride) return member.target;
            else clearTargetFor(member);
        }

        Soldier target = null;
        if(ownSquad.config.targetMode == TargetMode.Closest){
            target = findClosestEnemy(enemies, member);
        }else if(ownSquad.config.targetMode == TargetMode.Distributed){
            target = findEnemyDistributed(enemies, member);
        }else{
            target = enemies[0];
        }
        if(!target){
            // fallback
            target = findClosestEnemy(enemies, member);
        }

        targetedEnemies.Add(target);
        return target;
    }

    public void clearTargetFor(Soldier member){
        if(!member.target) return;
        targetedEnemies.Remove(member.target);
        member.target = null;
    }
    public void clearSquadTargets(){
        if(targetedEnemies.Count == 0) return;
        foreach(Soldier member in ownSquad.getSoldiers()){
            clearTargetFor(member);
        }
    }

    public void setTargetMode(TargetMode mode){
        this.ownSquad.config.targetMode = mode;
    }

    private Soldier findClosestEnemy(List<Soldier> enemies, Soldier member){
        Soldier target = null;
        float closestDist = float.MaxValue;
        foreach(Soldier soldier in enemies){
            float dist = Vector3.Distance(member.transform.position, soldier.transform.position);
            if(dist < closestDist){
                closestDist = dist;
                target = soldier;
            }
        }
        return target;
    }
    private Soldier findEnemyDistributed(List<Soldier> enemies, Soldier member){
        Soldier target = null;
        foreach(Soldier soldier in enemies){
            // TODO: use probability?
            if(targetedEnemies.Contains(soldier)) continue;
            target = soldier;
            break;
        }
        return target;
    }
}

public enum TargetMode{
    Closest,
    Distributed
};