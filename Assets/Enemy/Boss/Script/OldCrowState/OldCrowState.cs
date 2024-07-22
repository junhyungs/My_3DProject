
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

    public override void StateEnter()
    {
        
    }

    public override void StateUpdate()
    {
        
    }

    public override void StateExit()
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
