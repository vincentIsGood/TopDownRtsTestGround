using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class CheckEnemyIsAttackable: BehaviorTreeNode<SquadBTData>{
    private Collider2D[] colliders = new Collider2D[5];
    private Soldier soldier;

    public override NodeState evaluate(){
        SquadBTData sharedData = tree.sharedData;

        int hitCount = Physics2D.OverlapCircleNonAlloc(
            sharedData.squad.center, sharedData.attackRange, colliders, sharedData.enemyMask);
        for(int i = 0; i < hitCount; i++){
            if(!colliders[i].transform.TryGetComponent<Soldier>(out soldier)) continue;
            if(!GameVisionUtils.canSeeTarget(sharedData.squad, soldier)) continue;
            sharedData.target = soldier.ownSquad;
            return state = NodeState.SUCCESS;
        }
        return state = NodeState.FAILURE;
    }
}