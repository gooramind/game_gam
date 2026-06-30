using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    // 중복 실행 방지 (한 번만 발동)
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("피니시 지점 도달!");

            FinishUIManager uiManager = Object.FindFirstObjectByType<FinishUIManager>();
            if (uiManager != null)
            {
                uiManager.ShowFinishUI();
            }
            else
            {
                Debug.LogWarning("FinishUIManager를 찾을 수 없습니다!");
            }
        }
    }
}