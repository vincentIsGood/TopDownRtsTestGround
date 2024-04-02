using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CommanderBTData{
    [Header("Resources")]
    public ResourceStat maintainResource;

    [Header("AI")]
    public bool enableAi = true;
    public float zoneCalcRadius = 5;

    public float idealInfluence = 5000;
    public float selectedSquadSumInfluence = 2000;
    public float fearInfluence = 2000;
    public float braveryProb = 0.05f;

    [Header("Dont Touch")]
    [Header("Command")]
    public Vector3 moveToTargetPos;
    public EnemyAllyInfluence moveToTargetPosInfluence;

    [NonSerialized] public GamePlayer self;
    public SerializableDictionary<Vector3, EnemyAllyInfluence> zoneInfluences = new SerializableDictionary<Vector3, EnemyAllyInfluence>();
    public SerializableDictionary<Vector3, EnemyAllyInfluence> potentialZoneInfluences = new SerializableDictionary<Vector3, EnemyAllyInfluence>();
    public SerializableDictionary<ResourceType, ResourcePoint> resourcesInNeed = new SerializableDictionary<ResourceType, ResourcePoint>();

    [NonSerialized] public Barracks barracks;
    [NonSerialized] public TankFactory tankFactory;
    [NonSerialized] public HashSet<Squad> selectedSquads = new HashSet<Squad>();

    public void assign(GamePlayer self){
        this.self = self;
    }
}

[Serializable]
public class EnemyAllyInfluence{
    public string name;

    public float ally;
    public float enemy;

    /// <summary>
    /// -ve => enemy has high influence
    /// +ve => enemy has high influence
    /// </summary>
    public float diff;

    public EnemyAllyInfluence(float ally, float enemy){
        this.ally = ally;
        this.enemy = enemy;
        diff = ally - enemy;
    }
}