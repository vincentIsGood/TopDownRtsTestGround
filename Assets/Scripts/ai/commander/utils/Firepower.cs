using System.Collections.Generic;
using System.Linq;

public class InfluenceUtils{
    public static float ofGameTeam(GameTeam team){
        return team.team.Select(ofGamePlayer).Sum();
    }

    public static float ofGamePlayer(GamePlayer player){
        return player.squads.Select(ofSquad).Sum(); // + ofGameBuildings(player.buildings.ToList());
    }

    public static float ofGameBuildings(List<GameBuilding> buildings){
        return buildings.Select(ofUnit).Sum();
    }

    public static float ofSquads(List<Squad> squads){
        return squads.Select(ofSquad).Sum();
    }

    public static float ofSquad(Squad squad){
        return squad.getUnits().Select(ofUnit).Sum();
    }

    public static float ofUnit(GameUnit unit){
        EntityStat stat = unit.getStat();
        return stat.health + stat.damage*4 + stat.defense*2;
    }
}