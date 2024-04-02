using System.Collections.Generic;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;


[RequireComponent(typeof(Squad))]
public class TankSquadBehaviorTree : MonoBehaviour{
    private Squad squad;
    private BehaviorTree<SquadBTData> behaviorTree;
    private IntervalActionUtils updateTreeUpdater;

    void Start(){
        squad = GetComponent<Squad>();
        behaviorTree = buildTree();
        updateTreeUpdater = new IntervalActionUtils(behaviorTree.evaluate, .01f);
    }

    void Update(){
        if(squad.config.enableAi)
            updateTreeUpdater.tick();
    }

    public BehaviorTree<SquadBTData> buildTree(){
        BehaviorTree<SquadBTData> tree = new BehaviorTree<SquadBTData>(squad.config);
        tree.setRoot(new SelectorNode<SquadBTData>(
            tree.init(new SequenceNode<SquadBTData>(
                tree.init(new TankCheckEnemyIsAttackable()),
                tree.init(new AllNode<SquadBTData>(
                    tree.init(new MaintainDistance()),
                    tree.init(new TaskFireTowardsEnemy())
                ))
            )),
            tree.init(new ResetDistance())
            // tree.init(new SequenceNode<SquadBTData>(
            //     tree.init(new CanTakeCover()),
            //     tree.init(new TaskFindCover()),
            //     tree.init(new MoveToCover())
            // ))
        ));
        return tree;
    }
}