using System.Collections.Generic;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;

// Shorten Behavior Tree = BT
[RequireComponent(typeof(Squad))]
public class SquadBehaviorTree : MonoBehaviour{
    public LayerMask enemyMask;

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
        tree.setRoot(new SequenceNode<SquadBTData>(
            tree.init(new CheckEnemyInAttackRange())
        ));
        return tree;
    }
}