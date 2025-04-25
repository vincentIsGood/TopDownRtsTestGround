using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Option", menuName = "SteelAndMagic/Spawn Option")]
public class SpawnOption: ScriptableObject{
    public new string name = "Unamed Unit";
    public string desc = "No Description";
    public ResourceStat cost;
    public GameObject squadPrefab;
    public Sprite icon;
}