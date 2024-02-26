using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Squad: MonoBehaviour{
    public float spawnRadius = 2f;
    public SquadBTData config;

    // public int soldiersCount = 5; // spawn them
    private FormationSolver formation;
    private TargetSolver targetSolver;

    [NonSerialized] public Vector3 center;
    [NonSerialized] public bool aiControl;

    private List<Soldier> members;
    private TargetPosIndicator targetPosIndicator;
    private Transform targetPos;

    private CoverSide cover;
    private Squad enemySquad;
    private bool inCombat = false;

    void Start(){
        formation = new FormationSolver(this);
        targetSolver = new TargetSolver(this);
        members = GetComponentsInChildren<Soldier>().ToList();
        targetPosIndicator = GetComponentInChildren<TargetPosIndicator>();
        targetPos = targetPosIndicator.transform;

        config.assign(this);
        initMembers();
    }
    private void initMembers(){
        foreach(Soldier member in members){
            member.transform.position = targetPos.position + RandomUtils.randomWithNeg(0.1f, 0.1f, 0);
        }
        formation.updateMembersLocalPos();
    }

    void Update(){
        center = findCenter();
    }

#if UNITY_EDITOR
    // https://discussions.unity.com/t/draw-2d-circle-with-gizmos/123578/1
    void OnDrawGizmos(){
        if(!EditorApplication.isPlaying) return;
        Handles.color = Color.black;
        Handles.DrawWireDisc(targetPos.position, Vector3.forward, spawnRadius);
        Handles.DrawWireDisc(center, Vector3.forward, 0.1f);
        Handles.color = Color.yellow;
        foreach(Soldier member in members)
            Handles.DrawWireDisc(member.headingToPos, Vector3.forward, 0.2f);

        Handles.color = Color.green;
        foreach(Soldier enemy in targetSolver.targetedEnemies)
            Handles.DrawWireDisc(enemy.transform.position, Vector3.forward, 0.2f);
    }
#endif

    public void onMemberDie(Soldier member){
        cover?.leaveSpot(member);
        targetSolver.clearTargetFor(member);

        members.Remove(member);
        formation.updateMembersLocalPos();

        if(members.Count == 0){
            Destroy(this.gameObject);
        }
    }
    public void onEnemyKilled(Soldier attacker, Soldier enemy){
        targetSolver.clearTargetFor(attacker);
        fireTowards(enemy.ownSquad);
    }

    public void moveToPos(Vector3 pos){
        targetPos.position = pos;
        targetSolver.clearSquadTargets();
        enemySquad = null;

        if(this.cover){
            leaveCover();
            this.cover = null;
        }

        formation.updateMembersLocalPos();
        foreach(Soldier member in members){
            member.resetStoppingDistance();
            member.moveToPos(pos + formation.getNoFormationPos(member));
        }
    }
    public void moveToPos(Vector3 pos, Cover cover){
        targetPos.position = pos;
        targetSolver.clearSquadTargets();
        enemySquad = null;

        if(this.cover) leaveCover();
        this.cover = cover.findClosestSide(this);

        formation.updateMembersLocalPos();
        foreach(Soldier member in members){
            member.moveCloseToPos();
            member.moveToPos(formation.getFormationPos(member, this.cover, pos));
        }
    }
    public void moveToPos(Vector3 pos, Squad squad){
        if(this == squad) return;
        targetPos.position = pos;
        enemySquad = squad;

        formation.updateMembersLocalPos();
        foreach(Soldier member in members){
            member.attackAndMove(targetSolver.assignTargetFrom(squad, member));
        }
    }
    public void fireTowards(Squad squad){
        if(this == squad || enemySquad != null) return;
        if(squad.getSoldiers().Count == 0) return;

        formation.updateMembersLocalPos();
        foreach(Soldier member in members){
            member.attackOnSight(targetSolver.assignTargetFrom(squad, member, false));
        }
    }
    
    public void leaveCover(){
        foreach(Soldier member in members)
            cover.leaveSpot(member);
    }

    public Vector3 findCenter(){
        // avg pos
        Vector3 result = Vector3.zero;
        foreach(Soldier member in members){
            result += member.transform.position;
        }
        return result / members.Count;
    }

    // Getter & Setters
    public List<Soldier> getSoldiers(){
        return members;
    }
    public bool isMoving(){
        return members.Any(member => member.agent.isMoving());
    }
    public bool isWithinStoppingDistance(){
        return members.All(member => member.agent.isWithinStoppingDistance());
    }
    public bool isInCombat(){
        return inCombat;
    }
}