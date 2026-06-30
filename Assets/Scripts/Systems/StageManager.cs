using UnityEngine;

namespace FF.Systems
{
    /// <summary>
    /// 현재 스테이지의 진행 상태를 관리한다. 지금은 클리어 로그만 찍지만,
    /// 나중에 UI 단계에서 결과화면 표시, 다시하기 등과 연결할 예정이다.
    /// 씬에 빈 오브젝트 하나를 만들어서 이 컴포넌트를 붙여두면 된다.
    /// </summary>
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private int stageNumber = 1; // 이 씬이 몇 번째 스테이지인지 (스테이지 선택 화면과 번호를 맞춰야 함)

        public static StageManager Instance { get; private set; }

        private bool isStageCompleted = false;

        private void Awake()
        {
            Instance = this;
        }

        public void CompleteStage()
        {
            if (isStageCompleted) return;
            isStageCompleted = true;

            StageProgress.UnlockStage(stageNumber); // 다음 스테이지 잠금 해제

            Debug.Log($"스테이지 {stageNumber} 성공! 다음 스테이지가 잠금 해제됐습니다.");
            // TODO: UI 단계에서 결과화면 표시, 다음 스테이지 이동 등을 여기에 연결
        }
    }
}