using UnityEngine;

public class CopySquadLocationUtils: MonoBehaviour{
    public Squad copyFrom;

    private Vector3 initOffset;

    void Start(){
        initOffset = transform.localPosition;
    }

    void Update(){
        transform.position = copyFrom.center + initOffset;
    }
}