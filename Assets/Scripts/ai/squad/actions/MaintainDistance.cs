using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class MaintainDistance: BehaviorTreeNode<SquadBTData>{
    public override NodeState evaluate(){
        if(!tree.sharedData.squad.config.allowAiMovement) return NodeState.FAILURE;
        tree.sharedData.squad.setStoppingDistance(Mathf.Max(tree.sharedData.squad.config.attackRange - 2, 0));
        return NodeState.SUCCESS;
    }
}