using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishUIManager : MonoBehaviour
{
    [Header("연결할 UI 패널")]
    public GameObject finishPanel;

    [Header("씬 이름 설정")]
    public string stagePrefix = "Stage";   // 🌟 씬 이름 접두사 (Stage1, Stage2...)
    public string menuSceneName = "MenuScene";

    private void Start()
    {
        if (finishPanel != null)
        {
            finishPanel.SetActive(false);
        }
    }

    public void ShowFinishUI()
    {
        if (finishPanel != null)
        {
            finishPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // 🌟 'Next Stage' 버튼: 현재 스테이지 번호 +1 로 이동
    public void OnNextStageClicked()
    {
        Time.timeScale = 1f;

        int nextNumber = GetCurrentStageNumber() + 1;
        string nextSceneName = stagePrefix + nextNumber;

        Debug.Log("다음 스테이지로 이동: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

    public void OnMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    // 🌟 현재 씬 이름에서 숫자만 뽑아낸다 (예: "Stage3" → 3)
    private int GetCurrentStageNumber()
    {
        string currentName = SceneManager.GetActiveScene().name;

        // 접두사("Stage")를 떼어내고 남은 숫자 부분만 추출
        string numberPart = currentName.Replace(stagePrefix, "");

        if (int.TryParse(numberPart, out int number))
        {
            return number;
        }

        // 숫자를 못 읽으면 경고하고 1로 처리 (안전장치)
        Debug.LogWarning("현재 씬 이름에서 스테이지 번호를 읽지 못했습니다: " + currentName);
        return 1;
    }
}