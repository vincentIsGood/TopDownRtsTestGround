using UnityEngine;

public class RandomUtils{
    public static float valueWithNeg{
        get{
            return Random.Range(-1f, 1f);
        }
    }

    public static Vector3 random(){
        return new Vector3(Random.value, Random.value, Random.value);
    }
    public static Vector3 random(float scaleX, float scaleY, float scaleZ){
        return new Vector3(Random.value * scaleX, Random.value * scaleY, Random.value * scaleZ);
    }
    public static Vector3 random(float scale){
        return new Vector3(Random.value * scale, Random.value * scale, Random.value * scale);
    }
    
    public static Vector3 randomWithNeg(){
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }
    public static Vector3 randomWithNeg(float scaleX, float scaleY, float scaleZ){
        return new Vector3(Random.Range(-1f, 1f) * scaleX, Random.Range(-1f, 1f) * scaleY, Random.Range(-1f, 1f) * scaleZ);
    }
    public static Vector3 randomWithNeg(float scale){
        return new Vector3(Random.Range(-1f, 1f) * scale, Random.Range(-1f, 1f) * scale, Random.Range(-1f, 1f) * scale);
    }
}