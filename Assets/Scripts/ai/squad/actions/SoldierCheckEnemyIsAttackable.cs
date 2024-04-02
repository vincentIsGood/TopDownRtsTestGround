using System;
using System.Collections.Generic;
using System.Linq;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class SoldierCheckEnemyIsAttackable: BehaviorTreeNode<SquadBTData>{
    private Collider2D[] colliders = new Collider2D[5];
    private GameUnit unit;
    private GameUnit lowlyPrioritizedUnit;

    private Dictionary<Type, List<GameUnit>> sawUnits;

    public override NodeState evaluate(){
        SquadBTData sharedData = tree.sharedData;
        if(sawUnits == null){
            sawUnits = new Dictionary<Type, List<GameUnit>>();
            sawUnits[typeof(TankUnit)] = new List<GameUnit>();
            sawUnits[typeof(Soldier)] = new List<GameUnit>();
        }

        int hitCount = Physics2D.OverlapCircleNonAlloc(
            sharedData.squad.center, sharedData.attackRange, colliders, sharedData.enemyMask);
        if(hitCount == 0) return NodeState.FAILURE;

        clearSawUnits();
        
        for(int i = 0; i < hitCount; i++){
            if(!colliders[i].TryGetComponent(out unit)) continue;
            if(!GameVisionUtils.canSeeTarget(sharedData.squad, unit, sharedData.squad.getExcludeFromView())) continue;
            sharedData.target = null;
            if(unit is not GameBuilding){
                sawUnits[unit.GetType()].Add(unit);
            }else{
                lowlyPrioritizedUnit = unit;
            }
        }
        
        if(sawUnits[typeof(Soldier)].Count > 0){
            sharedData.target = sawUnits[typeof(Soldier)][0].getOwnSquad();
            return state = NodeState.SUCCESS;
        }
        // if(sawUnits[typeof(TankUnit)].Count > 0){
        //     sharedData.target = sawUnits[typeof(TankUnit)][0].getOwnSquad();
        //     return state = NodeState.SUCCESS;
        // }

        if(lowlyPrioritizedUnit != null){
            sharedData.targetUnit = lowlyPrioritizedUnit;
            return state = NodeState.SUCCESS;
        }
        return state = NodeState.FAILURE;
    }

    private void clearSawUnits(){
        foreach(List<GameUnit> v in sawUnits.Values)
            v.Clear();
    }
}