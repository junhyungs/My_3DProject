using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallCrowTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var timeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.HallCrow);

            if(timeLine != null)
            {
                GameManager.Instance.Player.SetActive(false);

                timeLine.Play();
            }
        }
    }
}
