using com.vincentcodes.ai.behaviortree;

public class CreateTank: BehaviorTreeNode<CommanderBTData>{
    public override NodeState evaluate(){
        Squad squad = tree.sharedData.tankFactory.spawnTank();
        return squad != null? NodeState.SUCCESS : NodeState.FAILURE;
    }
}