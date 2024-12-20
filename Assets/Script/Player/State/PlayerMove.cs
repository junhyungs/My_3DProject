using System.Collections;
using UnityEngine;

public class PlayerMove : PlayerState, IPlayerState<PlayerMove>
{
    public PlayerMove(NewPlayer player) : base(player)
    {
        InitializePlayerMoveState();
    }

    private void InitializePlayerMoveState()
    {
        _player.StartCoroutine(LoadData("P101"));
    }

    protected override IEnumerator LoadData(string id)
    {
        yield return new WaitWhile(() => { return DataManager.Instance.GetData(id) == null; });

        _data = DataManager.Instance.GetData(id) as PlayerData;

        _moveSpeed = _data.Speed;
        _speedOffSet = _data.SpeedOffSet;
    }

    private readonly int _blendTree = Animator.StringToHash("MoveSpeed");

    private float _moveSpeed;
    private float _changeSpeedValue = 100f;
    private float _currentHorizontalSpeed;
    private float _targetSpeed;
    private float _currentPlayerSpeed;
    private float _speedOffSet;
    private float _rotationVelocity;
    private float _targetRotation;
    private float _smoothRotation;

    public void OnStateEnter()
    {
        
    }

    public void OnStateFixedUpdate()
    {
        InputCheck();
        Movement();
    }

    public void OnStateExit()
    {
        
    }

    private void InputCheck()
    {
        IsGround();

        if (_input.IsRoll)
        {
            _state.ChangePlayerState(State.Roll);
        }
    }

    private void Movement()
    {
        _targetSpeed = _moveSpeed;
        
        if(_input.InputValue == Vector2.zero)
        {
            _targetSpeed = 0f;

            if (Mathf.Abs(_currentPlayerSpeed - _targetSpeed) < 0.01f)
            {
                _state.ChangePlayerState(State.Idle);

                return;
            }
        }

        _currentHorizontalSpeed = new Vector3(_rigidbody.velocity.x,
            0f, _rigidbody.velocity.z).magnitude;

        if(_currentHorizontalSpeed < _targetSpeed - _speedOffSet ||
            _currentHorizontalSpeed >  _targetSpeed + _speedOffSet)
        {

            _currentPlayerSpeed = Mathf.Lerp(_currentHorizontalSpeed, _targetSpeed,
                _changeSpeedValue * Time.fixedDeltaTime);

            _currentPlayerSpeed = Mathf.Round(_currentPlayerSpeed * 1000f) / 1000f;
        }
        else
        {
            _currentPlayerSpeed = _targetSpeed;
        }

        Vector3 inputDirection = new Vector3(_input.InputValue.x, 0f, _input.InputValue.y).normalized;

        if(inputDirection != Vector3.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

            _smoothRotation = Mathf.SmoothDampAngle(_player.transform.eulerAngles.y, _targetRotation
                , ref _rotationVelocity, 0.12f);

            _player.transform.rotation = Quaternion.Euler(0f, _smoothRotation, 0f);
        }

        _playerAnimator.SetFloat(_blendTree, _currentPlayerSpeed);

        Vector3 moveVector = inputDirection * _currentPlayerSpeed;

        moveVector.y = _rigidbody.velocity.y;

        _rigidbody.velocity = moveVector;
    }
}
