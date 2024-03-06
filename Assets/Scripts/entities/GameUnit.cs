using UnityEngine;

public interface GameUnit{
    
    public void onDie();
    public void onEnemyKilled(GameUnit enemy);
    public void onBuildingDestroyed(GameBuilding building);

    public void moveToPos(Vector3 pos);
    public void teleportToPos(Vector3 pos);
    public void attackOnSight(GameUnit target);
    public void attackAndMove(GameUnit target);

    public void stopAtFireDistance();
    public void moveCloseToPos();
    public void resetStoppingDistance();

    public Vector3 getHeadingToPos();
    public GameUnit getTarget();
    public void setTarget(GameUnit target);
    public Squad getOwnSquad();
    
    public AiNavigator getAgent();
    public EntityStat getStat();
    public CombatManager getCombatManager();

    public GameObject getOwner();
    public Transform getTransform();

    public bool isDead();

}