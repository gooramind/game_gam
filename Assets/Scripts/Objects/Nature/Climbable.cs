using UnityEngine;

namespace FF.Objects.Nature
{
    /// <summary>
    /// 이 오브젝트(나무 등)에 플레이어가 닿아있는 동안 등반(클라이밍)이 가능함을 표시한다.
    /// 실제 등반 이동 처리는 PlayerController가 담당하고, 이 컴포넌트는 "지금 등반 구역 안에 있는지"만 알려준다.
    /// Collider2D를 Is Trigger로 설정해서 사용한다.
    /// </summary>
    public class Climbable : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            FF.Player.ClimbZoneTracker tracker = other.GetComponent<FF.Player.ClimbZoneTracker>();
            if (tracker != null)
            {
                tracker.EnterClimbZone();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            FF.Player.ClimbZoneTracker tracker = other.GetComponent<FF.Player.ClimbZoneTracker>();
            if (tracker != null)
            {
                tracker.ExitClimbZone();
            }
        }
    }
}