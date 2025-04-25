using System;
using System.Linq;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class CreateRequestedUnit: BehaviorTreeNode<CommanderBTData>{
    public override NodeState evaluate(){
        Type unitType = tree.sharedData.nextUnitBeCreated;
        Squad squad = null;
        if(unitType == typeof(Soldier)){
            squad = tree.sharedData.barracks.spawnInfantry();
        }else if(unitType == typeof(LightTankUnit)){
            squad = tree.sharedData.tankFactory.spawnLightTank();
        }else if(unitType == typeof(HeavyTankUnit)){
            squad = tree.sharedData.tankFactory.spawnHeavyTank();
        }

        if(squad != null){
            tree.sharedData.nextUnitBeCreated = null;
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}