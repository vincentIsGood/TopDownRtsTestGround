using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class AiNavigator : MonoBehaviour{
    [NonSerialized]
    public float offsetRotZ = 0;

    [SerializeField] 
    private Transform target;
    private float stoppingDistance = 1;

    private NavMeshAgent agent;

    void Awake(){
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.stoppingDistance = stoppingDistance;
    }

    void Update(){
        if(target)
            agent.SetDestination(target.position);
        if(isMoving()) faceTarget();
    }

    public void moveTo(Transform target){
        enable();
        this.target = target;
        if(target == null){
            agent.SetDestination(agent.transform.position);
        }
    }
    public void moveToPos(Vector3 pos){
        enable();
        moveTo(null);
        agent.SetDestination(pos);
    }

    public void setStoppingDistance(float dist){
        agent.stoppingDistance = dist;
    }
    public void resetStoppingDistance(){
        setStoppingDistance(stoppingDistance);
    }

    public bool isMoving(){
        return agent.velocity.magnitude > 0.005f;
    }

    public bool isWithinStoppingDistance(){
        if(target == null) return false;
        return Vector3.Distance(transform.position, target.position) < agent.stoppingDistance;
    }


    public void faceTarget(){
        if(agent.velocity.magnitude > 0.1f)
            transform.rotation = Quaternion.AngleAxis(findAngle(agent.velocity.normalized) + offsetRotZ, Vector3.forward);
    }

    public void disable(){
        agent.enabled = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }
    public void enable(){
        if(!agent.enabled)
            agent.enabled = true;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }


    public static float findAngle(Vector3 direction){
        return Quaternion.FromToRotation(Vector3.up, direction).eulerAngles.z;
    }
}
