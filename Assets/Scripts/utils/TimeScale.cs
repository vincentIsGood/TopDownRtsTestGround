using UnityEngine;

public class TimeScale : MonoBehaviour{
    public static TimeScale instance;
    void Awake(){if(instance == null) instance = this;}

    public void pause(){
        if(Time.timeScale == 0){
            Time.timeScale = 1;
        }else{
            Time.timeScale = 0;
        }
    }

    public void speedup(){
        if(Time.timeScale == 2){
            Time.timeScale = 1;
        }else{
            Time.timeScale = 2;
        }
    }
}
