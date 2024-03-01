using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Barracks: GameBuilding{
    void Awake(){
        spawnOptions = new List<SpawnOption>(){
            AssetDatabase.LoadAssetAtPath<SpawnOption>("Assets/Config/Spawn/Infantry.asset"),
            AssetDatabase.LoadAssetAtPath<SpawnOption>("Assets/Config/Spawn/TankSquad.asset"),
        };
    }

    [ContextMenu("Player as Owner")]
    public void playerOwner(){
        owner = RtsController.instance.player;
    }

    [ContextMenu("Enemy as Owner")]
    public void enemyOwner(){
        owner = RtsController.instance.enemy;
    }

    [ContextMenu("Spawn/Infantry")]
    public void spawnInfantry(){
        spawnSquad(spawnOptions[0]);
    }
    
    [ContextMenu("Spawn/Tank")]
    public void spawnTank(){
        spawnSquad(spawnOptions[1]);
    }
}