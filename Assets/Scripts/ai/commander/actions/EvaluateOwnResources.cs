using System.Collections.Generic;
using System.Linq;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class EvaluateOwnResources: BehaviorTreeNode<CommanderBTData>{
    private List<KeyValuePair<ResourcePoint, float>> resPointsDist;

    public override NodeState evaluate(){
        tree.sharedData.resourcesInNeed.Clear();
        ResourceStat resourceStat = tree.sharedData.self.resourceStat;
        ResourceStat maintainResource = tree.sharedData.maintainResource;

        sortResPoints();
        if(resourceStat.food < maintainResource.food){
            ResourcePoint resourcePoint = findClosestResPoint(ResourceType.Food);
            if(resourcePoint != null)
                tree.sharedData.resourcesInNeed[ResourceType.Food] = resourcePoint;
        }
        if(resourceStat.iron < maintainResource.iron){
            ResourcePoint resourcePoint = findClosestResPoint(ResourceType.Iron);
            if(resourcePoint != null)
                tree.sharedData.resourcesInNeed[ResourceType.Iron] = resourcePoint;
        }
        if(resourceStat.oil < maintainResource.oil){
            ResourcePoint resourcePoint = findClosestResPoint(ResourceType.Oil);
            if(resourcePoint != null)
                tree.sharedData.resourcesInNeed[ResourceType.Oil] = resourcePoint;
        }
        return tree.sharedData.resourcesInNeed.Count > 0? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private ResourcePoint findClosestResPoint(ResourceType type){
        foreach(KeyValuePair<ResourcePoint, float> res in resPointsDist){
            if(res.Key.type == type){
                return res.Key;
            }
        }
        return null;
    }

    private Vector3 prevCenter = Vector3.zero;
    private void sortResPoints(){
        if(tree.sharedData.self.center == prevCenter){
            return;
        }
        Dictionary<ResourcePoint, float> resDist = GameMap.instance.resourcePoints.ToDictionary(
            x => x, x => Vector2.Distance(tree.sharedData.self.center, x.transform.position));
        resPointsDist = resDist.ToList();
        prevCenter = tree.sharedData.self.center;
        resPointsDist.Sort(ResourceDistComparer.instance);
    }

    private class ResourceDistComparer : IComparer<KeyValuePair<ResourcePoint, float>>{
        public static ResourceDistComparer instance = new ResourceDistComparer();

        public int Compare(KeyValuePair<ResourcePoint, float> x, KeyValuePair<ResourcePoint, float> y){
            return x.Value.CompareTo(y.Value);
        }
    }
}