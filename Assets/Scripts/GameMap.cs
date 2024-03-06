using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMap: MonoBehaviour{
    public static GameMap instance;

    [NonSerialized] public ResourcePoint[] resourcePoints;

    void Awake(){
        if(instance == null) instance = this;

        GameObject resourcePointCollection = SceneManager.GetActiveScene()
            .GetRootGameObjects().First(ele => ele.name == "ResourcePoints");
        resourcePoints = resourcePointCollection.GetComponentsInChildren<ResourcePoint>();
    }
}