using System;
using System.Collections;
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
    public CombatManager combatManager;
    [NonSerialized] public AiNavigator agent;
    [NonSerialized] public Squad ownSquad;

    private GameUnit target = null;
    private Vector3 headingToPos;

    private bool dead = false;
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
        if(target != null && !target.isDead()){
            if(chaseTarget){
                agent.moveTo(target.getTransform());
            }
            if(GameVisionUtils.canSeeTarget(this, target, out RaycastHit2D hit) 
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
        if(GameVisionUtils.canSeeTarget(this, target, out RaycastHit2D hit)){
            if(hit.distance < ownSquad.config.attackRange){
                Handles.color = Color.blue;
            }else Handles.color = Color.black;
        }else Handles.color = Color.red;
        Handles.DrawLine(transform.position, target.getTransform().position);
    }
#endif

    private void shoot(){
        WeaponSuite.bullet(this, (target.getTransform().position - transform.position).normalized);
    }

    public void onDie(){
        dead = true;
        ownSquad.onMemberDie(this);
        Destroy(this.transform.parent.gameObject);
    }
    public void onEnemyKilled(GameUnit enemy){
        ownSquad.onEnemyKilled(this, enemy);
    }

    public void moveToPos(Vector3 pos){
        headingToPos = pos;
        agent.moveToPos(pos);
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
        agent.setStoppingDistance(Mathf.Max(0.1f, ownSquad.config.attackRange-0.5f));
    }
    public void moveCloseToPos(){
        agent.setStoppingDistance(0.1f);
    }
    public void resetStoppingDistance(){
        agent.resetStoppingDistance();
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