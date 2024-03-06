using System.Collections.Generic;
using UnityEngine;

public class ArrayUtils{
    public static List<Transform> enumerate(Transform transform){
        List<Transform> children = new List<Transform>();
        foreach(Transform t in transform){
            children.Add(t);
        }
        return children;
    }
}