using com.vincentcodes.ai.behaviortree;

public class TaskFireTowardsEnemy: BehaviorTreeNode<SquadBTData>{
    public override NodeState evaluate(){
        if(tree.sharedData.target != null)
            tree.sharedData.squad.fireTowards(tree.sharedData.target);
        else tree.sharedData.squad.fireTowards(tree.sharedData.targetUnit);
        return state = NodeState.RUNNING;
    }
}