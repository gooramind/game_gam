using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필수!

public class BackButton : MonoBehaviour
{
    // 이동할 씬의 이름을 설정창(인스펙터)에서 입력받음
    public string targetSceneName;

    public void OnBackButtonClicked()
    {
        // 입력받은 이름의 씬으로 전환
        SceneManager.LoadScene(targetSceneName);
    }
}