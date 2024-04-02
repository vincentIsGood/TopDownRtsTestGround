using System.Collections.Generic;
using System.Linq;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class FindAndSelectSquad: BehaviorTreeNode<CommanderBTData>{
    public override NodeState evaluate(){
        float sumInfluence = 0;
        int failsafe = tree.sharedData.self.squads.Count + 10;
        tree.sharedData.selectedSquads.Clear();
        
        while(sumInfluence <= Mathf.Abs(tree.sharedData.moveToTargetPosInfluence.diff) && failsafe-- > 0){
            Squad squad = RandomUtils.randomElement(tree.sharedData.self.squads);
            if(squad != null && !tree.sharedData.selectedSquads.Contains(squad) && !squad.isMoving()){
                tree.sharedData.selectedSquads.Add(squad);
                sumInfluence += InfluenceUtils.ofSquad(squad);
            }
        }
        tree.sharedData.selectedSquadSumInfluence = sumInfluence;
        if(tree.sharedData.selectedSquads.Count > 0){
            // Debug.Log("Selected squad count: " + tree.sharedData.selectedSquads.Count);
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}