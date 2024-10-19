using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
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
