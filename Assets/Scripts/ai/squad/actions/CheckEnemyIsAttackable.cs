using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class CheckEnemyIsAttackable: BehaviorTreeNode<SquadBTData>{
    private Collider2D[] colliders = new Collider2D[5];
    private GameUnit unit;
    private GameBuilding building;

    public override NodeState evaluate(){
        SquadBTData sharedData = tree.sharedData;

        // TODO: Make it smarter. Create some kind of priority list and determine what to attack next
        int hitCount = Physics2D.OverlapCircleNonAlloc(
            sharedData.squad.center, sharedData.attackRange, colliders, sharedData.enemyMask);
        for(int i = 0; i < hitCount; i++){
            if(!colliders[i].TryGetComponent(out unit)) continue;
            if(!GameVisionUtils.canSeeTarget(sharedData.squad, unit, sharedData.squad.getExcludeFromView())) continue;
            sharedData.target = null;
            if(unit is not GameBuilding){
                sharedData.target = unit.getOwnSquad();
            }else{
                sharedData.target = null;
                sharedData.targetUnit = unit;
            }
            return state = NodeState.SUCCESS;
        }
        return state = NodeState.FAILURE;
    }
}