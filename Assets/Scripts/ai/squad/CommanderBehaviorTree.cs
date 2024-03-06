using com.vincentcodes.ai.behaviortree;
using UnityEngine;


public class CommanderBehaviorTree : MonoBehaviour{
    public RtsController rtsController;
    private BehaviorTree<CommanderBTData> behaviorTree;
    private IntervalActionUtils updateTreeUpdater;

    void Start(){
        behaviorTree = buildTree();
        updateTreeUpdater = new IntervalActionUtils(behaviorTree.evaluate, .01f);
    }

    void Update(){
        updateTreeUpdater.tick();
    }

    public BehaviorTree<CommanderBTData> buildTree(){
        BehaviorTree<CommanderBTData> tree = new BehaviorTree<CommanderBTData>(rtsController.enemyCommanderConfig);
        tree.setRoot(new SequenceNode<CommanderBTData>(
        ));
        return tree;
    }
}