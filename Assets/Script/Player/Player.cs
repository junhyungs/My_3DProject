using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerHealth _healthComponent;
    private GameObject _shadowPlayer;
 
    public PlayerMoveController _moveController { get; private set; }
    public ShadowPlayer ShadowPlayer { get; set; }
    
    private void Awake()
    {
        _healthComponent = gameObject.GetComponent<PlayerHealth>();

        _moveController = gameObject.GetComponent<PlayerMoveController>();
    }
    
    private void OnEnable()
    {
        if(_shadowPlayer == null)
        {
            CreateShadowPlayer();
        }
    }

    private void Start()
    {
        StartCoroutine(LoadPlayerData("P101"));
    }

    private IEnumerator LoadPlayerData(string id)
    {
        _moveController.IsAction = false;

        yield return new WaitWhile(() =>
        {
            Debug.Log("플레이어 데이터를 가져오지 못했습니다.");
            return DataManager.Instance.GetData(id) == null;
        });

        var data = DataManager.Instance.GetData(id) as PlayerData;

        _healthComponent.PlayerHP = data.Health;
        _moveController.SetMoveData(data.Speed, data.RollSpeed, data.LadderSpeed,
            data.SpeedChangeValue, data.SpeedOffSet, data.Gravity);

        _moveController.IsAction = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteraction();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            OnInteractionDialogue();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            _moveController.OnLadder();
        }
    }

    private void CreateShadowPlayer()
    {
        _shadowPlayer = new GameObject();

        ShadowPlayer shadowPlayer = _shadowPlayer.AddComponent<ShadowPlayer>();

        shadowPlayer.InitializeShadowPlayer(this);

        ShadowPlayer = shadowPlayer;

        _shadowPlayer.transform.position = new Vector3(transform.position.x,
            transform.position.y + 1f, transform.position.z);
    }

    private void OnInteractionDialogue()
    {
        Vector3 spherePosition = transform.position + new Vector3(0f, 0.5f, 0f);

        float sphereRadius = 0.5f;

        Collider[] colliders = Physics.OverlapSphere(spherePosition, sphereRadius);

        foreach (var target in colliders)
        {
            IInteractionDialogue interaction = target.gameObject.GetComponent<IInteractionDialogue>();

            if(interaction != null)
            {
                interaction.TriggerDialogue();
            }
        }
    }

    private void OnInteraction()
    {
        Vector3 spherePosition = transform.position + new Vector3(0, 0.5f, 0);

        float sphereRadius = 0.5f;

        Collider[] colliders = Physics.OverlapSphere(spherePosition, sphereRadius);

        foreach(var collider in colliders)
        {
            IInteractionItem interaction = collider.gameObject.GetComponent<IInteractionItem>();

            if(interaction != null)
            {
                interaction.InteractionItem();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Soul")
        {
            DropSoul soul = other.gameObject.GetComponent<DropSoul>();

            int soulValue = soul.GetSoulValue();

            InventoryManager.Instance.SetSoulCount(soulValue);
        }
    }
}
