using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Mother_Hyper : Mother, IMotherPattern
{
    private Transform _playerTransform;
    private CapsuleCollider _motherCollider;
    private WaitForSeconds _hyperDelayTime;

    private Vector3 _movePosition;
    private Vector3 _myPosition;
    private Vector3 _rotateDirection;

    private int _collidingCount;
    private float _rotationSpeed;
    private bool _rotation;
    private bool _isEnd;

    public void InitializeOnAwake(ForestMother mother, ForestMotherProperty property)
    {
        GetComponent(mother, property);

        _hyperDelayTime = new WaitForSeconds(0.5f);
        _agent.speed = _property.CurrentSpeed;
        _agent.updateRotation = false;
        _myPosition = mother.transform.position;    
        _collidingCount = 0;
        _rotationSpeed = 360f;
    }

    public void OnStart()
    {
        _property.IsPlaying = true;

        _playerTransform = _property.PlayerObject.transform;

        _motherCollider.isTrigger = true;        

        _animator.SetTrigger(_hyperTrigger);

        _mother.StartCoroutine(HyperDelay());
    }

    public void OnUpdate()
    {
        if (_collidingCount > 2)
        {
            _agent.SetDestination(_myPosition);

            float distance = Vector3.Distance(_mother.transform.position, _myPosition);

            if (distance <= 0.1f && !_isEnd)
            {
                _rotation = false;

                _animator.SetBool(_hyperBool, false);

                _mother.StartCoroutine(EndAnimation());
            }
        }

        RotateHyper();
    }

    private IEnumerator EndAnimation()
    {
        _isEnd = true;

        yield return new WaitForSeconds(2.0f);

        _property.IsPlaying = false;
    }

    public bool IsRunning()
    {
        bool isPlaying = _property.IsPlaying;

        if (isPlaying)
        {
            return true;
        }

        return false;
    }

    public void OnEnd()
    {
        _collidingCount = 0;

        _motherCollider.isTrigger = false;

        _isEnd = false;
    }

    private void GetComponent(ForestMother mother, ForestMotherProperty property)
    {
        _mother = mother;
        _property = property;
        _animator = mother.GetComponent<Animator>();
        _agent = mother.GetComponent<NavMeshAgent>();
        _motherCollider = mother.GetComponent<CapsuleCollider>();
        _motherRig = mother.transform.GetComponentInChildren<Rig>();
    }

    private IEnumerator HyperDelay()
    {
        yield return new WaitUntil(() =>
        {
            int layerIndex = _animator.GetLayerIndex("LowerLayer");

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(layerIndex);

            return stateInfo.IsName("HyperSpin");
        });

        yield return _hyperDelayTime;

        _rotation = true;

        _movePosition = MoveToPlayer(_playerTransform);

        _agent.SetDestination(_movePosition);
    }

    public void Colliding(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamaged hit = other.gameObject.GetComponent<IDamaged>();

            if(hit != null)
            {
                hit.TakeDamage(_property.CurrentPower);
            }
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _collidingCount++;

            _movePosition = MoveToPlayer(_playerTransform);

            _agent.SetDestination(_movePosition);
        }
    }

    private Vector3 MoveToPlayer(Transform playerTransform)
    {
        Vector3 rayDirection = (playerTransform.position - _mother.transform.position).normalized;

        Vector3 rayPosition = _mother.transform.position + new Vector3(0f, 0.5f, 0f);

        RaycastHit[] hits = Physics.RaycastAll(rayPosition, rayDirection, 80f, LayerMask.GetMask("Wall"));

        if(hits.Length > 0)
        {
            float backDistance = 2f;

            Vector3 movePosition = hits[0].point - rayDirection * backDistance;

            return movePosition;
        }

        Debug.Log("레이가 감지하지 못함");
        return playerTransform.position;
    }

    private void RotateHyper()
    {
        if (_rotation)
        {
            _rotateDirection = Vector3.down;

            Quaternion rotation = Quaternion.Euler(_rotateDirection * _rotationSpeed * Time.deltaTime);

            _mother.transform.rotation = Quaternion.RotateTowards(_mother.transform.rotation,
                _mother.transform.rotation * rotation, _rotationSpeed * Time.deltaTime);
        }
    }
}
