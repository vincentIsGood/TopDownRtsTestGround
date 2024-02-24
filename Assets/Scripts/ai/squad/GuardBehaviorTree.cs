using System.Collections.Generic;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

// Shorten Behavior Tree = BT
[RequireComponent(typeof(Squad))]
public class GuardsBehaviorTree : MonoBehaviour{
    public Transform[] waypoints;

    private Squad squad;
    private BehaviorTree<SquadBTData> behaviorTree;
    private IntervalActionUtils treeUpdater;

    void Start(){
        squad = GetComponent<Squad>();
        behaviorTree = buildTree();
        treeUpdater = new IntervalActionUtils(behaviorTree.evaluate, .01f);
    }

    void Update(){
        treeUpdater.tick();
    }

    public BehaviorTree<SquadBTData> buildTree(){
        BehaviorTree<SquadBTData> tree = new BehaviorTree<SquadBTData>(squad.config);
        tree.setRoot(new SelectorNode<SquadBTData>(
            tree.init(new SequenceNode<SquadBTData>(
                tree.init(new CheckEnemyIsAttackable()),
                tree.init(new SelectorNode<SquadBTData>(
                    tree.init(new SequenceNode<SquadBTData>(
                        // TODO: still doesn't find cover. HELP
                        tree.init(new DebugBeep()),
                        tree.init(new TaskFindCover()),
                        tree.init(new TaskFireTowardsEnemy())
                    )),
                    tree.init(new TaskFireTowardsEnemy())
                ))
            )),
            tree.init(new TaskPatrol(waypoints))
        ));
        return tree;
    }
}