using UnityEngine;
using UnityEngine.SceneManagement; // 🌟 씬 전환 및 재시작을 위해 필수!

public class RestartGame : MonoBehaviour
{
    // 🌟 안전장치: 씬이 시작될 때 시간이 멈춰있다면 확실하게 풀어줍니다.
    private void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        // 🌟 편의 기능: 키보드의 ESC 키를 눌러도 즉시 재시작 가능
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestartCurrentScene();
        }
    }

    // 🌟 UI 버튼의 OnClick 이벤트에 연결할 함수
    public void OnRestartButtonClicked()
    {
        RestartCurrentScene();
    }

    // 현재 활성화된 씬을 다시 로드하는 공통 함수
    private void RestartCurrentScene()
    {
        // 🌟 재시작 버튼을 누르는 순간에도 시간을 원래대로 돌려놓습니다.
        Time.timeScale = 1f;

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}   