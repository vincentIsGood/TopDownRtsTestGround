using UnityEditor;
using UnityEngine;

public class GOLocatorUtils: MonoBehaviour{
    
#if UNITY_EDITOR
    void OnDrawGizmos(){
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, Vector3.forward, 0.1f);
    }
#endif
}