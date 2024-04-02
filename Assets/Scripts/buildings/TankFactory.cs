using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TankFactory: GameBuilding{
    void Awake(){
        spawnOptions = new List<SpawnOption>(){
            AssetDatabase.LoadAssetAtPath<SpawnOption>("Assets/Config/Spawn/TankSquad.asset"),
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
    
    [ContextMenu("Spawn/Tank")]
    public Squad spawnTank(){
        return spawnSquad(spawnOptions[0]);
    }
}