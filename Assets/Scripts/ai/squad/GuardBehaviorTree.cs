using System.Collections.Generic;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

// Shorten Behavior Tree = BT
[RequireComponent(typeof(Squad))]
public class GuardsBehaviorTree : MonoBehaviour{
    public Transform[] waypoints;

    private Squad squad;
    private BehaviorTree<SquadBTData> behaviorTree;

    void Start(){
        squad = GetComponent<Squad>();
        behaviorTree = buildTree();
    }

    void Update(){
        InvokeRepeating("updateTree", 0, .5f);
    }

    void updateTree(){
        behaviorTree.tick();
    }

    public BehaviorTree<SquadBTData> buildTree(){
        BehaviorTree<SquadBTData> tree = new BehaviorTree<SquadBTData>(squad.config);
        tree.setRoot(new SelectorNode<SquadBTData>(
            tree.init(new SequenceNode<SquadBTData>(
                tree.init(new CheckEnemyInPursueRange()),
                tree.init(new TaskPursueEnemy())
            )),
            tree.init(new TaskPatrol(waypoints))
        ));
        return tree;
    }
}