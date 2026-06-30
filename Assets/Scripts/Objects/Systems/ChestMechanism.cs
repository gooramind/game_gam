using UnityEngine;

public class ChestMechanism : MonoBehaviour
{
    private Animator animator; // 🌟 애니메이션 제어를 위한 애니메이터 컴포넌트

    [Header("아이템 소환 설정")]
    public GameObject goalItemPrefab;
    public Vector3 spawnOffset = new Vector3(0f, 1f, 0f);

    private bool isOpened = false;
    private GameObject spawnedItem;

    void Start()
    {
        // 🌟 Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("GameObject에 Animator 컴포넌트를 찾을 수 없습니다!");
        }
    }

    public void OpenChest()
    {
        if (isOpened) return;
        isOpened = true;

        // 🌟 애니메이터의 IsOpen 파라미터를 true로 설정하여 열림 애니메이션 재생
        if (animator != null)
        {
            animator.SetBool("IsOpen", true);
        }

        // 아이템 소환 (소환한 아이템을 변수에 저장)
        if (goalItemPrefab != null)
        {
            spawnedItem = Instantiate(goalItemPrefab, transform.position + spawnOffset, Quaternion.identity);
            Debug.Log("아이템이 소환되었습니다!");
        }
    }

    public void CloseChest()
    {
        if (!isOpened) return;
        isOpened = false;

        // 🌟 애니메이터의 IsOpen 파라미터를 false로 설정하여 닫힘 애니메이션 재생
        if (animator != null)
        {
            animator.SetBool("IsOpen", false);
        }

        // 소환했던 아이템이 아직 안 먹혔으면 없앤다
        if (spawnedItem != null)
        {
            Destroy(spawnedItem);
            spawnedItem = null;
        }
    }
}