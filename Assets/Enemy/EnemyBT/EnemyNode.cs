using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNode
{
    INode _root;

    public EnemyNode(INode root)
    {
        _root = root;
    }

    public void Execute()
    {
        _root.Evaluate();
    }

}
