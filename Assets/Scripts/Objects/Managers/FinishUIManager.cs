using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishUIManager : MonoBehaviour
{
    [Header("연결할 UI 패널")]
    public GameObject finishPanel; // 아까 만든 FinishPanel을 여기에 연결

    [Header("이동할 씬 이름")]
    public string nextStageName = "Stage2"; // 다음 스테이지 이름
    public string menuSceneName = "MenuScene"; // 메뉴 씬 이름

    private void Start()
    {
        // 게임 시작 시에는 클리어 창을 숨겨둡니다.
        if (finishPanel != null)
        {
            finishPanel.SetActive(false);
        }
    }

    // 🌟 GoalItem이 획득되었을 때 호출되는 함수
    public void ShowFinishUI()
    {
        if (finishPanel != null)
        {
            finishPanel.SetActive(true);

            // (선택) UI가 떴을 때 게임 캐릭터들이 안 움직이게 하려면 시간 흐름을 멈춥니다.
            Time.timeScale = 0f;
        }
    }

    // 🌟 'Next Stage' 버튼을 눌렀을 때 실행될 함수
    public void OnNextStageClicked()
    {
        Time.timeScale = 1f; // 멈췄던 시간을 다시 흐르게 돌려놓음
        SceneManager.LoadScene(nextStageName);
    }

    // 🌟 'Menu' 버튼을 눌렀을 때 실행될 함수
    public void OnMenuClicked()
    {
        Time.timeScale = 1f; // 멈췄던 시간을 다시 흐르게 돌려놓음
        SceneManager.LoadScene(menuSceneName);
    }
}