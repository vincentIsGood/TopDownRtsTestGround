using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMap: MonoBehaviour{
    public static GameMap instance;

    public int maxSquads = 30;

    [NonSerialized] public GameTeam allyTeam = new GameTeam();
    [NonSerialized] public GameTeam enemyTeam = new GameTeam();
    [NonSerialized] public ResourcePoint[] resourcePoints;

    void Awake(){
        if(instance == null) instance = this;

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("PlayerController")){
            GamePlayer player = go.GetComponent<GamePlayer>();
            player.ownTeam = allyTeam;
            player.enemyTeam = enemyTeam;
            allyTeam.Add(player);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("AllyController")){
            GamePlayer player = go.GetComponent<GamePlayer>();
            player.ownTeam = allyTeam;
            player.enemyTeam = enemyTeam;
            allyTeam.Add(player);
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("EnemyController")){
            GamePlayer player = go.GetComponent<GamePlayer>();
            player.ownTeam = enemyTeam;
            player.enemyTeam = allyTeam;
            enemyTeam.Add(player);
        }

        GameObject resourcePointCollection = SceneManager.GetActiveScene()
            .GetRootGameObjects().First(ele => ele.name == "ResourcePoints");
        resourcePoints = resourcePointCollection.GetComponentsInChildren<ResourcePoint>();
    }
}