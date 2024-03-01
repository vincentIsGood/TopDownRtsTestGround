using System;
using System.Collections.Generic;
using UnityEngine;


public class CoverSide: MonoBehaviour{

    public List<Transform> coverSpots = new List<Transform>();
    public Soldier[] occupiedSpots;
    private int occupiedCount = 0;

    void Start(){
        foreach(Transform transform in transform){
            if(transform.tag == "coverspot"){
                coverSpots.Add(transform);
            }
        }
        occupiedSpots = new Soldier[coverSpots.Count];
    }

    public Transform assignClosestSpot(Soldier soldier, float withinDistance = -1){
        if(!isAnySpotAvailable()) return null;

        int closestSpotIndex = -1;
        float shortestDist = float.MaxValue;
        for(int i = 0; i < coverSpots.Count; i++){
            Transform spot = coverSpots[i];
            float dist = Vector3.Distance(soldier.transform.position, spot.position);
            if(withinDistance > 0 && dist > withinDistance || occupiedSpots[i] != null)
                continue;
            if(dist < shortestDist){
                shortestDist = dist;
                closestSpotIndex = i;
            }
        }
        if(closestSpotIndex == -1) return null;

        occupiedSpots[closestSpotIndex] = soldier;
        occupiedCount++;
        return coverSpots[closestSpotIndex];
    }

    public void leaveSpot(Soldier soldier){
        int index = Array.IndexOf(occupiedSpots, soldier);
        if(index == -1) return;

        soldier.resetStoppingDistance();
        occupiedSpots[index] = null;
        occupiedCount = Math.Max(occupiedCount - 1, 0);
    }

    public void clearSpot(){
        foreach(Soldier soldier in occupiedSpots){
            if(!soldier) leaveSpot(soldier);
        }
    }

    public bool hasEnoughSpaceFor(Squad squad){
        return squad.getUnits().Count + occupiedCount <= coverSpots.Count;
    }

    public bool isAnySpotAvailable(){
        return occupiedCount < coverSpots.Count;
    }

    public Vector3 findCenter(){
        Vector3 result = Vector3.zero;
        foreach(Transform spots in coverSpots){
            result += spots.position;
        }
        return result / coverSpots.Count;
    }
}