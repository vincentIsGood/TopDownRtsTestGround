using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FormationSolver{
    private Squad ownSquad;
    private Dictionary<GameUnit, Vector3> soldierSquadLocalPos = new Dictionary<GameUnit, Vector3>();

    public FormationSolver(Squad ownSquad){
        this.ownSquad = ownSquad;
    }


    public Vector3 getNoFormationPos(GameUnit member){
        return soldierSquadLocalPos[member];
    }


    public Vector3 getFormationPos(GameUnit member, Cover cover, Vector3 coverPos = new Vector3()){
        if(member is Soldier soldier){
            Transform spot = cover.assignClosestSpot(soldier);
            if(spot != null){
                return spot.position;
            }
        }
        return coverPos + getNoFormationPos(member);
    }
    public Vector3 getFormationPos(GameUnit member, CoverSide cover, Vector3 coverPos = new Vector3()){
        if(member is Soldier soldier){
            Transform spot = cover.assignClosestSpot(soldier);
            if(spot != null){
                return spot.position;
            }
        }
        return coverPos + getNoFormationPos(member);
    }
    
    public void updateMembersLocalPos(){
        foreach(GameUnit member in ownSquad.getUnits()){
            soldierSquadLocalPos[member] = calcLocalPos(member, ownSquad.center);
        }
    }

    public Vector3 calcLocalPos(GameUnit member, Vector3 squadCenter){
        List<GameUnit> units = ownSquad.getUnits();
        if(units.Count == 1){
            return Vector3.zero;
        }

        Vector3 separationForce = Vector3.zero;
        foreach(GameUnit other in units){
            float dist = Vector3.Distance(member.getTransform().position, other.getTransform().position);
            if(dist != 0){
                Vector3 diff = member.getTransform().position - other.getTransform().position;
                separationForce += diff.normalized / dist;
            }
        }
        if(units.Count-1 > 0){
            separationForce /= units.Count-1;
        }


        Vector3 cohesionForce = squadCenter - member.getTransform().position;
        return ownSquad.config.separation * separationForce.normalized + ownSquad.config.cohesion * cohesionForce.normalized;
    }

    public static Vector3 center(Vector3[] vects){

        Vector3 result = Vector3.zero;
        foreach(Vector3 vect in vects){
            result += vect;
        }
        return result / vects.Length;
    }
}