using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Soldier: MonoBehaviour{
    public EntityStat stat = new EntityStat();
    [NonSerialized] public AiNavigator agent;
    [NonSerialized] public Squad ownSquad;

    [NonSerialized] public Soldier target = null;
    [NonSerialized] public Vector3 headingToPos;

    private bool chaseTarget = false;
    private IntervalActionUtils shootUpdater;

    void Start(){
        agent = GetComponent<AiNavigator>();
        ownSquad = GetComponentInParent<Squad>();
        shootUpdater = new IntervalActionUtils(shoot, ownSquad.config.attackSpeedSec);
    }
    void Update(){
        if(target){
            if(chaseTarget){
                agent.moveTo(target.transform);
            }
            RaycastHit2D hit = GameVisionUtils.canSeeTargetDetailed(this, target);
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
        RaycastHit2D hit = GameVisionUtils.canSeeTargetDetailed(this, target);
        if(hit.collider != null){
            if(hit.distance < ownSquad.config.attackRange){
                Handles.color = Color.blue;
            }else Handles.color = Color.black;
        }else Handles.color = Color.red;
        Handles.DrawLine(transform.position, target.transform.position);
    }
#endif

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