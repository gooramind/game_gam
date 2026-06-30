using UnityEngine;

public class GoalItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 아이템에 닿았을 때
        if (collision.CompareTag("Player"))
        {
            Debug.Log("클리어 아이템 획득!");

            // 🌟 씬을 바로 이동하지 않고, 씬에 있는 FinishUIManager를 찾아서 UI를 띄우라고 명령
            FinishUIManager uiManager = Object.FindFirstObjectByType<FinishUIManager>();
            if (uiManager != null)
            {
                uiManager.ShowFinishUI();
            }
            else
            {
                Debug.LogWarning("FinishUIManager를 찾을 수 없습니다!");
            }

            // 아이템은 먹었으니 화면에서 삭제
            Destroy(gameObject);
        }
    }
}