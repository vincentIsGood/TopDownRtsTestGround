using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
Soldier +
        |- HpBar
        |- Soldier
*/
public class Soldier: MonoBehaviour, GameUnit{
    public HpBar hpBar;
    public EntityStat stat = new EntityStat();

    [Header("Combat")]
    public float bulletSpeed = 10;
    public float attackSpeedSec = 1.5f;

    protected Squad ownSquad;
    protected AiNavigator agent;
    protected CombatManager combatManager;

    protected GameUnit target = null;
    protected Vector3 headingToPos;

    protected bool dead = false;
    protected bool chaseTarget = false;
    protected IntervalActionUtils shootUpdater;
    
    private Quaternion offsetRot;

    void Awake(){
        ownSquad = GetComponentInParent<Squad>();
        agent = GetComponent<AiNavigator>();
        combatManager = new CombatManager(this);
        shootUpdater = new IntervalActionUtils(shoot, attackSpeedSec);

        stat.setHpBar(hpBar);

        offsetRot = transform.rotation;
        agent.offsetRotZ = offsetRot.eulerAngles.z;
    }
    void Update(){
        if(target != null && !target.isDead()){
            if(chaseTarget){
                agent.moveTo(target.getTransform());
            }
            if(GameVisionUtils.canSeeTarget(this, target, out RaycastHit2D hit, ownSquad.getExcludeFromView())
            && hit.distance < ownSquad.config.attackRange){
                shootUpdater.tick();
            }else if(!chaseTarget){
                setTarget(null);
            }
        }
    }
#if UNITY_EDITOR
    void OnDrawGizmos(){
        if(target == null || target.isDead()) return;
        if(GameVisionUtils.canSeeTarget(this, target, out RaycastHit2D hit, ownSquad.getExcludeFromView())){
            if(hit.distance < ownSquad.config.attackRange){
                Handles.color = Color.blue;
            }else Handles.color = Color.black;
        }else Handles.color = Color.red;
        Handles.DrawLine(transform.position, target.getTransform().position);
    }
#endif

    protected virtual void shoot(){
        WeaponSuite.bullet(this, (target.getTransform().position - transform.position).normalized, bulletSpeed);
    }

    public void onDie(){
        dead = true;
        ownSquad.onMemberDie(this);
        Destroy(this.transform.parent.gameObject);
    }
    public void onEnemyKilled(GameUnit enemy){
        ownSquad.onEnemyKilled(this, enemy);
    }
    public void onBuildingDestroyed(GameBuilding house){
        ownSquad.onBuildingDestroyed(this, house);
    }

    public void moveToPos(Vector3 pos){
        headingToPos = pos;
        agent.moveToPos(pos);
    }
    public void teleportToPos(Vector3 pos){
        headingToPos = pos;
        agent.moveToPos(pos);
        transform.position = pos;
        agent.disable();
    }
    public void attackOnSight(GameUnit target){
        setTarget(target);
        chaseTarget = false;
    }
    public void attackAndMove(GameUnit target){
        setTarget(target);
        if(target == null) return;
        chaseTarget = true;
        stopAtFireDistance();
    }

    public void stopAtFireDistance(){
        agent.setStoppingDistance(Mathf.Max(0.1f, ownSquad.config.attackRange-0.2f));
    }
    public void moveCloseToPos(){
        agent.setStoppingDistance(0.1f);
    }
    public void resetStoppingDistance(){
        agent.resetStoppingDistance();
    }
    public void setStoppingDistance(float dist){
        agent.setStoppingDistance(dist);
    }

    public Vector3 getHeadingToPos(){
        return headingToPos;
    }
    public GameUnit getTarget(){
        return target;
    }
    public void setTarget(GameUnit target){
        this.target = target;
    }
    public Squad getOwnSquad(){
        return ownSquad;
    }

    public AiNavigator getAgent(){
        return agent;
    }
    public EntityStat getStat(){
        return stat;
    }
    public CombatManager getCombatManager(){
        return combatManager;
    }

    public GameObject getOwner(){
        return this.gameObject;
    }
    public Transform getTransform(){
        return this.transform;
    }

    public bool isDead(){
        return dead;
    }
}
