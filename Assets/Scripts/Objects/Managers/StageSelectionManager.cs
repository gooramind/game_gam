using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FF.Systems; // 🌟 StageProgress 참조를 위해 추가

public class StageSelectionManager : MonoBehaviour
{
    [Header("스테이지 버튼들 (1부터 5까지 순서대로)")]
    public Button[] stageButtons;

    [Header("🛠️ 개발자 테스트용 (현재 진행도 강제 조작)")]
    [Tooltip("이것을 체크하고 게임을 켜면 아래 설정한 숫자만큼 스테이지가 열립니다.")]
    public bool overrideStage = false;

    [Tooltip("강제로 열어둘 스테이지 번호 (1~5)")]
    [Range(1, 5)]
    public int targetStage = 1;

    void Start()
    {
        // 1. 시간 정상화
        Time.timeScale = 1f;

        // 🌟 2. 개발자 치트 활성화 시: StageProgress를 통해 강제로 진행도 덮어씌움!
        if (overrideStage)
        {
            StageProgress.ForceUnlock(targetStage);
            Debug.Log($"🛠️ [치트 작동] 진행도를 {targetStage} 스테이지까지 강제로 열었습니다!");
        }

        // 3. 저장된 기록 불러오기
        int unlockedStage = StageProgress.GetUnlockedStage();
        Debug.Log("🟢 메뉴 화면 로드 완료! 현재 열린 스테이지: " + unlockedStage);

        // 4. 버튼 켜고 끄기 로직
        for (int i = 0; i < stageButtons.Length; i++)
        {
            if (stageButtons[i] == null) continue;

            int stageLevel = i + 1;
            stageButtons[i].gameObject.SetActive(StageProgress.IsUnlocked(stageLevel));
        }
    }

    // 게임 중 R 키를 누르면 진행도 초기화 후 1스테이지로 리셋
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StageProgress.ResetProgress();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("🚨 데이터 초기화 완료! 1스테이지만 열립니다.");
        }
    }
}