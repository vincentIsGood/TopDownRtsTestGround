using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Option", menuName = "SteelAndMagic/Spawn Option")]
public class SpawnOption: ScriptableObject{
    public ResourceStat cost;
    public GameObject squadPrefab;
}