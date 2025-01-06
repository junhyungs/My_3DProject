using System;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : Stage
{
    [Header("BossDoor")]
    [SerializeField] private BossDoor _bossDoor;

    [Header("BossTrigger")]
    [SerializeField] private BossTrigger _bossTrigger;

    [Header("Door")]
    [SerializeField] private CutSceneDoor _door;

    [Header("RespawnPoint")]
    [SerializeField] private Transform _respawnPoint;

    [Header("RespawnDisableObject")]
    [SerializeField] private GameObject[] _respawnDisableObjects;

    private HashSet<HitSwitch> _hitSwitchSet;

    private void Awake()
    {
        RespawnPoint = _respawnPoint;
    }

    private void OnEnable()
    {
        if(_door != null)
        {
            _door.CloseDoor();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

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
            _bossDoor.OpenDoor();
        }
    }

    public override void OnEnableGimikObject()
    {
        _bossDoor.OpenDoor();
        _bossTrigger.OnEnableBossTrigger();

        foreach (var disableObject in _respawnDisableObjects)
        {
            if(disableObject != null)
            {
                disableObject.SetActive(false);
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
}
