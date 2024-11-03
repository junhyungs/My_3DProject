using System.Data;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MotherAim : MonoBehaviour
{
    private MultiAimConstraint _multiAimConstraint;
    private MultiAimConstraintData _multiAimConstraintData;

    private void Awake()
    {
        _multiAimConstraint = GetComponent<MultiAimConstraint>();
    }

    private void Start()
    {
        GameObject player = GameManager.Instance.Player;

        if (player != null)
        {
            var data = _multiAimConstraint.data;

            var sources = data.sourceObjects;

            sources.Clear();

            sources.Add(new WeightedTransform(player.transform, 1f));

            data.sourceObjects = sources;

            _multiAimConstraint.data = data;
        }
    }
}
