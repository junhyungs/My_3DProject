using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerHealth _healthComponent;
    private PlayerMoveController _moveController;
    private PlayerAttackController _attackController;
    
    private void OnEnable()
    {
        GameManager.Instance.Player = this.gameObject;
    }

    private void Awake()
    {
        _healthComponent = gameObject.GetComponent<PlayerHealth>();
        _moveController = gameObject.GetComponent<PlayerMoveController>();
        _attackController = gameObject.GetComponent<PlayerAttackController>();
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
            Debug.Log("�÷��̾� �����͸� �������� ���߽��ϴ�.");
            return DataManager.Instance.GetData(id) == null;
        });

        var data = DataManager.Instance.GetData(id) as PlayerData;

        _healthComponent.SetHealthData(data.Health);
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

    public void DialogueLock(bool dialogueLock)
    {
        bool playerLock = dialogueLock ? false : true;

        _moveController.IsAction = playerLock;
        _attackController.IsAction = playerLock;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0,0.5f,0), 0.5f);
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
