using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

/*
Soldier +
        |- HpBar
        |- Soldier
*/
public class Soldier: MonoBehaviour{
    public HpBar hpBar;

    public EntityStat stat = new EntityStat();
    public CombatManager combatManager;
    [NonSerialized] public AiNavigator agent;
    [NonSerialized] public Squad ownSquad;

    [NonSerialized] public Soldier target = null;
    [NonSerialized] public Vector3 headingToPos;

    private bool chaseTarget = false;
    private IntervalActionUtils shootUpdater;

    void Start(){
        combatManager = new CombatManager(this);

        agent = GetComponent<AiNavigator>();
        ownSquad = GetComponentInParent<Squad>();
        stat.setHpBar(hpBar);
        shootUpdater = new IntervalActionUtils(shoot, ownSquad.config.attackSpeedSec);
    }
    void Update(){
        if(target){
            if(chaseTarget){
                agent.moveTo(target.transform);
            }
            GameVisionUtils.canSeeTarget(this, target, out RaycastHit2D hit);
            if(hit.collider != null && hit.distance < ownSquad.config.attackRange){
                shootUpdater.tick();
            }else{
                target = null;
            }
        }
    }
#if UNITY_EDITOR
    void OnDrawGizmos(){
        if(!target) return;
        GameVisionUtils.canSeeTarget(this, target, out RaycastHit2D hit);
        if(hit.collider != null){
            if(hit.distance < ownSquad.config.attackRange){
                Handles.color = Color.blue;
            }else Handles.color = Color.black;
        }else Handles.color = Color.red;
        Handles.DrawLine(transform.position, target.transform.position);
    }
#endif

    public void onDie(){
        ownSquad.onMemberDie(this);
        Destroy(this.transform.parent.gameObject);
    }
    public void onEnemyKilled(Soldier enemy){
        ownSquad.onEnemyKilled(this, enemy);
    }

    private void shoot(){
        WeaponSuite.bulletForward(this, (target.transform.position - transform.position).normalized);
    }

    public void moveToPos(Vector3 pos){
        headingToPos = pos;
        agent.moveToPos(pos);
    }

    public void attackOnSight(Soldier target){
        this.target = target;
        chaseTarget = false;
    }

    public void attackAndMove(Soldier target){
        this.target = target;
        
        if(target == null) return;
        chaseTarget = true;
        stopAtFireDistance();
    }

    public void stopAtFireDistance(){
        agent.setStoppingDistance(ownSquad.config.attackRange);
    }

    public void moveCloseToPos(){
        agent.setStoppingDistance(0.1f);
    }
    public void resetStoppingDistance(){
        agent.resetStoppingDistance();
    }
}