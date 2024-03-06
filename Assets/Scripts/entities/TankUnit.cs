using System;
using UnityEditor;
using UnityEngine;

public class TankUnit: MonoBehaviour, GameUnit{
    public HpBar hpBar;
    public Transform turretObject;

    public EntityStat stat = new EntityStat();
    public CombatManager combatManager;
    [NonSerialized] public AiNavigator agent;
    [NonSerialized] public Squad ownSquad;

    private GameUnit target = null;
    private Vector3 headingToPos;

    private bool dead = false;
    private bool chaseTarget = false;
    private bool canSeeEnemy = false;
    private bool canResetTurret = true;
    private IntervalActionUtils shootUpdater;
    private IntervalActionUtils turretAngleReseter;

    void Awake(){
        combatManager = new CombatManager(this);
        agent = GetComponent<AiNavigator>();
        ownSquad = GetComponentInParent<Squad>();
        stat.setHpBar(hpBar);
        shootUpdater = new IntervalActionUtils(shoot, ownSquad.config.attackSpeedSec);
        turretAngleReseter = new IntervalActionUtils(()=>canResetTurret = true, 15);
    }
    void Update(){
        if(target != null && !target.isDead()){
            if(chaseTarget){
                agent.moveTo(target.getTransform());
            }
            canSeeEnemy = GameVisionUtils.canSeeTarget(this, target, out RaycastHit2D hit, ownSquad.getExcludeFromView()) && hit.distance < ownSquad.config.attackRange;
            if(canSeeEnemy){
                turretRotateTowardsEnemy();
                shootUpdater.tick();
            }else if(!chaseTarget){
                setTarget(null);
            }
        }else{
            turretAngleReseter.tick();
        }

        if(canResetTurret){
            resetTurretAngle();
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
    private void shoot(){
        WeaponSuite.antiTankBullet(this, (target.getTransform().position - transform.position).normalized);
    }
    private void resetTurretAngle(){
        turretObject.rotation = Quaternion.Lerp(turretObject.rotation, transform.rotation, Time.deltaTime * 10);
    }
    private void turretRotateTowardsEnemy(){
        Vector3 enemyDirection = target.getTransform().position - transform.position;
        enemyDirection.z = 0;
        canResetTurret = false;
        turretObject.rotation = Quaternion.Lerp(
            turretObject.rotation, 
            Quaternion.FromToRotation(Vector3.up, enemyDirection), 
            Time.deltaTime * 10);
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
        moveToPos(pos);
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