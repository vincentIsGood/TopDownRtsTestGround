using System;
using UnityEngine;

[Serializable]
public class SquadBTData{
    // predefied configs
    [Header("Masks")]
    public LayerMask enemyMask;
    public LayerMask wallMask;

    [Header("Ranges")]
    public float findRange = 2;
    public float chaseRange = 2;
    public float attackRange = 4;

    [Header("Cooldowns")]
    public float attackSpeedSec = 1.5f;

    [Header("Formations")]
    [NonSerialized] public float separation = 2f;
    [NonSerialized] public float cohesion = 1;

    [Header("Target Solver")]
    public TargetMode targetMode = TargetMode.Closest;

    // assigned in runtime
    [NonSerialized] public Squad squad;
    [NonSerialized] public Squad target;

    public void init(Squad squad){
        this.squad = squad;
    }
}