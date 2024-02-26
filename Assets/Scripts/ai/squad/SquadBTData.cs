using System;
using UnityEngine;

[Serializable]
public class SquadBTData{
    // predefied configs
    [Header("Masks")]
    public LayerMask enemyMask;
    public LayerMask wallMask;
    public LayerMask coverMask;

    [Header("Ranges")]
    public float findRange = 2;
    public float chaseRange = 4;
    public float attackRange = 4;

    [Header("Cooldowns")]
    public float attackSpeedSec = 1.5f;

    [Header("Formations")]
    public float separation = 2f;
    public float cohesion = 1.3f;

    [Header("Target Solver")]
    public TargetMode targetMode = TargetMode.Closest;

    // assigned in runtime
    [NonSerialized] public Squad squad;
    [NonSerialized] public Squad target;

    public void assign(Squad squad){
        this.squad = squad;
    }
}