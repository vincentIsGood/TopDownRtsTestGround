using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class TaskFindCover: BehaviorTreeNode<SquadBTData>{
    private Collider2D[] colliders = new Collider2D[2];

    public override NodeState evaluate(){
        SquadBTData sharedData = tree.sharedData;

        int hitCount = Physics2D.OverlapCircleNonAlloc(
            sharedData.squad.center, sharedData.findRange, colliders, sharedData.coverMask);
        for(int i = 0; i < hitCount; i++){
            Cover cover = colliders[i].transform.GetComponent<Cover>();
            if(cover == null) continue;
            sharedData.squad.moveToPos(cover.transform.position, cover);
            return state = NodeState.SUCCESS;
        }
        return state = NodeState.FAILURE;
    }
}