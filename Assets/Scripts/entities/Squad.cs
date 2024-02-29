using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Squad: MonoBehaviour{
    public float spawnRadius = 2f;
    public SquadBTData config;

    private FormationSolver formation;
    private TargetSolver targetSolver;

    [NonSerialized] public Vector3 center;
    [NonSerialized] public bool aiControl;

    private List<GameUnit> members;
    private TargetPosIndicator targetPosIndicator;
    private Transform targetPos;

    private CoverSide cover;
    private Squad targetSquad;

    void Start(){
        formation = new FormationSolver(this);
        targetSolver = new TargetSolver(this);
        members = GetComponentsInChildren<GameUnit>().ToList();
        targetPosIndicator = GetComponentInChildren<TargetPosIndicator>();
        targetPos = targetPosIndicator.transform;

        config.assign(this);
        initMembers();
    }
    private void initMembers(){
        foreach(GameUnit member in members){
            member.getTransform().position = targetPos.position + RandomUtils.randomWithNeg(0.1f, 0.1f, 0);
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
        foreach(GameUnit unit in members)
            Handles.DrawWireDisc(unit.getHeadingToPos(), Vector3.forward, 0.2f);

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
            Destroy(this.gameObject);
        }
    }
    public void onEnemyKilled(GameUnit attacker, GameUnit enemy){
        targetSolver.clearTargetFor(attacker);
        fireTowards(enemy.getOwnSquad());
    }

    public void moveToPos(Vector3 pos){
        targetPos.position = pos;
        targetSolver.clearSquadTargets();
        targetSquad = null;

        if(this.cover){
            leaveCover();
            this.cover = null;
        }

        formation.updateMembersLocalPos();
        foreach(GameUnit member in members){
            member.resetStoppingDistance();
            member.moveToPos(pos + formation.getNoFormationPos(member));
        }
    }
    public void moveToPos(Vector3 pos, Cover cover){
        targetPos.position = pos;
        targetSolver.clearSquadTargets();
        targetSquad = null;

        if(this.cover) leaveCover();
        this.cover = cover.findClosestSide(this);

        formation.updateMembersLocalPos();
        foreach(GameUnit member in members){
            member.moveCloseToPos();
            member.moveToPos(formation.getFormationPos(member, this.cover, pos));
        }
    }
    public void moveToPos(Vector3 pos, Squad squad){
        if(this == squad) return;
        targetPos.position = pos;
        targetSquad = squad;

        formation.updateMembersLocalPos();
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
            member.attackOnSight(targetSolver.assignTargetFrom(squad, member, false));
        }
    }
    
    public void leaveCover(){
        foreach(GameUnit member in members){
            if(member is Soldier soldier)
                cover.leaveSpot(soldier);
        }
    }

    public Vector3 findCenter(){
        // avg pos
        Vector3 result = Vector3.zero;
        foreach(GameUnit member in members){
            result += member.getTransform().position;
        }
        return result / members.Count;
    }

    // Getter & Setters
    public List<GameUnit> getUnits(){
        return members;
    }
    public bool isMoving(){
        return members.Any(member => member.getAgent().isMoving());
    }
    public bool isWithinStoppingDistance(){
        return members.All(member => member.getAgent().isWithinStoppingDistance());
    }
}