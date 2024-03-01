using System;

[Serializable]
public class ResourceStat{
    public float oil;
    public float iron;
    public float food;
    
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
        oil -= amount.oil;
        iron -= amount.iron;
        food -= amount.food;
    }
    public void add(ResourceStat amount){
        oil += amount.oil;
        iron += amount.iron;
        food += amount.food;
    }
}