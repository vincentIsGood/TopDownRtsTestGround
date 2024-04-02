using System.Collections.Generic;
using System.Linq;

namespace com.vincentcodes.ai.behaviortree{

public enum NodeState{
    RUNNING, 
    SUCCESS, 
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


    public BehaviorTreeNode<T> addChild(BehaviorTreeNode<T> child){

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


    public void evaluate(){
        root.evaluate();
    }


    public V init<V>(V node) where V: BehaviorTreeNode<T>{
        node.tree = this;
        return node;
    }
}


public class SequenceNode<T>: BehaviorTreeNode<T>{
    public SequenceNode(): base(){
    }
    public SequenceNode(List<BehaviorTreeNode<T>> children): base(children){
    }
    public SequenceNode(params BehaviorTreeNode<T>[] children): base(children){
    }

    public override NodeState evaluate(){
        foreach(BehaviorTreeNode<T> child in children){
            switch(child.evaluate()){
                case NodeState.FAILURE:
                    return state = NodeState.FAILURE;
                case NodeState.RUNNING:
                    return state = NodeState.RUNNING;
                case NodeState.SUCCESS:
                    continue;
            }
        }
        return NodeState.SUCCESS;
    }
}


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

public class AllNode<T>: BehaviorTreeNode<T>{
    public AllNode(): base(){
    }
    public AllNode(List<BehaviorTreeNode<T>> children): base(children){
    }
    public AllNode(params BehaviorTreeNode<T>[] children): base(children){
    }

    public override NodeState evaluate(){
        foreach(BehaviorTreeNode<T> child in children){
            child.evaluate();
        }
        return NodeState.SUCCESS;
    }
}

}