using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    protected NodeState state;
    public Node parent;
    protected List<Node> children = new List<Node>();

    public virtual NodeState Evaluate() => NodeState.FAILURE;

    private Dictionary<string, object> dataContext =
    new Dictionary<string, object>();

    public Node()
    {
        parent = null;
    }

    public Node(List<Node> children)
    {
        foreach (Node child in children)
            Attach(child);
    }

    private void Attach(Node node)
    {
        node.parent = this;
        children.Add(node);
    }

    public void SetData(string key, object value)
    {
        dataContext[key] = value;
    }

    // function is recursive and continues working up the branch
    // until we’ve either found the key we were looking for or reached the root of the tree
    public object GetData(string key)
    {
        object value = null;
        if (dataContext.TryGetValue(key, out value))
            return value;

        Node node = parent;
        while (node != null)
        {
            value = node.GetData(key);
            if (value != null)
                return value;
            node = node.parent;
        }
        return null;
    }

    //recursively search for the key and if we find it, we remove it from the dict.
    //Else, if we reach the root, we just ignore the request.
    public bool ClearData(string key)
    {
        if (dataContext.ContainsKey(key))
        {
            dataContext.Remove(key);
            return true;
        }

        Node node = parent;
        while (node != null)
        {
            bool cleared = node.ClearData(key);
            if (cleared)
                return true;
            node = node.parent;
        }
        return false;
    }
}

public enum NodeState
{
    RUNNING = 0,
    SUCCESS,
    FAILURE
}