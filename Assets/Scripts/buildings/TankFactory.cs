using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TankFactory: GameBuilding{
    void Start(){
        spawnOptions = new List<SpawnOption>(){
            AssetDatabase.LoadAssetAtPath<SpawnOption>($"Assets/Config/Spawn/{owner.country}/LightTankSquad.asset"),
            AssetDatabase.LoadAssetAtPath<SpawnOption>($"Assets/Config/Spawn/{owner.country}/HeavyTankSquad.asset"),
        };
    }

    public override void onDestroyed(){
        owner.removeBuilding(this);
        Destroy(gameObject);
        base.onDestroyed();
    }

    [ContextMenu("Player as Owner")]
    public void playerOwner(){
        owner = RtsController.instance.player;
        owner.addBuilding(this);
    }

    [ContextMenu("Enemy as Owner")]
    public void enemyOwner(){
        owner = EnemyController.instance.ai;
        owner.addBuilding(this);
    }
    
    [ContextMenu("Spawn/Light Tank")]
    public Squad spawnLightTank(){
        return spawnSquad(spawnOptions[0]);
    }
    
    [ContextMenu("Spawn/Heavy Tank")]
    public Squad spawnHeavyTank(){
        return spawnSquad(spawnOptions[1]);
    }
}