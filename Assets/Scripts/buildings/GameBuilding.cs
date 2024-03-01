using System;
using System.Collections.Generic;
using UnityEngine;

public class GameBuilding: MonoBehaviour{
    [Header("Stats")]
    public EntityStat entityStat;
    public GameObject spawnRallyPoint;

    [Header("Production")]
    public List<SpawnOption> spawnOptions = new List<SpawnOption>();

    [NonSerialized] public GamePlayer owner;

    public void spawnSquad(SpawnOption option){
        if(!owner.resourceStat.canAfford(option.cost)){
            return;
        }
        owner.resourceStat.subtract(option.cost);
        
        Squad squad = Instantiate(
            option.squadPrefab, transform.position, transform.rotation).GetComponent<Squad>();
        squad.owner = owner;
        squad.moveToPos(spawnRallyPoint.transform.position);
        configSquadForFaction(squad, owner);
        owner.addSquad(squad);
    }

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
}