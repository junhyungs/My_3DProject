using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotBT : MonoBehaviour
{
    private INode _node;

    private BTMonster _monster;


    private void Awake()
    {
        //∫Ò∆º∏¶ ∏∏µÎ
        SetBT();
    }


    private void Update()
    {
        _node.Evaluate();
    }

    private void SetBT()
    {
        //INode node = new SelectorNode(new List<INode> 
        //{
        //   new SelectorNode(new List<INode>
        //   {
        //       new Hit(_monster),
        //       new Hit(_monster)
        //   }),

        //   new SelectorNode(new List<INode>
        //   {


        //   })
        //});
    }

}

public class BTMonster : MonoBehaviour
{

}

//public class SelectorNode : INode
//{
//    private List<INode> _childNode;
//    public SelectorNode(List<INode> _childList)
//    {
//        _childNode = _childList;
//    }

//    public INode.State Evaluate()
//    {


//        return INode.State.Success;
//    }
//}


public class Hit : INode
{
    BTMonster _monster;

    public Hit(BTMonster monsterBT)
    {
        _monster = monsterBT;   
    }

    public INode.State Evaluate()
    {
        throw new System.NotImplementedException();
    }
}