using com.vincentcodes.ai.behaviortree;

public class FindBuildings: BehaviorTreeNode<CommanderBTData>{
    public override NodeState evaluate(){
        foreach(GameBuilding building in tree.sharedData.self.buildings){
            if(building is Barracks barracks){
                tree.sharedData.barracks = barracks;
                continue;
            }
            if(building is TankFactory tankFactory){
                tree.sharedData.tankFactory = tankFactory;
                continue;
            }
        }
        return NodeState.SUCCESS;
    }
}