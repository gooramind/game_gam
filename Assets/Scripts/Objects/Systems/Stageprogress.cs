using UnityEngine;

namespace FF.Systems
{
    /// <summary>
    /// 스테이지 잠금/해제 상태를 관리하는 정적 헬퍼 클래스.
    /// PlayerPrefs를 여러 스크립트에서 각자 건드리지 않고, 이 클래스를 통해서만 접근하도록 통일한다.
    /// </summary>
    public static class StageProgress
    {
        private const string SaveKey = "UnlockedStage";
        private const int DefaultUnlockedStage = 1; // 기본적으로 1스테이지는 항상 열려있음

        // 현재까지 해금된 가장 높은 스테이지 번호를 반환
        public static int GetUnlockedStage()
        {
            return PlayerPrefs.GetInt(SaveKey, DefaultUnlockedStage);
        }

        // 해당 스테이지 번호가 플레이 가능한(잠금 해제된) 상태인지 확인
        public static bool IsUnlocked(int stageNumber)
        {
            return stageNumber <= GetUnlockedStage();
        }

        // 특정 스테이지를 클리어했을 때 호출. 다음 스테이지를 해금한다. (이미 더 앞서 있으면 무시)
        public static void UnlockNextStage(int clearedStageNumber)
        {
            int nextStage = clearedStageNumber + 1;
            int currentUnlocked = GetUnlockedStage();

            if (nextStage > currentUnlocked)
            {
                PlayerPrefs.SetInt(SaveKey, nextStage);
                PlayerPrefs.Save();
                Debug.Log($"[StageProgress] 스테이지 {nextStage}까지 해금되었습니다!");
            }
        }

        // 🌟 UnlockNextStage의 별칭. StageManager.cs 등 다른 스크립트가 이 이름으로 호출하기 때문에 추가.
        // 방금 클리어한 스테이지 번호를 넘기면, 그 다음 스테이지가 잠금 해제된다.
        public static void UnlockStage(int clearedStageNumber)
        {
            UnlockNextStage(clearedStageNumber);
        }

        // 개발자 테스트용: 특정 스테이지까지 강제로 해금
        public static void ForceUnlock(int stageNumber)
        {
            PlayerPrefs.SetInt(SaveKey, stageNumber);
            PlayerPrefs.Save();
            Debug.Log($"[StageProgress] (강제) 스테이지 {stageNumber}까지 해금되었습니다!");
        }

        // 진행도 초기화 (R키 리셋 등에서 사용)
        public static void ResetProgress()
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
            Debug.Log("[StageProgress] 진행도가 초기화되었습니다.");
        }
    }
}