using System.Collections.Generic;
using UnityEngine;

public class EnterableHouse: GameBuilding{
    public CoverSide space;
    public List<Squad> squads = new List<Squad>();
    
    private int ogLayer;

    void Awake(){
        ogLayer = gameObject.layer;
        space = GetComponentInChildren<CoverSide>();
    }
    public override void onDestroyed(){
        kickSquadsOut();
        base.onDestroyed();
    }
    private void kickSquadsOut(){
        for(int i = squads.Count-1; i >= 0; i--){
            squads[i].moveToPosReset(getEntrancePos());
        }
    }

    public bool canEnter(Squad squad){
        return space.hasEnoughSpaceFor(squad) 
            && !isDestroyed 
            && (gameObject.layer != ogLayer || gameObject.layer != squad.gameObject.layer);
    }

    public void squadEntered(Squad squad){
        squads.Add(squad);
        gameObject.layer = squad.gameObject.layer;
    }
    public void squadLeaved(Squad squad){
        squads.Remove(squad);
        if(squads.Count == 0)
            gameObject.layer = ogLayer;
    }

    public Vector3 enter(GameUnit unit){
        if(unit is Soldier soldier)
            return space.assignClosestSpot(soldier).position;
        return transform.position;
    }

    public Vector3 getEntrancePos(){
        return spawnRallyPoint.transform.position;
    }

    public bool isEmpty(){
        return squads.Count == 0;
    }
}