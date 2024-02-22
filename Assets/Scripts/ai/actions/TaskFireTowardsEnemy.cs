using com.vincentcodes.ai.behaviortree;

public class TaskFireTowardsEnemy: BehaviorTreeNode<SquadBTData>{
    public override NodeState evaluate(){
        tree.sharedData.squad.fireTowards(tree.sharedData.target);
        return state = NodeState.SUCCESS;
    }
}