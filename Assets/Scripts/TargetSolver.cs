using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetSolver{
    private Squad ownSquad;
    public HashSet<GameUnit> targetedEnemies = new HashSet<GameUnit>();

    public TargetSolver(Squad ownSquad){
        this.ownSquad = ownSquad;
    }


    public GameUnit assignTargetFrom(Squad enemySquad, GameUnit member, bool allowOverride = true){
        List<GameUnit> enemies = enemySquad.getUnits();
        if(enemies.Count == 0) return null;

        GameUnit originalTarget = member.getTarget();
        if(originalTarget != null){
            if(!allowOverride && !originalTarget.isDead()) return originalTarget;
            else clearTargetFor(member);
        }

        GameUnit target = null;
        if(ownSquad.config.targetMode == TargetMode.Closest){
            target = findClosestEnemy(enemies, member);
        }else if(ownSquad.config.targetMode == TargetMode.Distributed){
            target = findEnemyDistributed(enemies, member);
        }else{
            target = enemies[0];
        }
        if(target == null){

            target = findClosestEnemy(enemies, member);
        }

        targetedEnemies.Add(target);
        return target;
    }

    public void clearTargetFor(GameUnit member){
        if(member.getTarget() == null) return;
        targetedEnemies.Remove(member.getTarget());
        member.setTarget(null);
    }
    public void clearSquadTargets(){
        if(targetedEnemies.Count == 0) return;
        foreach(GameUnit member in ownSquad.getUnits()){
            clearTargetFor(member);
        }
    }

    public void setTargetMode(TargetMode mode){
        this.ownSquad.config.targetMode = mode;
    }

    private GameUnit findClosestEnemy(List<GameUnit> enemies, GameUnit member){
        GameUnit target = null;
        float closestDist = float.MaxValue;
        foreach(GameUnit soldier in enemies){
            float dist = Vector3.Distance(member.getTransform().position, soldier.getTransform().position);
            if(dist < closestDist){
                closestDist = dist;
                target = soldier;
            }
        }
        return target;
    }
    private GameUnit findEnemyDistributed(List<GameUnit> enemies, GameUnit member){
        GameUnit target = null;
        foreach(GameUnit soldier in enemies){

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