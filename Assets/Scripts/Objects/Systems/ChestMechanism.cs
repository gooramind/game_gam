using UnityEngine;

public class ChestMechanism : MonoBehaviour
{
    public Sprite closedSprite;
    public Sprite openSprite;
    private SpriteRenderer spriteRenderer;

    [Header("아이템 소환 설정")]
    public GameObject goalItemPrefab;
    public Vector3 spawnOffset = new Vector3(0f, 1f, 0f);

    private bool isOpened = false;

    // 🌟 추가: 소환한 아이템을 기억해두는 변수
    private GameObject spawnedItem;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer를 찾을 수 없습니다!");
        }
    }

    public void OpenChest()
    {
        if (isOpened) return;
        isOpened = true;

        // 상자 열기 이미지 교체
        if (spriteRenderer != null && openSprite != null)
        {
            spriteRenderer.sprite = openSprite;
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

        // 상자 닫기 이미지로 교체
        if (spriteRenderer != null && closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }

        // 🌟 추가: 소환했던 아이템이 아직 살아있으면(=안 먹었으면) 없앤다.
        // 플레이어가 이미 먹었다면 spawnedItem이 Destroy되어 null이므로 그냥 둔다.
        if (spawnedItem != null)
        {
            Destroy(spawnedItem);
            spawnedItem = null;
        }
    }
}