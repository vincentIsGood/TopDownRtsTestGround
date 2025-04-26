using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Barracks: GameBuilding{
    void Start(){
        spawnOptions = new List<SpawnOption>(){
            AssetDatabase.LoadAssetAtPath<SpawnOption>($"Assets/Config/Spawn/Infantry.asset"),
            // AssetDatabase.LoadAssetAtPath<SpawnOption>($"Assets/Config/Spawn/{owner.country}/Infantry.asset"),
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

    [ContextMenu("Spawn/Infantry")]
    public Squad spawnInfantry(){
        return spawnSquad(spawnOptions[0]);
    }
}