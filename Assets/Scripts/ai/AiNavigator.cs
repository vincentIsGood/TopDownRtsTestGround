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
        if(target){
            agent.SetDestination(target.position);
            faceTarget();
        }
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
        return agent.velocity.magnitude > 0;
    }

    public bool isWithinStoppingDistance(){
        if(target == null) return false;
        return Vector3.Distance(transform.position, target.position) < agent.stoppingDistance;
    }

    // pointing up is the forward vector in our 2D game
    public float getForwardAngle(){
        float angle = Vector3.Angle(agent.velocity.normalized, Vector3.up);
        if (agent.velocity.normalized.z < 0){
            return 360 - angle;
        }
        return angle;
    }

    private void faceTarget(){
        transform.rotation = Quaternion.AngleAxis(getForwardAngle(), Vector3.forward);
    }
}
