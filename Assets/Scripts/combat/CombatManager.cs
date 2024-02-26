public class CombatManager{
    private Soldier owner;

    public CombatManager(Soldier owner){
        this.owner = owner;
    }

    public void attack(Soldier enemy){
        EntityStat ownerStat = owner.stat;
        EntityStat enemyStat = enemy.stat;

        enemyStat.takeDamage(ownerStat.calculateDamage());

        if(enemyStat.health <= 0){
            enemy.onDie();
            owner.onEnemyKilled(enemy);
        }
    }
}