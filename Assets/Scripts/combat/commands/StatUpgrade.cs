using UnityEngine;

[CreateAssetMenu(fileName = "Stat Upgrade", menuName = "SteelAndMagic/Upgrades/Stat Upgrade")]
public class StatUpgrade : CommandOption{
    [Header("Upgrade")]
    public EntityStat upgradeStat;

    public override bool execute(Squad squad){
        if(cost == null || !squad.player.resourceStat.canAfford(cost)) 
            return false;
        foreach(GameUnit unit in squad.getUnits()){
            unit.getStat().add(upgradeStat);
        }
        squad.player.resourceStat.subtract(cost);
        return true;
    }
}