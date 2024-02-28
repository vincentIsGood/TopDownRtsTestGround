public class CombatManager{
    private GameUnit owner;

    public CombatManager(GameUnit owner){
        this.owner = owner;
    }

    public void attack(GameUnit enemy){
        EntityStat ownerStat = owner.getStat();
        EntityStat enemyStat = enemy.getStat();

        enemyStat.takeDamage(ownerStat.calculateDamage());

        if(enemyStat.health <= 0){
            enemy.onDie();
            owner.onEnemyKilled(enemy);
        }
    }
}