using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Soldier: MonoBehaviour{
    [NonSerialized] public AiNavigator agent;
    public Vector3 headingToPos;

    // do not clear target yourself
    public Soldier target = null;
    private bool chaseTarget = false;
    private IntervalUtils shooter;

    public Squad ownSquad;

    void Start(){
        agent = GetComponent<AiNavigator>();
        ownSquad = GetComponentInParent<Squad>();
        shooter = new IntervalUtils(shoot, ownSquad.config.attackSpeedSec, true);
    }
    void Update(){
        if(target){
            if(chaseTarget){
                agent.moveTo(target.transform);
            }
            RaycastHit2D hit = TargetSolver.canSeeTargetDetailed(this, target);
            if(hit.collider != null && hit.distance < ownSquad.config.attackRange){
                shooter.tick();
            }
        }
    }
    void OnDrawGizmos(){
        if(!target) return;
        RaycastHit2D hit = TargetSolver.canSeeTargetDetailed(this, target);
        if(hit.collider != null){
            if(hit.distance < ownSquad.config.attackRange){
                Handles.color = Color.blue;
            }else Handles.color = Color.black;
        }else Handles.color = Color.red;
        Handles.DrawLine(this.transform.position, target.transform.position);
    }
    private void shoot(){
        Debug.Log("Shooting");
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