using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugMsgPanel: MonoBehaviour{
    public static DebugMsgPanel instance;

    public TextMeshProUGUI text;

    void Awake(){
        if(instance == null) instance = this;
    }

    public void showStats(GamePlayer player, GamePlayer enemy){
        string finalText = 
$@"Player Resources:
Oil: {player.resourceStat.oil}
Iron: {player.resourceStat.iron}
Food: {player.resourceStat.food}
Squads: {player.squads.Count}/{GameMap.instance.maxSquads}

Enemy Resources:
Oil: {enemy.resourceStat.oil}
Iron: {enemy.resourceStat.iron}
Food: {enemy.resourceStat.food}
Squads: {enemy.squads.Count}/{GameMap.instance.maxSquads}";

        text.text = finalText;
    }
}