using UnityEngine;

public class CopyLocationUtils: MonoBehaviour{
    public GameObject copyFrom;

    private Vector3 initOffset;

    void Start(){
        initOffset = transform.localPosition;
    }

    void Update(){
        transform.position = copyFrom.transform.position + initOffset;
    }
}