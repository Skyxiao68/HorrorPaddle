// Behavior Tree Base Nodes
using System.Collections.Generic;

public abstract class BTNode
{
    public abstract bool Execute();
}

public class Sequence : BTNode
{
    private List<BTNode> nodes = new List<BTNode>();

    public Sequence(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }

    public override bool Execute()
    {
        foreach (var node in nodes)
        {
            if (!node.Execute())
                return false;
        }
        return true;
    }
}

public class Selector : BTNode
{
    private List<BTNode> nodes = new List<BTNode>();

    public Selector(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }

    public override bool Execute()
    {
        foreach (var node in nodes)
        {
            if (node.Execute())
                return true;
        }
        return false;
    }
}

public class Condition : BTNode
{
    private System.Func<bool> condition;

    public Condition(System.Func<bool> condition)
    {
        this.condition = condition;
    }

    public override bool Execute()
    {
        return condition();
    }
}

public class ActionNode : BTNode
{
    private System.Action action;

    public ActionNode(System.Action action)
    {
        this.action = action;
    }

    public override bool Execute()
    {
        action();
        return true;
    }
}