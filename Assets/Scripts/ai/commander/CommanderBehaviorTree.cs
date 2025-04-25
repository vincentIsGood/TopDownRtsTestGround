using System;
using com.vincentcodes.ai.behaviortree;
using UnityEngine;


public class CommanderBehaviorTree : MonoBehaviour{
    [NonSerialized] public CommanderBTData config;

    private BehaviorTree<CommanderBTData> behaviorTree;
    private IntervalActionUtils updateTreeUpdater;

    void Start(){
        behaviorTree = buildTree();
        updateTreeUpdater = new IntervalActionUtils(behaviorTree.evaluate, 1f);
    }

    void Update(){
        if(config.enableAi)
            updateTreeUpdater.tick();
    }

    public BehaviorTree<CommanderBTData> buildTree(){
        BehaviorTree<CommanderBTData> tree = new BehaviorTree<CommanderBTData>(config);
        tree.setRoot(new SequenceNode<CommanderBTData>(
            tree.init(new FindBuildings()),
            tree.init(new EvaluateZoneInfluences()),
            tree.init(new AllNode<CommanderBTData>(
                tree.init(new SequenceNode<CommanderBTData>(
                    tree.init(new EvaluateOwnResources()),
                    tree.init(new FindNextMovePoint()),
                    tree.init(new FindAndSelectSquad()),
                    tree.init(new WalkToPoint())
                )),
                tree.init(new SequenceNode<CommanderBTData>(
                    tree.init(new PrioritizeNextUnitToBeCreated()),
                    tree.init(new CreateRequestedUnit())
                ))
            ))
        ));
        return tree;
    }
}