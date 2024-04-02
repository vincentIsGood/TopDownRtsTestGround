using System.Collections.Generic;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class WalkToPoint: BehaviorTreeNode<CommanderBTData>{
    public override NodeState evaluate(){
        moveTo(tree.sharedData.moveToTargetPos);
        return NodeState.RUNNING;
    }

    private void moveTo(Vector3 pos){
        foreach(Squad squad in tree.sharedData.selectedSquads){
            squad.moveToPos(pos);
        }
    }
}