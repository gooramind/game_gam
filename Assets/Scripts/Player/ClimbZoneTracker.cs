using UnityEngine;

namespace FF.Player
{
    /// <summary>
    /// 플레이어가 현재 등반(클라이밍) 가능 구역 안에 있는지 추적한다.
    /// Climbable 컴포넌트가 진입/이탈을 알려주면 카운트를 올리고 내린다.
    /// (등반 구역이 겹칠 수도 있으니 bool 하나가 아니라 카운트로 관리)
    /// 실제 등반 이동 처리는 PlayerController에서 이 컴포넌트의 IsInClimbZone 값을 읽어서 한다.
    /// </summary>
    public class ClimbZoneTracker : MonoBehaviour
    {
        private int climbZoneCount = 0;

        public bool IsInClimbZone => climbZoneCount > 0;

        public void EnterClimbZone()
        {
            climbZoneCount++;
        }

        public void ExitClimbZone()
        {
            climbZoneCount = Mathf.Max(0, climbZoneCount - 1);
        }
    }
}