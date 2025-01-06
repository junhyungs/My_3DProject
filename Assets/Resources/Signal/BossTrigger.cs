using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void OnEnableBossTrigger()
    {
        if (!_boxCollider.enabled)
        {
            _boxCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var timeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.ForestMotherIntro);

            if(timeLine != null)
            {
                GameManager.Instance.PlayerLock(true);

                timeLine.Play();

                gameObject.SetActive(false);
            }
        }
    }
}
