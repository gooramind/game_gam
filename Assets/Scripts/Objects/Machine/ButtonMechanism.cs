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
        if (collision.CompareTag("Player"))
        {
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
                resetCoroutine = null;
            }

            spriteRenderer.sprite = pushedSprite;
            // 🌟 버튼 눌릴 때 위치 이동
            transform.localPosition += pushedOffset;

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