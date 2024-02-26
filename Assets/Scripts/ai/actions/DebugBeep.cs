using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class DebugBeep: BehaviorTreeNode<SquadBTData>{
    private string beep = "Beep";

    public DebugBeep(string beep){
        this.beep = beep;
    }

    public override NodeState evaluate(){
        Debug.Log(beep);
        return state = NodeState.SUCCESS;
    }
}