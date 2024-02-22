using System.Collections.Generic;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

// Shorten Behavior Tree = BT
[RequireComponent(typeof(Squad))]
public class SquadBehaviorTree : MonoBehaviour{
    private Squad squad;
    private BehaviorTree<SquadBTData> behaviorTree;
    private IntervalActionUtils updateTreeUpdater;

    void Start(){
        squad = GetComponent<Squad>();
        behaviorTree = buildTree();
        updateTreeUpdater = new IntervalActionUtils(behaviorTree.evaluate, .01f);
    }

    void Update(){
        updateTreeUpdater.tick();
    }

    public BehaviorTree<SquadBTData> buildTree(){
        BehaviorTree<SquadBTData> tree = new BehaviorTree<SquadBTData>(squad.config);
        tree.setRoot(new SequenceNode<SquadBTData>(
            tree.init(new CheckEnemyIsAttackable()),
            tree.init(new TaskFireTowardsEnemy())
        ));
        return tree;
    }
}