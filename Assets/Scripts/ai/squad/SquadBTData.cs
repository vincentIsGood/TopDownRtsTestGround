using System;
using UnityEngine;

[Serializable]
public class SquadBTData{
    // predefied configs
    public LayerMask enemyMask;
    public LayerMask wallMask;

    public float findRange = 2;
    public float chaseRange = 2;
    public float attackRange = 4;

    public float attackSpeedSec = 1.5f;

    // assigned in runtime
    [NonSerialized] public Squad squad;
    [NonSerialized] public Squad target;

    public void init(Squad squad){
        this.squad = squad;
    }
}