using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Unity 2d only
[RequireComponent(typeof(NavMeshAgent))]
public class AiNavigator : MonoBehaviour{
    [SerializeField] 
    private Transform target;
    private float stoppingDistance = 1;

    private NavMeshAgent agent;

    void Start(){
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
        this.target = target;
        if(target == null){
            agent.SetDestination(agent.transform.position);
        }
    }
    public void moveToPos(Vector3 pos){
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
            transform.rotation = Quaternion.AngleAxis(findAngle(agent.velocity.normalized), Vector3.forward);
    }

    // No rotation along xyz == forward (ideally point upward + no rotation == point forward)
    public static float findAngle(Vector3 direction){
        return Quaternion.FromToRotation(Vector3.up, direction).eulerAngles.z;
    }
}
