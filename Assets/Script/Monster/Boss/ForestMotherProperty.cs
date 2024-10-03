using System.Collections.Generic;
using UnityEngine;

public class ForestMotherProperty
{
    public Queue<IMotherPattern> PatternQueue { get; set; }
    public GameObject PlayerObject { get; set; }
    public Transform SoulTransform { get; set; }
    public float CurrentHP { get; set; }
    public float CurrentSpeed { get; set; }
    public float CurrentPower { get; set; }
}
