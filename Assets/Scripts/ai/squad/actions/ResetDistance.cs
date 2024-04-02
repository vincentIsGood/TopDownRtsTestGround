using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class ResetDistance: BehaviorTreeNode<SquadBTData>{
    public override NodeState evaluate(){
        if(!tree.sharedData.squad.config.allowAiMovement) return NodeState.FAILURE;
        tree.sharedData.squad.resetStoppingDistance();
        return NodeState.SUCCESS;
    }
}