using System.Collections.Generic;
using System.Linq;

// Credit:
// https://www.youtube.com/watch?v=aR6wt5BlE-E&t=946s
namespace com.vincentcodes.ai.behaviortree{

public enum NodeState{
    RUNNING, 
    
    // Node ended successfully
    SUCCESS, 

    // Node failed, either during or after running.
    FAILURE
}

public class BehaviorTreeNode<T>{
    public BehaviorTree<T> tree;
    protected NodeState state;
    protected BehaviorTreeNode<T> parent;
    protected List<BehaviorTreeNode<T>> children = new List<BehaviorTreeNode<T>>();

    public BehaviorTreeNode(){
    }

    public BehaviorTreeNode(List<BehaviorTreeNode<T>> children){
        addChildren(children);
    }
    public BehaviorTreeNode(params BehaviorTreeNode<T>[] children){
        addChildren(children.ToList());
    }

    // Remember to do `tree.init(child)` yourself
    public BehaviorTreeNode<T> addChild(BehaviorTreeNode<T> child){
        // child.tree = tree;
        child.parent = this;
        this.children.Add(child);
        return this;
    }
    public BehaviorTreeNode<T> addChildren(List<BehaviorTreeNode<T>> children){
        foreach(BehaviorTreeNode<T> node in children)
            addChild(node);
        return this;
    }
    public BehaviorTreeNode<T> addChildren(params BehaviorTreeNode<T>[] children){
        foreach(BehaviorTreeNode<T> node in children)
            addChild(node);
        return this;
    }

    public virtual NodeState evaluate() => NodeState.FAILURE;
}

public class BehaviorTree<T>{
    public BehaviorTreeNode<T> root;
    public T sharedData;

    public BehaviorTree(T sharedData){
        this.sharedData = sharedData;
    }
    public BehaviorTree(BehaviorTreeNode<T> root, T sharedData){
        setRoot(root);
        this.sharedData = sharedData;
    }

    public BehaviorTreeNode<T> setRoot(BehaviorTreeNode<T> root){
        return this.root = init(root);
    }

    // update the tree so that states within the tree can be updated
    public void tick(){
        root.evaluate();
    }

    // configure a node
    public V init<V>(V node) where V: BehaviorTreeNode<T>{
        node.tree = this;
        return node;
    }
}

// AND Logic
public class SequenceNode<T>: BehaviorTreeNode<T>{
    public SequenceNode(): base(){
    }
    public SequenceNode(List<BehaviorTreeNode<T>> children): base(children){
    }
    public SequenceNode(params BehaviorTreeNode<T>[] children): base(children){
    }

    public override NodeState evaluate(){
        bool isAnyChildRunning = false;
        foreach(BehaviorTreeNode<T> child in children){
            switch(child.evaluate()){
                case NodeState.FAILURE:
                    return state = NodeState.FAILURE;
                case NodeState.RUNNING:
                    isAnyChildRunning = true;
                    continue;
                case NodeState.SUCCESS:
                    continue;
            }
        }
        return isAnyChildRunning? NodeState.RUNNING : NodeState.SUCCESS;
    }
}

// OR Logic (ordering is important)
public class SelectorNode<T>: BehaviorTreeNode<T>{
    public SelectorNode(): base(){
    }
    public SelectorNode(List<BehaviorTreeNode<T>> children): base(children){
    }
    public SelectorNode(params BehaviorTreeNode<T>[] children): base(children){
    }

    public override NodeState evaluate(){
        foreach(BehaviorTreeNode<T> child in children){
            switch(child.evaluate()){
                case NodeState.FAILURE:
                    continue;
                case NodeState.RUNNING:
                    return state = NodeState.RUNNING;
                case NodeState.SUCCESS:
                    return state = NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}

}