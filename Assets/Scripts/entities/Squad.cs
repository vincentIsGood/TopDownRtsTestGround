using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Squad: MonoBehaviour{
    public SquadBTData config;

    [NonSerialized] public GamePlayer player;
    [NonSerialized] public Vector3 center;
    [NonSerialized] public Vector3 destCenter;
    [NonSerialized] public int originalAmount;
    [NonSerialized] public Type unitType;
    [NonSerialized] public bool engaging = false;
    [NonSerialized] public int xp = 0;

    private bool insideBuilding;
    private Action enterAction;
    private Action exitAction;

    private FormationSolver formation;
    private TargetSolver targetSolver;

    [NonSerialized] public Transform targetPos;
    [NonSerialized] public List<Squad> canSeeEnemies = new List<Squad>();
    [NonSerialized] public List<GameBuilding> canSeeEnemyBuildings = new List<GameBuilding>();

    private List<GameUnit> members;
    private TargetPosIndicator targetPosIndicator;
    private Vector3 headingToPos;
    private HashSet<GameObject> excludeFromView = new HashSet<GameObject>();

    private IntervalActionUtils intervalCaller;

    private CoverSide cover;
    private Squad targetSquad;

    void Awake(){
        formation = new FormationSolver(this);
        targetSolver = new TargetSolver(this);
        members = GetComponentsInChildren<GameUnit>().ToList();
        targetPosIndicator = GetComponentInChildren<TargetPosIndicator>();
        targetPos = targetPosIndicator.transform;

        intervalCaller = new IntervalActionUtils(updateNearbyEnemies, 1.5f);

        originalAmount = members.Count;
        unitType = members[0].GetType();
        config.assign(this);
    }
    void Start(){
        initMembers();
    }
    private void initMembers(){
        foreach(GameUnit member in members){
            member.getTransform().position = transform.position + RandomUtils.randomWithNeg(0.1f, 0.1f, 0);

            // if(player.isAlly && member is Soldier){
            //     Color currentColor = member.getOwner().GetComponent<SpriteRenderer>().color;
            //     member.getOwner().GetComponent<SpriteRenderer>().color = Color.Lerp(currentColor, Color.blue, 0.6f);
            // }
        }
        formation.updateMembersLocalPos();
    }

    void Update(){
        intervalCaller.tick();
        center = findCenter();
        destCenter = findDestCenter();
        engaging = members.Any(m => m.getTarget() != null);

        if(enterAction != null && Vector3.Distance(center, headingToPos) < config.enterRadius){
            enterAction?.Invoke();
            enterAction = null;
        }
    }

    private Collider2D[] colliders = new Collider2D[5];
    private void updateNearbyEnemies(){
        canSeeEnemies.Clear();
        canSeeEnemyBuildings.Clear();
        int hitCount = Physics2D.OverlapCircleNonAlloc(center, config.findRange, colliders, config.enemyMask);
        for(int i = 0; i < hitCount; i++){
            if(colliders[i].TryGetComponent(out GameUnit unit)){
                if(unit is GameBuilding building){
                    canSeeEnemyBuildings.Add(building);
                }else{
                    canSeeEnemies.Add(unit.getOwnSquad());
                }
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos(){
        if(!EditorApplication.isPlaying) return;
        Handles.color = Color.black;
        Handles.DrawWireDisc(center, Vector3.forward, 0.1f);
        // Handles.DrawWireDisc(center, Vector3.forward, config.findRange);
        Handles.color = Color.yellow;
        foreach(GameUnit unit in members)
            Handles.DrawWireDisc(unit.getHeadingToPos(), Vector3.forward, 0.2f);
        Handles.DrawWireDisc(destCenter, Vector3.forward, 0.1f);

        Handles.color = Color.green;
        foreach(GameUnit enemy in targetSolver.targetedEnemies){
            if(!enemy.isDead())
                Handles.DrawWireDisc(enemy.getTransform().position, Vector3.forward, 0.2f);
        }
    }
#endif

    public void onMemberDie(GameUnit member){
        if(member is Soldier soldier)
            cover?.leaveSpot(soldier);
        targetSolver.clearTargetFor(member);

        members.Remove(member);
        formation.updateMembersLocalPos();

        if(members.Count == 0){
            player.removeSquad(this);
            moveToPosReset(transform.position);
            Destroy(this.gameObject);
        }
    }
    public void onEnemyKilled(GameUnit attacker, GameUnit enemy){
        targetSolver.clearTargetFor(attacker);
        fireTowards(enemy.getOwnSquad());
        xp += enemy.getStat().xpReward;
    }
    public void onBuildingDestroyed(GameUnit attacker, GameBuilding building){
        targetSolver.clearTargetFor(attacker);
    }
    public void onAnyUnitTakeDamage(GameUnit member){
        
    }

    public void moveToPos(Vector3 pos){
        moveToPosReset(pos);

        foreach(GameUnit member in members){
            member.resetStoppingDistance();
            member.moveToPos(pos + formation.getNoFormationPos(member));
        }
    }
    public void moveToPos(Vector3 pos, Cover cover){
        moveToPosReset(pos);
        this.cover = cover.findClosestSide(this);

        if(members[0] is not Soldier){
            moveToPos(pos);
            return;
        }

        foreach(GameUnit member in members){
            member.moveCloseToPos();
            member.moveToPos(formation.getFormationPos(member, this.cover, pos));
        }
    }
    public void moveToPos(Vector3 pos, EnterableHouse house){
        moveToPosReset(pos);
        if(!house.canEnter(this))
            return;

        if(members[0] is not Soldier){
            moveToPos(pos);
            return;
        }

        headingToPos = house.getEntrancePos();
        enterAction = ()=>{
            if(!house.canEnter(this))
                return;
            cover = house.space;
            insideBuilding = true;
            formation.updateMembersLocalPos();
            house.squadEntered(this);
            foreach(GameUnit member in members){
                excludeFromView.Add(house.gameObject);
                member.teleportToPos(house.enter(member));
            }
        };
        exitAction = ()=>{
            house.squadLeaved(this);
            foreach(GameUnit member in members){
                excludeFromView.Remove(house.gameObject);
                member.teleportToPos(house.getEntrancePos() + formation.getNoFormationPos(member));
            }
            house = null;
        };
        foreach(GameUnit member in members){
            member.moveCloseToPos();
            member.moveToPos(headingToPos);
        }
    }
    public void moveToPos(Vector3 pos, Squad squad){
        if(this == squad) return;
        targetPos.position = pos;
        targetSquad = squad;

        foreach(GameUnit member in members){
            member.attackAndMove(targetSolver.assignTargetFrom(squad, member));
        }
    }
    public void fireTowards(Squad squad){
        if(this == squad) return;
        if(targetSquad != null && targetSquad != squad) return;
        if(squad.getUnits().Count == 0) return;

        formation.updateMembersLocalPos();
        foreach(GameUnit member in members){
            member.attackOnSight(targetSolver.assignTargetFrom(squad, member));
        }
    }
    public void fireTowards(GameUnit unit){
        formation.updateMembersLocalPos();
        foreach(GameUnit member in members){
            member.attackOnSight(unit);
        }
    }
    public void moveToPosReset(Vector3 pos){
        targetPos.position = pos;
        targetSolver.clearSquadTargets();
        targetSquad = null;

        if(exitAction != null && insideBuilding){
            exitAction?.Invoke();
            exitAction = null;
            insideBuilding = false;
        }

        if(this.cover){
            leaveCover();
            this.cover = null;
        }

        formation.updateMembersLocalPos();
    }
    public void setStoppingDistance(float dist){
        foreach(GameUnit member in members){
            member.setStoppingDistance(dist);
        }
    }
    public void resetStoppingDistance(){
        foreach(GameUnit member in members){
            member.resetStoppingDistance();
        }
    }

    public void leaveCover(){
        foreach(GameUnit member in members){
            if(member is Soldier soldier)
                cover.leaveSpot(soldier);
        }
    }

    public Vector3 findCenter(){
        Vector3 result = Vector3.zero;
        foreach(GameUnit member in members){
            result += member.getTransform().position;
        }
        return result / members.Count;
    }
    public Vector3 findDestCenter(){
        Vector3 result = Vector3.zero;
        foreach(GameUnit member in members){
            result += member.getHeadingToPos();
        }
        return result / members.Count;
    }

    public bool isExcludedFromView(GameObject go){
        return excludeFromView.Contains(go);
    }
    public HashSet<GameObject> getExcludeFromView(){
        return excludeFromView;
    }

    public List<GameUnit> getUnits(){
        return members;
    }

    public Type getUnitType(){
        return unitType;
    }

    public bool isMoving(){
        return members.Any(member => member.getAgent().isMoving());
    }
    public bool isWithinStoppingDistance(){
        return members.All(member => member.getAgent().isWithinStoppingDistance());
    }
}
