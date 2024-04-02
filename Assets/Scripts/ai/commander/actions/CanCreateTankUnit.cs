using System.Linq;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class CanCreateTankUnit: BehaviorTreeNode<CommanderBTData>{
    public override NodeState evaluate(){
        if(tree.sharedData.tankFactory == null || tree.sharedData.self.squads.Count >= GameMap.instance.maxSquads) 
            return NodeState.FAILURE;

        float currentTeamInfluence = InfluenceUtils.ofGameTeam(tree.sharedData.self.ownTeam) - tree.sharedData.idealInfluence;
        if(tree.sharedData.self.squads.Any(s => s.getUnits()[0] is Soldier) 
        && currentTeamInfluence < InfluenceUtils.ofGameTeam(tree.sharedData.self.enemyTeam)
        && Random.value < 0.3f)
            return NodeState.SUCCESS;
        return NodeState.FAILURE;
    }
}