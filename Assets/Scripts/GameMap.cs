using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMap: MonoBehaviour{
    public static GameMap instance;

    public int maxSquads = 30;
    public ResourceStat eachPlayerEarningsPerTime;

    [NonSerialized] public GameTeam allyTeam = new GameTeam();
    [NonSerialized] public GameTeam enemyTeam = new GameTeam();
    [NonSerialized] public ResourcePoint[] resourcePoints;

    private IntervalActionUtils resGenerationCounter;

    void Awake(){
        if(instance == null) instance = this;

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("PlayerController")){
            GamePlayer player = go.GetComponent<GamePlayer>();
            if(GameStartOption.instance){
                player.country = GameStartOption.instance.selectedCountry;
            }
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
        
        resGenerationCounter = new IntervalActionUtils(giveResourceToCapturer, 10);
    }

    void Update(){
        resGenerationCounter.tick();
    }

    public void onBuildingDestroyed(GameBuilding building){
        if(building.owner == null) return;
        if(building.owner.buildings.Count == 0){
            onGameEnds();
            return;
        }
    }

    public void onGameEnds(){
        TimeScale.instance.pause();
        SceneManager.LoadScene("MainMenu");
    }

    private void giveResourceToCapturer(){
        foreach(GamePlayer player in allyTeam.team)
            player.resourceStat.add(eachPlayerEarningsPerTime);
        foreach(GamePlayer player in enemyTeam.team)
            player.resourceStat.add(eachPlayerEarningsPerTime);
    }
}