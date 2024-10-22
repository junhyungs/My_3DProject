using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Boss/Mother/Initialize")]
public class Mother_Initialize : Action
{
    private ForestMother _mother;
    private ForestMotherProperty _property;

    private TaskStatus _status = TaskStatus.Success;

    public override void OnAwake()
    {
        _mother = Owner.gameObject.GetComponent<ForestMother>();

        InitializeMother(_mother);
    }

    public override TaskStatus OnUpdate()
    {
        return _status;
    }

    private void InitializeMother(ForestMother mother)
    {
        _property = new ForestMotherProperty();

        var data = BossManager.Instance.MotherData;

        _property.CurrentHP = data.Health;
        _property.CurrentPower = data.Power;
        _property.CurrentSpeed = data.Speed;
        _property.DownHealth = data.DownHealth;
        _property.PlayerObject = GameManager.Instance.Player;

        mother.Property = _property;

        InitializePattern(mother, mother.Property);
    }

    private void InitializePattern(ForestMother mother, ForestMotherProperty property)
    {
        List<IMotherPattern> newPatternList = new List<IMotherPattern>
        {
            new Mother_Slam(),
            new Mother_Slam(),
            new Mother_SlamSlow(),
            new Mother_Hyper(),
            new Mother_Lift()
        };

        foreach(var pattern in newPatternList)
        {
            pattern.InitializeOnAwake(mother, property);
        }

        property.PatternList = newPatternList;
    }
}
