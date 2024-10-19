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
            _multiAimConstraintData = _multiAimConstraint.data;

            WeightedTransform weightedTransform = new WeightedTransform();
            weightedTransform.transform = player.transform;
            weightedTransform.weight = 1f;

            var sources = _multiAimConstraintData.sourceObjects;
            sources.Add(weightedTransform);

            _multiAimConstraintData.sourceObjects = sources;

            _multiAimConstraint.data = _multiAimConstraintData;
        }
    }
}
