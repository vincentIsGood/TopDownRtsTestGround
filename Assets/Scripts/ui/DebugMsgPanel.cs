using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugMsgPanel: MonoBehaviour{
    public static DebugMsgPanel instance;

    public TextMeshProUGUI text;

    void Awake(){
        if(instance == null) instance = this;
    }

    public void showStats(ResourceStat player, ResourceStat enemy){
        string finalText = 
$@"Player Resources:
Oil: {player.oil}
Iron: {player.iron}
Food: {player.food}

Enemy Resources:
Oil: {enemy.oil}
Iron: {enemy.iron}
Food: {enemy.food}";
        text.text = finalText;
    }
}