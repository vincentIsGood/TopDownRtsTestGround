using com.vincentcodes.ai.behaviortree;

public class CanCreateSoldier: BehaviorTreeNode<CommanderBTData>{
    public override NodeState evaluate(){
        if(tree.sharedData.barracks == null || tree.sharedData.self.squads.Count >= GameMap.instance.maxSquads) 
            return NodeState.FAILURE;

        float currentTeamInfluence = InfluenceUtils.ofGameTeam(tree.sharedData.self.ownTeam) - tree.sharedData.idealInfluence;
        return currentTeamInfluence < InfluenceUtils.ofGameTeam(tree.sharedData.self.enemyTeam)? 
            NodeState.SUCCESS : NodeState.FAILURE;
    }
}