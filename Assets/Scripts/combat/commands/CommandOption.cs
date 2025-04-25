using UnityEngine;
using UnityEngine.UI;

public abstract class CommandOption: ScriptableObject{
    [Header("Basic Info")]
    public new string name = "Unamed command";
    public string desc = "No Description";
    public ResourceStat cost;
    public Sprite image;

    /// <returns>whether the execution is successful or not</returns>
    public abstract bool execute(Squad squad);
}
