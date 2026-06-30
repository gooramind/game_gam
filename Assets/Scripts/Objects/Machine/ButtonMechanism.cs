using System.Collections;
using UnityEngine;

public class ButtonMechanism : MonoBehaviour
{
    public Sprite unpushedSprite;
    public Sprite pushedSprite;
    public float resetDelay = 0.5f;
    public Vector3 pushedOffset = new Vector3(0, -0.1f, 0);
    public bool isToggleButton = false;
    public ChestMechanism linkedChest;

    private SpriteRenderer spriteRenderer;
    private Coroutine resetCoroutine;
    private bool isPushed = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetButtonToUnpushedState();
    }

    // 🌟 버튼을 누를 수 있는 대상인지 판별 (플레이어 또는 돌만)
    private bool CanPushButton(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return true;
        if (collision.GetComponent<StoneTransformable>() != null) return true;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanPushButton(collision))
        {
            if (isToggleButton)
            {
                isPushed = !isPushed;
                if (isPushed)
                {
                    ApplyPushedVisuals();
                    if (linkedChest != null) linkedChest.OpenChest();
                }
                else
                {
                    ApplyUnpushedVisuals();
                    if (linkedChest != null) linkedChest.CloseChest();
                }
            }
            else
            {
                if (resetCoroutine != null) { StopCoroutine(resetCoroutine); resetCoroutine = null; }
                ApplyPushedVisuals();
                if (linkedChest != null) linkedChest.OpenChest();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CanPushButton(collision))
        {
            if (!isToggleButton && gameObject.activeInHierarchy)
            {
                resetCoroutine = StartCoroutine(ResetButtonRoutine());
            }
        }
    }

    IEnumerator ResetButtonRoutine()
    {
        yield return new WaitForSeconds(resetDelay);
        ApplyUnpushedVisuals();
        if (linkedChest != null) linkedChest.CloseChest();
        resetCoroutine = null;
    }

    private void ApplyPushedVisuals()
    {
        spriteRenderer.sprite = pushedSprite;
        transform.localPosition += pushedOffset;
    }

    private void ApplyUnpushedVisuals()
    {
        spriteRenderer.sprite = unpushedSprite;
        transform.localPosition -= pushedOffset;
    }

    public void ResetButtonToUnpushedState()
    {
        if (resetCoroutine != null) { StopCoroutine(resetCoroutine); resetCoroutine = null; }
        spriteRenderer.sprite = unpushedSprite;
        transform.localPosition -= pushedOffset;
        isPushed = false;
        if (linkedChest != null) linkedChest.CloseChest();
    }
}