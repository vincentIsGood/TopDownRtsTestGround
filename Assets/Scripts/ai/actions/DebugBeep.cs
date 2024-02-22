using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class DebugBeep: BehaviorTreeNode<SquadBTData>{
    public override NodeState evaluate(){
        Debug.Log("Beep");
        return state = NodeState.SUCCESS;
    }
}