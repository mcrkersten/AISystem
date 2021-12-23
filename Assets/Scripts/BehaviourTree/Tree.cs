using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tree : MonoBehaviour
{
    protected Node root = null;

    protected virtual void Start()
    {
        root = SetupTree();
    }

    private void Update()
    {
        if (root != null)
            root.Evaluate();
    }

    protected abstract Node SetupTree();
}
