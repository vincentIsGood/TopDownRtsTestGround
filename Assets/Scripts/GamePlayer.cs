using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePlayer: MonoBehaviour{
    public string playerName;
    public bool isAlly = true;
    [NonSerialized] public GameTeam ownTeam;
    [NonSerialized] public GameTeam enemyTeam;

    public CommanderBTData commanderConfig = new CommanderBTData();
    public ResourceStat resourceStat = new ResourceStat();

    [NonSerialized] public Vector3 center;
    [NonSerialized] public HashSet<GameBuilding> buildings = new HashSet<GameBuilding>();
    [NonSerialized] public List<Squad> squads = new List<Squad>();
    [NonSerialized] public SquadFormationSolver formationSolver = new SquadFormationSolver();
    [NonSerialized] public RtsController controller;

    void Awake(){
        commanderConfig.assign(this);
    }

    void Update(){
        center = buildingsCenter();
    }

    public void addSquad(Squad squad){
        squads.Add(squad);
    }
    public void removeSquad(Squad squad){
        squads.Remove(squad);
        controller?.clearSelectedSquadsAndPos();
    }

    public void addBuilding(GameBuilding building){
        buildings.Add(building);
    }
    public void removeBuilding(GameBuilding building){
        buildings.Remove(building);
    }

    public Vector3 buildingsCenter(){
        Vector3 center = Vector3.zero;
        if(buildings.Count == 0) return center;

        foreach(GameBuilding building in buildings){
            if(building.isDestroyed) continue;
            center += building.transform.position;
        }
        return center /= buildings.Count;
    }

    public List<Squad> findSquadsNearby(Vector3 pos, float radius){
        List<Squad> result = new List<Squad>();
        foreach(Squad squad in squads){
            if(Vector3.Distance(pos, squad.center) < radius){
                result.Add(squad);
            }
        }
        return result;
    }
    public List<Squad> findSquadsNearbyByDestination(Vector3 pos, float radius){
        List<Squad> result = new List<Squad>();
        foreach(Squad squad in squads){
            if(Vector3.Distance(pos, squad.destCenter) < radius){
                result.Add(squad);
            }
        }
        return result;
    }
}