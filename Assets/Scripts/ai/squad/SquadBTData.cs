using System;
using UnityEngine;

[Serializable]
public class SquadBTData{

    [Header("Masks")]
    public LayerMask enemyMask;
    public LayerMask wallMask;
    public LayerMask coverMask;

    [Header("Ranges")]
    public float enterRadius = 1.5f;
    public float findRange = 4;
    public float chaseRange = 2;
    public float attackRange = 8;

    [Header("Cooldowns")]
    public float attackSpeedSec = 1.5f;

    [Header("Formations")]
    public float separation = 2f;
    public float cohesion = 1.3f;

    [Header("Target Solver")]
    public TargetMode targetMode = TargetMode.Closest;

    [Header("AI")]
    public bool enableAi = true;
    public bool allowAiMovement = true;

    [NonSerialized] public Squad squad;
    public Squad target;
    public GameUnit targetUnit;
    public Cover moveToCover;

    public void assign(Squad squad){
        this.squad = squad;
    }
}