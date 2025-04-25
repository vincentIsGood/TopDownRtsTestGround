using System.Collections.Generic;
using System.Linq;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class PrioritizeNextUnitToBeCreated: BehaviorTreeNode<CommanderBTData>{

    public override NodeState evaluate(){
        if(tree.sharedData.self.squads.Count >= GameMap.instance.maxSquads){
            tree.sharedData.nextUnitBeCreated = null;
            return NodeState.FAILURE;
        }

        float currentTeamInfluence = InfluenceUtils.ofGameTeam(tree.sharedData.self.ownTeam) - tree.sharedData.idealInfluence;
        bool enoughInfluence = currentTeamInfluence >= InfluenceUtils.ofGameTeam(tree.sharedData.self.enemyTeam);
        if(enoughInfluence){
            tree.sharedData.nextUnitBeCreated = null;
            return NodeState.FAILURE;
        }

        // Drop the probability of spawning common units (by 0.05 per squad spawned?)

        if(tree.sharedData.barracks != null){
            evaluateSpawningSoldiers();
        }

        if(tree.sharedData.tankFactory != null){
            evaluateSpawningTanks();
        }

        return tree.sharedData.nextUnitBeCreated != null? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private void evaluateSpawningSoldiers(){
        if(Random.value < tree.sharedData.soldierProb - tree.sharedData.self.unitTypeToAmount[typeof(Soldier)] * 0.0005){
            tree.sharedData.nextUnitBeCreated = typeof(Soldier);
        }
    }

    private void evaluateSpawningTanks(){
        bool isThereSoldierInMySquad = tree.sharedData.self.squads.Any(s => s.getUnitType() == typeof(Soldier));
        if(isThereSoldierInMySquad){
            if(Random.value < tree.sharedData.lightTankProb - tree.sharedData.self.unitTypeToAmount[typeof(LightTankUnit)] * 0.05){
                tree.sharedData.nextUnitBeCreated = typeof(LightTankUnit);
            }else if(Random.value < tree.sharedData.heavyTankProb - tree.sharedData.self.unitTypeToAmount[typeof(HeavyTankUnit)] * 0.3){
                tree.sharedData.nextUnitBeCreated = typeof(HeavyTankUnit);
            }
        }
    }
}