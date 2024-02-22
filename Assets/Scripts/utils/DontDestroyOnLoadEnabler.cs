using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class DontDestroyOnLoadEnabler: MonoBehaviour{
    public GameObject singletonObject;
    public string idTag = "singletons";

    void Awake(){
        GameObject[] objs = GameObject.FindGameObjectsWithTag(idTag);
        if(objs.Length > 1){
            Destroy(this.gameObject);
            return;
        }

        if(singletonObject == null) singletonObject = this.gameObject;
        singletonObject.tag = idTag;
        DontDestroyOnLoad(singletonObject);
    }
}