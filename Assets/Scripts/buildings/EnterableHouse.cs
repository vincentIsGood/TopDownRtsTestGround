using UnityEngine;

public class EnterableHouse: GameBuilding{
    public CoverSide space;

    void Awake(){
        space = GetComponentInChildren<CoverSide>();
    }

    public bool hasEnoughSpaceFor(Squad squad){
        return space.hasEnoughSpaceFor(squad);
    }

    public Vector3 enter(GameUnit unit){
        if(unit is Soldier soldier)
            return space.assignClosestSpot(soldier).position;
        return transform.position;
    }

    public Vector3 getEntrancePos(){
        return spawnRallyPoint.transform.position;
    }
}