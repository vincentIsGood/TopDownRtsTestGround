using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FormationSolver{
    private Squad ownSquad;
    private Dictionary<Soldier, Vector3> soldierSquadLocalPos = new Dictionary<Soldier, Vector3>();

    public FormationSolver(Squad ownSquad){
        this.ownSquad = ownSquad;
    }

    // return local pos
    public Vector3 getNoFormationPos(Soldier member){
        return soldierSquadLocalPos[member];
    }

    // return abs pos
    public Vector3 getFormationPos(Soldier member, Cover cover, Vector3 coverPos = new Vector3()){
        Transform spot = cover.assignClosestSpot(member);
        if(spot != null){
            return spot.position;
        }
        return coverPos + getNoFormationPos(member);
    }
    public Vector3 getFormationPos(Soldier member, CoverSide cover, Vector3 coverPos = new Vector3()){
        Transform spot = cover.assignClosestSpot(member);
        if(spot != null){
            return spot.position;
        }
        return coverPos + getNoFormationPos(member);
    }
    
    public void updateMembersLocalPos(){
        foreach(Soldier member in ownSquad.getSoldiers()){
            soldierSquadLocalPos[member] = calcLocalPos(member, ownSquad.center);
        }
    }

    public Vector3 calcLocalPos(Soldier member, Vector3 squadCenter){
        // separation
        Vector3 separationForce = Vector3.zero;
        foreach(Soldier other in ownSquad.getSoldiers()){
            float dist = Vector3.Distance(member.transform.position, other.transform.position);
            if(dist != 0){
                Vector3 diff = member.transform.position - other.transform.position;
                separationForce += diff.normalized / dist;
            }
        }
        if(ownSquad.getSoldiers().Length-1 > 0){
            separationForce /= ownSquad.getSoldiers().Length-1;
        }

        // cohesion
        Vector3 cohesionForce = squadCenter - member.transform.position;
        return ownSquad.config.separation * separationForce.normalized + ownSquad.config.cohesion * cohesionForce.normalized;
    }

    public static Vector3 center(Vector3[] vects){
        // avg pos
        Vector3 result = Vector3.zero;
        foreach(Vector3 vect in vects){
            result += vect;
        }
        return result / vects.Length;
    }
}