using UnityEngine.UI;

public class BuildingSpawnOption : CommandOption{
    private GameBuilding gameBuilding;
    private SpawnOption spawnOption;

    public void init(GameBuilding gameBuilding, SpawnOption spawnOption){
        this.gameBuilding = gameBuilding;
        this.spawnOption = spawnOption;
        name = spawnOption.name;
        desc = spawnOption.desc;
        cost = spawnOption.cost;
        image = spawnOption.icon;
    }

    public override bool execute(Squad squad){
        if(gameBuilding is Barracks || gameBuilding is TankFactory){
            gameBuilding.spawnSquad(spawnOption);
            return true;
        }
        return false;
    }
}
