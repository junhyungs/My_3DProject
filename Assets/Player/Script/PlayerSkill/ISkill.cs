using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    public void UseSkill(Transform spawnPosition);
    public void Fire(bool isFire);
}

