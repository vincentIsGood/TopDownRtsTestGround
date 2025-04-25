using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SquadBTData{
    [Header("Upgrades")]
    public List<CommandOption> squadUpgrades = new List<CommandOption>();

    [Header("Masks")]
    public LayerMask enemyMask;
    public LayerMask wallMask;
    public LayerMask coverMask;

    [Header("Ranges")]
    [Tooltip("Enter radius is used when entering buildings")]
    public float enterRadius = 1.5f;
    [Tooltip("Used in finding covers (not implemented)")]
    public float findRange = 4;
    [Tooltip("How far it go while chasing (not implemented)")]
    public float chaseRange = 2;
    public float attackRange = 8;

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