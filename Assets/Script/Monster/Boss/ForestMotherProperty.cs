using System.Collections.Generic;
using UnityEngine;

public class ForestMotherProperty
{
    public IMotherPattern CurrentPattern { get; set; }
    public List<IMotherPattern> PatternList { get; set; }
    public GameObject PlayerObject { get; set; }
    public bool isStunned { get; set; }
    public bool IsPlaying { get; set; }
    public bool IsColliding { get; set; }
    public float CurrentHP { get; set; }
    public float CurrentSpeed { get; set; }
    public float CurrentPower { get; set; }
}
