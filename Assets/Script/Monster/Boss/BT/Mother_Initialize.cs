using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Boss/Mother/Initialize")]
public class Mother_Initialize : Action
{
    private ForestMother _mother;
    private ForestMotherProperty _property;

    private bool _initialized;

    private TaskStatus _status = TaskStatus.Success;

    public override void OnAwake()
    {
        _mother = Owner.gameObject.GetComponent<ForestMother>();

        InitializeMother();
    }

    public override void OnStart()
    {
        if (!_initialized)
        {
            List<IMotherPattern> patternList = new List<IMotherPattern>();

            Mother_Slam slamPattern = new Mother_Slam();
            Mother_SlamSlow slamSlowPattern = new Mother_SlamSlow();

            patternList.Add(slamPattern);
            patternList.Add(slamSlowPattern);

            _property.PatternList = patternList;

            _initialized = true;
        }
    }

    public override TaskStatus OnUpdate()
    {
        return _status;
    }

    private void InitializeMother()
    {
        _property = new ForestMotherProperty();

        var data = BossManager.Instance.MotherData;

        _property.CurrentHP = data.Health;
        _property.CurrentPower = data.Power;
        _property.CurrentSpeed = data.Speed;
        _property.PlayerObject = GameManager.Instance.Player;

        _mother.Property = _property;
    }
}
