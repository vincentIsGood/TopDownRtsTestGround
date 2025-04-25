using System.Linq;

public class CombatManager{
    private GameUnit attacker;

    public CombatManager(GameUnit owner){
        this.attacker = owner;
    }

    public void attack(GameUnit enemy){
        EntityStat attackerStat = attacker.getStat();
        EntityStat enemyStat = enemy.getStat();

        enemyStat.takeDamage(attackerStat.damage);
        enemy.getOwnSquad().onAnyUnitTakeDamage(enemy);

        if(enemyStat.health <= 0){
            enemy.onDie();
            attacker.onEnemyKilled(enemy);
        }
    }
    public void attack(GameBuilding building){
        EntityStat attackerStat = attacker.getStat();
        EntityStat buildingStat = building.stat;

        buildingStat.takeDamage(attackerStat.damage);
        if(building is EnterableHouse house && !house.isDestroyed && !house.isEmpty()){
            Squad squad = RandomUtils.randomElement(house.squads);
            Soldier enemy = (Soldier)RandomUtils.randomElement(squad.getUnits());
            enemy.stat.takeDamage(attackerStat.damage, building.reductionStatScaler);
            enemy.getOwnSquad().onAnyUnitTakeDamage(enemy);
            if(enemy.stat.health <= 0){
                enemy.onDie();
                attacker.onEnemyKilled(enemy);
            }
        }

        if(buildingStat.health <= 0){
            building.onDestroyed();
            attacker.onBuildingDestroyed(building);
            GameMap.instance.onBuildingDestroyed(building);
        }
    }
}
