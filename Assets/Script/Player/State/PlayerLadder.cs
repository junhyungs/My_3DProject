using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLadder : PlayerState, IPlayerState<PlayerLadder>
{
    public PlayerLadder(NewPlayer player) : base(player)
    {
        player.StartCoroutine(LoadData("P101"));

        _playerInput = player.GetComponent<PlayerInput>();
        _ladderAction = _playerInput.actions["Move"];
    }

    private PlayerInput _playerInput;
    private InputAction _ladderAction;

    private readonly int _climb = Animator.StringToHash("Ladder");
    private readonly int _climb_Top = Animator.StringToHash("LadderTop");
    private readonly int _climbSpeed = Animator.StringToHash("LadderSpeed");

    private float _ladderlowestPointY;
    private float _ladderhighestPointY;
    private float _ladderSpeed;
    private bool _isClimb;

    private Vector3 _climbVector;

    public void OnStateEnter()
    {
        _rigidbody.useGravity = false;

        _isClimb = true;

        _playerAnimator.SetBool(_climb, _isClimb);
    }

    public void OnStateFixedUpdate()
    {
        ClimbMove();
    }

    public void OnStateExit()
    {
        _input.SetLadder(false);

        _rigidbody.useGravity = true;
    }

    protected override IEnumerator LoadData(string id)
    {
        yield return new WaitWhile(() => { return DataManager.Instance.GetData(id) == null; });

        _data = DataManager.Instance.GetData(id) as PlayerData;

        _ladderSpeed = _data.LadderSpeed - 2f;
    }

    public void SetLadderLength((float lowestPointY, float highestPointY) ladderLength)
    {
        _ladderlowestPointY = ladderLength.lowestPointY;
        _ladderhighestPointY = ladderLength.highestPointY;
    }

    private void ClimbMove()
    {
        if (!_isClimb)
        {
            return;
        }

        if(_ladderAction.ReadValue<Vector2>() == Vector2.zero)
        {
            ResetVector3();

            SetClimbAnimationSpeed(_climbVector.y);
           
            return;
        }

        _climbVector = new Vector3(0f, _ladderAction.ReadValue<Vector2>().y, 0f).normalized;

        SetClimbAnimationSpeed(_climbVector.y);

        _rigidbody.velocity = _climbVector * _ladderSpeed;

        CheckLadder();
    }

    private void SetClimbAnimationSpeed(float value)
    {
        _playerAnimator.SetFloat(_climbSpeed, value);
    }

    private void CheckLadder()
    {
        if (_player.transform.position.y >= _ladderhighestPointY)
        {
            LadderExit(true);
        }
        else if (_player.transform.position.y < _ladderlowestPointY)
        {
            LadderExit(false);
        }
    }

    private void LadderExit(bool reachedTop)
    {
        _isClimb = false;

        ResetVector3();

        if (reachedTop)
        {
            _playerAnimator.SetTrigger(_climb_Top);
        }
        else
        {
            _playerAnimator.SetBool(_climb, _isClimb);

            _state.ChangePlayerState(State.Idle);
        }

    }

    private void ResetVector3()
    {
        _rigidbody.velocity = Vector3.zero;

        _climbVector = Vector3.zero;
    }
}
