using System.Collections.Generic;
using System.Linq;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class EvaluateZoneInfluences: BehaviorTreeNode<CommanderBTData>{
    private IntervalActionUtils resetter;
    private IntervalActionUtils evaluator;
    private IntervalActionUtils squadZoneEvaluator;

    public override NodeState evaluate(){
        if(resetter == null) resetter = new IntervalActionUtils(zoneReset, 3);
        if(evaluator == null) evaluator = new IntervalActionUtils(evaluateAllZones, 3);
        if(squadZoneEvaluator == null) squadZoneEvaluator = new IntervalActionUtils(evaluateSquadZones, 3);
        resetter.tick();
        evaluator.tick();
        squadZoneEvaluator.tick();
        return NodeState.SUCCESS;
    }

    private void evaluateAllZones(){
        foreach(ResourcePoint resource in GameMap.instance.resourcePoints){
            EnemyAllyInfluence zoneInfluence = evaluateZoneInfluence(resource.transform.position);
            EnemyAllyInfluence potentialZoneInfluence = evaluatePotentialZoneInfluence(resource.transform.position);
            zoneInfluence.name = resource.name;
            potentialZoneInfluence.name = resource.name;
        }
        foreach(GameBuilding building in GameMap.instance.allyTeam.findBuildings()){
            EnemyAllyInfluence zoneInfluence = evaluateZoneInfluence(building.transform.position);
            EnemyAllyInfluence potentialZoneInfluence = evaluatePotentialZoneInfluence(building.transform.position);
            zoneInfluence.name = building.name;
            potentialZoneInfluence.name = building.name;
        }
        foreach(GameBuilding building in GameMap.instance.enemyTeam.findBuildings()){
            EnemyAllyInfluence zoneInfluence = evaluateZoneInfluence(building.transform.position);
            EnemyAllyInfluence potentialZoneInfluence = evaluatePotentialZoneInfluence(building.transform.position);
            zoneInfluence.name = building.name;
            potentialZoneInfluence.name = building.name;
        }
    }
    private void evaluateSquadZones(){
        foreach(Squad squad in GameMap.instance.allyTeam.team.SelectMany(t => t.squads)){
            EnemyAllyInfluence zoneInfluence = evaluateZoneInfluence(squad.center);
            EnemyAllyInfluence potentialZoneInfluence = evaluatePotentialZoneInfluence(squad.center);
            zoneInfluence.name = squad.name;
            potentialZoneInfluence.name = squad.name;
        }
    }

    private EnemyAllyInfluence evaluateZoneInfluence(Vector3 zonePos){
        CommanderBTData aiState = tree.sharedData;
        List<Squad> allySquads = aiState.self.findSquadsNearby(zonePos, aiState.zoneCalcRadius);
        List<Squad> enemySquads = aiState.self.enemyTeam.findSquadsNearby(zonePos, aiState.zoneCalcRadius);
        List<GameBuilding> enemyBuildings = aiState.self.enemyTeam.findBuildingsNearby(zonePos, aiState.zoneCalcRadius);
        float allyInfluence = InfluenceUtils.ofSquads(allySquads);
        float enemyInfluence = InfluenceUtils.ofSquads(enemySquads) + InfluenceUtils.ofGameBuildings(enemyBuildings);
        EnemyAllyInfluence zoneInfluence = new EnemyAllyInfluence(
            allyInfluence,
            enemyInfluence
        );
        aiState.zoneInfluences[zonePos] = zoneInfluence;
        return zoneInfluence;
    }
    private EnemyAllyInfluence evaluatePotentialZoneInfluence(Vector3 zonePos){
        CommanderBTData aiState = tree.sharedData;
        List<Squad> allySquads = aiState.self.findSquadsNearbyByDestination(zonePos, aiState.zoneCalcRadius);
        List<Squad> enemySquads = aiState.self.enemyTeam.findSquadsNearby(zonePos, aiState.zoneCalcRadius);
        List<GameBuilding> enemyBuildings = aiState.self.enemyTeam.findBuildingsNearby(zonePos, aiState.zoneCalcRadius);
        float allyInfluence = InfluenceUtils.ofSquads(allySquads);
        float enemyInfluence = InfluenceUtils.ofSquads(enemySquads) + InfluenceUtils.ofGameBuildings(enemyBuildings);
        EnemyAllyInfluence zoneInfluence = new EnemyAllyInfluence(
            allyInfluence,
            enemyInfluence
        );
        aiState.potentialZoneInfluences[zonePos] = zoneInfluence;
        return zoneInfluence;
    }

    private void zoneReset(){
        tree.sharedData.zoneInfluences.Clear();
        tree.sharedData.potentialZoneInfluences.Clear();
    }
}