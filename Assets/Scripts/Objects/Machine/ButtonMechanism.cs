using System.Collections;
using UnityEngine;

public class ButtonMechanism : MonoBehaviour
{
    public Sprite unpushedSprite;
    public Sprite pushedSprite;
    public float resetDelay = 0.5f;

    // 🌟 위치 보정용 변수
    public Vector3 pushedOffset = new Vector3(0, -0.1f, 0);

    public ChestMechanism linkedChest;

    private SpriteRenderer spriteRenderer;
    private Coroutine resetCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = unpushedSprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 🌟 수정: 태그가 "Player" 이거나 "Stone" 이면 작동하게 변경!
        if (collision.CompareTag("Player") || collision.CompareTag("Stone"))
        {
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
                resetCoroutine = null;
            }

            spriteRenderer.sprite = pushedSprite;
            transform.localPosition += pushedOffset;

            if (linkedChest != null)
            {
                linkedChest.OpenChest();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 🌟 수정: "Player" 이거나 "Stone" 이면 복귀 로직 실행
        if (collision.CompareTag("Player") || collision.CompareTag("Stone"))
        {
            if (gameObject.activeInHierarchy)
            {
                resetCoroutine = StartCoroutine(ResetButtonRoutine());
            }
        }
    }

    IEnumerator ResetButtonRoutine()
    {
        yield return new WaitForSeconds(resetDelay);

        spriteRenderer.sprite = unpushedSprite;
        // 🌟 버튼 원래 위치로 복귀
        transform.localPosition -= pushedOffset;

        resetCoroutine = null;

        if (linkedChest != null)
        {
            linkedChest.CloseChest();
        }
    }
}