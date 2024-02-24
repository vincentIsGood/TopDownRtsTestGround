using System;
using System.Collections.Generic;
using UnityEngine;

public class Cover: MonoBehaviour{
    public CoverSide[] coverSides;

    void Start(){
        coverSides = GetComponentsInChildren<CoverSide>();
    }

    public CoverSide findClosestSide(Squad squad, float withinDistance = -1){
        CoverSide result = null;
        float shortestDist = float.MaxValue;
        foreach(CoverSide coverSide in coverSides){
            float dist = Vector3.Distance(coverSide.findCenter(), squad.center);
            if(dist < shortestDist){
                shortestDist = dist;
                result = coverSide;
            }
        }
        return result;
    }
    public Transform assignClosestSpot(Soldier soldier, float withinDistance = -1){
        CoverSide result = null;
        float shortestDist = float.MaxValue;
        foreach(CoverSide coverSide in coverSides){
            float dist = Vector3.Distance(coverSide.findCenter(), soldier.transform.position);
            if(dist < shortestDist){
                shortestDist = dist;
                result = coverSide;
            }
        }
        return result?.assignClosestSpot(soldier, withinDistance);
    }
}