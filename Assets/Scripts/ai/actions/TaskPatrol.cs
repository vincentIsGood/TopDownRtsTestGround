using com.vincentcodes.ai.behaviortree;
using UnityEngine;

public class TaskPatrol: BehaviorTreeNode<SquadBTData>{
    private Transform[] waypoints;

    private float waitTimeConfig = 3;

    private int waypointIndex = 0;
    private float timeStartedWaiting = 0;

    public TaskPatrol(Transform[] waypoints){
        this.waypoints = waypoints;
    }

    public override NodeState evaluate(){
        if(!tree.sharedData.squad.isMoving()){
            if(Time.time - timeStartedWaiting < waitTimeConfig) 
                return NodeState.RUNNING;
            
            timeStartedWaiting = Time.time;
            moveToNextWaypoint();
        }
        return NodeState.RUNNING;
    }

    private void moveToNextWaypoint(){
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
        tree.sharedData.squad.moveToPos(waypoints[waypointIndex].position);
    }
}