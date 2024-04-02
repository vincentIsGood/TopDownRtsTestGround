using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GameTeam{
    public List<GamePlayer> team = new List<GamePlayer>();

    public List<Squad> findSquadsNearby(Vector3 pos, float radius){
        return team.SelectMany(t => t.findSquadsNearby(pos, radius)).ToList();
    }
    public List<Squad> findSquadsNearbyByDestination(Vector3 pos, float radius){
        return team.SelectMany(t => t.findSquadsNearbyByDestination(pos, radius)).ToList();
    }

    public List<GameBuilding> findBuildings(){
        return team.SelectMany(t => t.buildings).ToList();
    }

    public List<GameBuilding> findBuildingsNearby(Vector3 pos, float radius){
        return findBuildings().Where(b => Vector3.Distance(pos, b.transform.position) < radius).ToList();
    }

    public void Add(GamePlayer p){
        team.Add(p);
    }

    public bool Contains(GamePlayer p){
        return team.Contains(p);
    }
}