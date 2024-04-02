using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SquadFormationSolver{
    public float separation = 2f;
    public float cohesion = 1.3f;

    private Dictionary<Squad, Vector3> squadLocalPos = new Dictionary<Squad, Vector3>();

    public Vector3 getNoFormationPos(Squad squad){
        return squadLocalPos[squad];
    }

    public void updateMembersLocalPos(List<Squad> selectedSquads){
        foreach(Squad squad in selectedSquads){
            squadLocalPos[squad] = calcLocalPos(squad, selectedSquads, center(selectedSquads.Select(squad => squad.center).ToList()));
        }
    }

    public Vector3 calcLocalPos(Squad squad, List<Squad> selectedSquads, Vector3 center){
        if(selectedSquads.Count == 1){
            return Vector3.zero;
        }

        Vector3 separationForce = Vector3.zero;
        foreach(Squad other in selectedSquads){
            float dist = Vector3.Distance(squad.center, other.center);
            if(dist != 0){
                Vector3 diff = squad.center - other.center;
                separationForce += diff.normalized / dist;
            }
        }
        if(selectedSquads.Count-1 > 0){
            separationForce /= selectedSquads.Count-1;
        }

        Vector3 cohesionForce = center - squad.center;
        return separation * separationForce.normalized + cohesion * cohesionForce.normalized;
    }

    public void clearLocalPos(){
        squadLocalPos.Clear();
    }

    public static Vector3 center(List<Vector3> vects){

        Vector3 result = Vector3.zero;
        foreach(Vector3 vect in vects){
            result += vect;
        }
        return result / vects.Count;
    }
}