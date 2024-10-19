using System;
using UnityEngine;

public class HitSwitch : MonoBehaviour
{
    public Action<HitSwitch> _swithAction;

    [Header("Key")]
    [SerializeField] private GimikEnum _key;

    [Header("GameObject")]
    [SerializeField] private GameObject EventObject;

    public void SwitchEvent()
    {
        var gimik = GimikManager.Instance.Gimik;

        if(gimik.TryGetValue(_key, out Action<GameObject> gimikEvent))
        {
            gimikEvent.Invoke(EventObject);

            _swithAction?.Invoke(this);
        }
    }
}
