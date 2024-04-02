using com.vincentcodes.ai.behaviortree;

public class CreateInfantry: BehaviorTreeNode<CommanderBTData>{
    public override NodeState evaluate(){
        Squad squad = tree.sharedData.barracks.spawnInfantry();
        return squad != null? NodeState.SUCCESS : NodeState.FAILURE;
    }
}