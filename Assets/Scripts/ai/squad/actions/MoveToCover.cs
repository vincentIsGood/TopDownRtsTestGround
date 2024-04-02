using com.vincentcodes.ai.behaviortree;

public class MoveToCover: BehaviorTreeNode<SquadBTData>{
    public override NodeState evaluate(){
        SquadBTData sharedData = tree.sharedData;
        sharedData.squad.moveToPos(sharedData.moveToCover.transform.position, sharedData.moveToCover);
        return NodeState.RUNNING;
    }
}