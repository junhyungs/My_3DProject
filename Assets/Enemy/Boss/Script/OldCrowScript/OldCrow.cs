using UnityEngine;

public partial class OldCrow : MonoBehaviour
{
    //Node
    private EnemyNode _node;

    //Animator
    [Header("Animator")]
    [SerializeField] private Animator _animator;

    //Player
    private GameObject _player;


    private void Start()
    {
        _player = GameManager.Instance.Player;
        //_node = new BossNode(SetUpNode());
    }

    //private INode SetUpNode()
    //{
        
    //}

}
