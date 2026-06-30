using UnityEngine;
using UnityEngine.SceneManagement; // 🌟 씬 전환 및 재시작을 위해 필수!

public class RestartGame : MonoBehaviour
{
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
        // 현재 열려 있는 씬의 이름을 자동으로 가져옵니다.
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 해당 씬을 다시 로드하여 처음 상태로 리셋합니다.
        SceneManager.LoadScene(currentSceneName);
    }
}