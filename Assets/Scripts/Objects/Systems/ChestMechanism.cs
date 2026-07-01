using UnityEngine;

public class ChestMechanism : MonoBehaviour
{
    [Header("아이템 소환 설정")]
    public GameObject goalItemPrefab;
    [Tooltip("상자 위치에서 얼마나 위에 아이템을 띄울지")]
    public Vector3 spawnOffset = new Vector3(0f, 1f, 0f);

    [Header("애니메이션 설정")]
    [Tooltip("Animator Controller 안에 있는, 열림 여부를 나타내는 Bool 파라미터 이름")]
    public string isOpenParam = "IsOpen";

    private Animator animator;
    private bool isOpened = false;
    private GameObject spawnedItem;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("[ChestMechanism] Animator를 찾을 수 없습니다!");
        }
    }

    public void OpenChest()
    {
        if (isOpened) return;
        isOpened = true;

        // 🌟 스프라이트 직접 교체 대신, 애니메이터에게 "열려라" 신호만 보냄
        if (animator != null)
        {
            animator.SetBool(isOpenParam, true);
        }

        if (goalItemPrefab != null)
        {
            Vector3 spawnPosition = transform.position + spawnOffset;
            spawnedItem = Instantiate(goalItemPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("[ChestMechanism] 아이템이 소환되었습니다! 위치: " + spawnPosition);
        }
    }

    public void CloseChest()
    {
        if (!isOpened) return;
        isOpened = false;

        // 🌟 스프라이트 직접 교체 대신, 애니메이터에게 "닫혀라" 신호만 보냄
        if (animator != null)
        {
            animator.SetBool(isOpenParam, false);
        }

        if (spawnedItem != null)
        {
            Destroy(spawnedItem);
            spawnedItem = null;
        }
    }
}