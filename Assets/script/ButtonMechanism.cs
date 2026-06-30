using System.Collections;
using UnityEngine;

public class ButtonMechanism : MonoBehaviour
{
    public Sprite unpushedSprite;
    public Sprite pushedSprite;
    public float resetDelay = 0.5f;

    public ChestMechanism linkedChest;

    private SpriteRenderer spriteRenderer;
    private Coroutine resetCoroutine;
    public Vector3 pushedOffset = new Vector3(0, -0.1f, 0); // 눌렸을 때 위치 이동값
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = unpushedSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.sprite = pushedSprite;
        transform.localPosition += pushedOffset; // 위치를 살짝 아래로 조정
        // 🌟 [추가됨] 로그 출력: 누가 닿았는지 무조건 알려줌!
        Debug.Log("감지된 오브젝트 이름: " + collision.gameObject.name + " / 태그: " + collision.tag);

        if (collision.CompareTag("Player"))
        {
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
                resetCoroutine = null;
            }

            spriteRenderer.sprite = pushedSprite;

            if (linkedChest != null)
            {
                linkedChest.OpenChest();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            resetCoroutine = StartCoroutine(ResetButtonRoutine());
        }
    }

    IEnumerator ResetButtonRoutine()
    {
        yield return new WaitForSeconds(resetDelay);
        spriteRenderer.sprite = unpushedSprite;
        transform.localPosition -= pushedOffset; // 원래 위치로 복귀
        spriteRenderer.sprite = unpushedSprite;
        resetCoroutine = null;

        if (linkedChest != null)
        {
            linkedChest.CloseChest();
        }
    }
}