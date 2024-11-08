using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamged
{
    public void TakeDamage(float damage); 
}

public interface IStateControl
{
    public IEnumerator Test();
}
