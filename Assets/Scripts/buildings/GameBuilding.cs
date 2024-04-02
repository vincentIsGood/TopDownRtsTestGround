using System;
using System.Collections.Generic;
using UnityEngine;

public class GameBuilding: MonoBehaviour, GameUnit{
    [Header("Stats")]
    public EntityStat stat;
    public EntityStat reductionStatScaler;
    public GameObject spawnRallyPoint;
    public bool isDestroyed = false;

    [Header("Production")]
    public List<SpawnOption> spawnOptions = new List<SpawnOption>();

    [NonSerialized] public GamePlayer owner;

    public virtual void onDestroyed(){
        isDestroyed = true;
    }

    public Squad spawnSquad(SpawnOption option){
        if(!owner.resourceStat.canAfford(option.cost) || owner.squads.Count +1 > GameMap.instance.maxSquads){
            return null;
        }
        owner.resourceStat.subtract(option.cost);
        
        Squad squad = Instantiate(
            option.squadPrefab, transform.position, Quaternion.identity).GetComponent<Squad>();
        squad.player = owner;
        squad.moveToPos(spawnRallyPoint.transform.position);
        configSquadForFaction(squad, owner);
        owner.addSquad(squad);
        return squad;
    }

    public void teleportToPos(Vector3 pos){}

    private void configSquadForFaction(Squad squad, GamePlayer owner){
        if(owner.isAlly){
            int allyLayer = LayerMask.NameToLayer("Ally");
            squad.config.enemyMask = LayerMask.GetMask("Enemy");
            squad.gameObject.layer = allyLayer;
            foreach(Collider2D collider in squad.GetComponentsInChildren<Collider2D>()){
                collider.gameObject.layer = allyLayer;
            }
        }else{
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            squad.config.enemyMask = LayerMask.GetMask("Ally");
            squad.gameObject.layer = enemyLayer;
            foreach(Collider2D collider in squad.GetComponentsInChildren<Collider2D>()){
                collider.gameObject.layer = enemyLayer;
            }
        }
    }

    public void attackAndMove(GameUnit target){}

    public void attackOnSight(GameUnit target){}

    public AiNavigator getAgent(){
        return null;
    }

    public CombatManager getCombatManager(){
        return null;
    }

    public Vector3 getHeadingToPos(){
        return Vector3.zero;
    }

    public GameObject getOwner(){
        return gameObject;
    }

    public Squad getOwnSquad(){
        return null;
    }

    public EntityStat getStat(){
        return stat;
    }

    public GameUnit getTarget(){
        return null;
    }

    public Transform getTransform(){
        return transform;
    }

    public bool isDead(){
        return isDestroyed;
    }

    public void moveCloseToPos(){}

    public void moveToPos(Vector3 pos){}

    public void onBuildingDestroyed(GameBuilding building){}

    public void onDie(){}

    public void onEnemyKilled(GameUnit enemy){}

    public void stopAtFireDistance(){}

    public void resetStoppingDistance(){}

    public void setStoppingDistance(float dist){}

    public void setTarget(GameUnit target){}
}