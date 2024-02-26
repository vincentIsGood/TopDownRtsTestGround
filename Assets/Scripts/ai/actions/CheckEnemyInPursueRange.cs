using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class CheckEnemyInPursueRange: BehaviorTreeNode<SquadBTData>{
    private Collider2D[] colliders = new Collider2D[2];

    public override NodeState evaluate(){
        SquadBTData sharedData = tree.sharedData;

        int hitCount = Physics2D.OverlapCircleNonAlloc(
            sharedData.squad.center, sharedData.findRange, colliders, sharedData.enemyMask);
        for(int i = 0; i < hitCount; i++){
            if(!colliders[i].TryGetComponent(out Soldier soldier)) continue;
            sharedData.target = soldier.ownSquad;
            return state = NodeState.SUCCESS;
        }
        
        if(sharedData.target){
            if(Vector3.Distance(sharedData.squad.center, sharedData.target.center) < sharedData.chaseRange)
                return state = NodeState.SUCCESS;
        }
        return state = NodeState.FAILURE;
    }
}