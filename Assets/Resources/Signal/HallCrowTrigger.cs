using UnityEngine;

public class HallCrowTrigger : MonoBehaviour
{
    private bool _isTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")
            && !_isTrigger)
        {
            var timeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.HallCrow);

            if(timeLine != null)
            {
                GameManager.Instance.Player.SetActive(false);

                _isTrigger = true;

                timeLine.Play();
            }
        }
    }
}
