using UnityEngine;

namespace FF.Systems
{
    /// <summary>
    /// 스테이지 잠금/해제 상태를 저장하고 불러온다.
    /// PlayerPrefs를 사용하므로 게임을 껐다 켜도 진행 상황이 유지된다.
    /// MonoBehaviour가 아니라 어디서든 바로 호출해서 쓸 수 있는 static 클래스다.
    /// </summary>
    public static class StageProgress
    {
        private const string UnlockedStageKey = "FF_HighestUnlockedStage";

        // 스테이지 1은 항상 열려있어야 하므로 기본값 1
        public static int HighestUnlockedStage
        {
            get => PlayerPrefs.GetInt(UnlockedStageKey, 1);
            private set => PlayerPrefs.SetInt(UnlockedStageKey, value);
        }

        public static bool IsUnlocked(int stageNumber)
        {
            return stageNumber <= HighestUnlockedStage;
        }

        /// <summary>
        /// stageNumber 스테이지를 깼을 때 호출한다. 그 다음 스테이지를 잠금 해제한다.
        /// </summary>
        public static void UnlockStage(int stageNumber)
        {
            int nextStage = stageNumber + 1;
            if (nextStage > HighestUnlockedStage)
            {
                HighestUnlockedStage = nextStage;
                PlayerPrefs.Save();
            }
        }

        // 테스트/디버그용 - 진행 상황을 처음으로 되돌린다
        public static void ResetProgress()
        {
            HighestUnlockedStage = 1;
            PlayerPrefs.Save();
        }
    }
}