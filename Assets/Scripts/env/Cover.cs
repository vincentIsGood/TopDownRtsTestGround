using System;
using System.Collections.Generic;
using UnityEngine;

// https://www.reddit.com/r/Unity3D/comments/7ujevc/how_to_program_a_cover_system_similar_to_an_rts/
public class Cover: MonoBehaviour{
    // What you have to do is to PRE-DEFINE where the spots are in a prefab or sth.
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

    public bool isAnySpotAvailable(){
        return occupiedCount < coverSpots.Count;
    }
}