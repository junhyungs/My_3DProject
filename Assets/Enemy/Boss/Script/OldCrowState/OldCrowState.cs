
using UnityEngine;

public abstract class OldCrowState : Boss_BaseState
{
    protected OldCrow _oldCrow;
    public OldCrowState(OldCrow oldCrow)
    {
        _oldCrow = oldCrow;
    }
}

public class OldCrow_Dash : OldCrowState
{
    public OldCrow_Dash(OldCrow oldCrow) : base(oldCrow) { }
    //방향은 항상 플레이어를 향해서 움직이고 플레이어와 나의 각도가 150?도 정도 차이가 나면 '턴'을 하는것 같음. 
    //네브메쉬 에이전트를 사용하는 부분도 있는것 같고
    private Vector3 _targetPosition;

    public override void StateEnter()
    {
        
    }

    public override void StateUpdate()
    {
        
    }

    public override void StateExit()
    {
        
    }

    private void Turn()
    {

    }

    private float GetAngle()
    {

    }

    private float GetTargetDistance()
    {

    }

    private void OnAnimatorMove()
    {
        
    }
}

public class OldCrow_Jump : OldCrowState
{
    public OldCrow_Jump(OldCrow oldCrow) : base(oldCrow) { }
    
}

//Skill
public class OldCrow_MegaDash : OldCrowState
{
    public OldCrow_MegaDash(OldCrow oldCrow) : base(oldCrow) { }
    
}
public class OldCrow_Scream : OldCrowState
{
    public OldCrow_Scream(OldCrow oldCrow) : base(oldCrow) { }
    
}
public class OldCrow_Egg : OldCrowState
{
    public OldCrow_Egg(OldCrow oldCrow) : base(oldCrow) { }
    
}
