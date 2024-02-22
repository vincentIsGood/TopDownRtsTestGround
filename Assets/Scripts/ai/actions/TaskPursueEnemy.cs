using com.vincentcodes.ai.behaviortree;

public class TaskPursueEnemy: BehaviorTreeNode<SquadBTData>{
    public override NodeState evaluate(){
        tree.sharedData.squad.moveToPos(tree.sharedData.target.center);
        return state = NodeState.RUNNING;
    }
}