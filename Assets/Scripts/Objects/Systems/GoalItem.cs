using UnityEngine;
using UnityEngine.SceneManagement;
using FF.Systems; // 🌟 StageProgress 참조를 위해 추가

public class GoalItem : MonoBehaviour
{
    [Header("스테이지 해금 설정")]
    [Tooltip("씬 이름에서 숫자 앞에 붙는 글자 (예: 씬 이름이 'Stage1'이면 'Stage' 입력)")]
    public string sceneNamePrefix = "Stage";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 아이템에 닿았을 때
        if (collision.CompareTag("Player"))
        {
            Debug.Log("클리어 아이템 획득!");

            // 1. 현재 씬 이름 가져오기
            string currentSceneName = SceneManager.GetActiveScene().name;

            // 2. 씬 이름이 설정한 접두사("Stage" 등)로 시작하는지 확인
            if (currentSceneName.StartsWith(sceneNamePrefix))
            {
                // 설정한 글자를 지우고 숫자만 뽑아냄
                string numberString = currentSceneName.Replace(sceneNamePrefix, "");

                if (int.TryParse(numberString, out int currentStageNumber))
                {
                    // 🌟 StageProgress로 통일된 방식으로 다음 스테이지 해금
                    StageProgress.UnlockNextStage(currentStageNumber);
                }
                else
                {
                    Debug.LogWarning("씬 이름에서 스테이지 번호를 읽지 못했습니다: " + currentSceneName);
                }
            }

            // --- 기존 로직 (Finish UI 띄우기) ---
            FinishUIManager uiManager = Object.FindFirstObjectByType<FinishUIManager>();
            if (uiManager != null)
            {
                uiManager.ShowFinishUI();
            }

            // 아이템 삭제
            Destroy(gameObject);
        }
    }
}