using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 스타트 씬에서 메뉴 씬으로 넘어가는 함수
    public void GoToMenu()
    {
        // 씬 이름이 정확히 일치해야 합니다.
        SceneManager.LoadScene("MenuScene");
    }

    // 메뉴 씬에서 실제 게임 씬으로 넘어가는 함수 (나중에 사용)
    public void GoToGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}