using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class CanTakeCover: BehaviorTreeNode<SquadBTData>{
    public override NodeState evaluate(){
        SquadBTData sharedData = tree.sharedData;
        return sharedData.squad.engaging && sharedData.allowAiMovement? NodeState.SUCCESS : NodeState.FAILURE;
    }
}