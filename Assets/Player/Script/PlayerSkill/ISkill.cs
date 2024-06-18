using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    public void UseSkill(GameObject spawnPositionObj);
    public void Fire(GameObject spawnPositionObj, bool isFire);
}

