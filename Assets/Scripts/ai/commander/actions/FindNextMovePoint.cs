using System.Collections.Generic;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class FindNextMovePoint: BehaviorTreeNode<CommanderBTData>{
    public override NodeState evaluate(){
        if(canGoToResourcePoint(ResourceType.Food, out tree.sharedData.moveToTargetPos)){
            // Debug.Log($"[{tree.sharedData.self.name}] Going to res pt: Food");
            return NodeState.SUCCESS;
        }else if(canGoToResourcePoint(ResourceType.Iron, out tree.sharedData.moveToTargetPos)){
            // Debug.Log($"[{tree.sharedData.self.name}] Going to res pt: Iron");
            return NodeState.SUCCESS;
        }else if(canGoToResourcePoint(ResourceType.Oil, out tree.sharedData.moveToTargetPos)){
            // Debug.Log($"[{tree.sharedData.self.name}] Going to res pt: Oil");
            return NodeState.SUCCESS;
        }

        if(findEnemyZoneLocation()){
            return NodeState.SUCCESS;
        }else if(findShelterZoneLocation()){
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }

    private bool findEnemyZoneLocation(){
        EnemyAllyInfluence finalResult = null;
        foreach(KeyValuePair<Vector3, EnemyAllyInfluence> zoneInfluence in tree.sharedData.potentialZoneInfluences){
            float diff = zoneInfluence.Value.diff;
            if(diff < tree.sharedData.idealInfluence && Random.value < 0.5f){
                if(diff < (tree.sharedData.selectedSquadSumInfluence - tree.sharedData.fearInfluence) 
                && Random.value < (1-tree.sharedData.braveryProb)){
                    // Debug.Log($"[{tree.sharedData.self.name}] Ignore feared zone ({zoneInfluence.Value.name}): {diff}");
                    continue;
                }
                finalResult = zoneInfluence.Value;
                tree.sharedData.moveToTargetPos = zoneInfluence.Key;
                if(Random.value < 0.3f) break;
            }
        }
        if(finalResult != null){
            // Debug.Log($"[{tree.sharedData.self.name}] Going to influence pt: {finalResult.name} (diff: {finalResult.diff})");
            tree.sharedData.moveToTargetPosInfluence = finalResult;
            return true;
        }
        return false;
    }
    private bool findShelterZoneLocation(){
        EnemyAllyInfluence finalResult = null;
        foreach(KeyValuePair<Vector3, EnemyAllyInfluence> zoneInfluence in tree.sharedData.zoneInfluences){
            float diff = zoneInfluence.Value.diff;
            if(diff >= 0 && Random.value < 0.7f){
                finalResult = zoneInfluence.Value;
                tree.sharedData.moveToTargetPos = zoneInfluence.Key;
            }
        }
        if(finalResult != null){
            // Debug.Log($"[{tree.sharedData.self.name}] Escaping to influence pt: {finalResult.name} (diff: {finalResult.diff})");
            tree.sharedData.moveToTargetPosInfluence = finalResult;
            return true;
        }
        return false;
    }

    private bool canGoToResourcePoint(ResourceType type, out Vector3 resourcePos){
        Dictionary<ResourceType, ResourcePoint> resourcesInNeed = tree.sharedData.resourcesInNeed;
        resourcePos = default;
        if(!resourcesInNeed.ContainsKey(type)) return false;
        if(tree.sharedData.self.ownTeam.Contains(resourcesInNeed[type].capturer)) return false;

        resourcePos = resourcesInNeed[type].transform.position;
        if(!tree.sharedData.potentialZoneInfluences.ContainsKey(resourcePos)) return false;
        EnemyAllyInfluence zoneInfluence = tree.sharedData.potentialZoneInfluences[resourcePos];
        return zoneInfluence.diff > tree.sharedData.selectedSquadSumInfluence - tree.sharedData.fearInfluence;
    }
}