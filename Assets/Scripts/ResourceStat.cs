using System;
using UnityEngine;

[Serializable]
public class ResourceStat{
    public float oil;
    public float iron;
    public float food;

    private float maxOil = 999999;
    private float maxIron = 999999;
    private float maxFood = 999999;

    public bool canAfford(ResourceStat amount){
        if(oil - amount.oil < 0){
            return false;
        }
        if(iron - amount.iron < 0){
            return false;
        }
        if(food - amount.food < 0){
            return false;
        }
        return true;
    }

    public void subtract(ResourceStat amount){
        oil = Mathf.Max(-maxOil, oil - amount.oil);
        iron = Mathf.Max(-maxIron, iron - amount.iron);
        food = Mathf.Max(-maxFood, food - amount.food);
    }
    public void add(ResourceStat amount){
        oil = Mathf.Min(oil + amount.oil, maxOil);
        iron = Mathf.Min(iron + amount.iron, maxIron);
        food = Mathf.Min(food + amount.food, maxFood);
    }
}

public enum ResourceType{
    Food, Iron, Oil
}