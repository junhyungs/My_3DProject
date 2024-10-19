using System;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : Stage
{
    [Header("BossDoor")]
    [SerializeField] private GameObject _bossDoor;

    private HashSet<HitSwitch> _hitSwitchSet;

    private void Start()
    {
        InitializeHitSwitch();
    }

    private void InitializeHitSwitch()
    {
        _hitSwitchSet = new HashSet<HitSwitch>();

        HitSwitch[] hitSwitch = transform.GetComponentsInChildren<HitSwitch>();

        for (int i = 0; i < hitSwitch.Length; i++)
        {
            hitSwitch[i]._swithAction += RemoveHitSwitch;

            _hitSwitchSet.Add(hitSwitch[i]);
        }
    }

    private void RemoveHitSwitch(HitSwitch hitSwitch)
    {
        if(!_hitSwitchSet.Contains(hitSwitch))
        {
            return;
        }

        _hitSwitchSet.Remove(hitSwitch);

        if (_hitSwitchSet.Count <= 0)
        {
            var moveGimik = GimikManager.Instance.Gimik;

            if(moveGimik.TryGetValue(GimikEnum.OpenDoor, out Action<GameObject> moveEvent))
            {
                moveEvent?.Invoke(_bossDoor);
            }
        }
    }

    public override void SetMapData(MapData data)
    {
        base.SetMapData(data);
    }

    public override void CreateMonsters()
    {
        base.CreateMonsters();
    }

    public override void SpawnItems()
    {
        base.SpawnItems();
    }

    public override void StartPosition()
    {
        return;
    }
}
