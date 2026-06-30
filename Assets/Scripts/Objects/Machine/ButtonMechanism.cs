using System.Collections;
using UnityEngine;

public class ButtonMechanism : MonoBehaviour
{
    public Sprite unpushedSprite;
    public Sprite pushedSprite;
    public float resetDelay = 0.5f;
    public Vector3 pushedOffset = new Vector3(0, -0.1f, 0);

    // 🌟 추가: 토글 모드 여부 (상태변환기가 이 값을 조절함)
    public bool isToggleButton = false;

    public ChestMechanism linkedChest;

    private SpriteRenderer spriteRenderer;
    private Coroutine resetCoroutine;

    // 🌟 추가: 토글 모드에서 눌려있는 상태인지 추적하는 변수
    private bool isPushed = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetButtonToUnpushedState(); // 시작할 때는 항상 안 눌린 상태로
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Stone"))
        {
            if (isToggleButton)
            {
                // 🌟 토글 모드 로직
                isPushed = !isPushed; // 상태를 반전시킴

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
                // 🌟 홀드 모드 (기존) 로직
                if (resetCoroutine != null)
                {
                    StopCoroutine(resetCoroutine);
                    resetCoroutine = null;
                }

                ApplyPushedVisuals();
                if (linkedChest != null) linkedChest.OpenChest();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Stone"))
        {
            if (!isToggleButton)
            {
                // 🌟 홀드 모드 (기존) 로직: 발을 떼면 리셋 코루틴 시작
                if (gameObject.activeInHierarchy)
                {
                    resetCoroutine = StartCoroutine(ResetButtonRoutine());
                }
            }
            // 토글 모드에서는 Exit에서 아무것도 안 함 (Enter에서 처리)
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

    // 🌟 추가: 버튼 상태를 강제로 안 눌린 상태로 리셋하는 함수 (상태변환기에서 호출함)
    public void ResetButtonToUnpushedState()
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }

        spriteRenderer.sprite = unpushedSprite;
        transform.localPosition -= pushedOffset;
        isPushed = false; // 토글 상태도 리셋

        if (linkedChest != null) linkedChest.CloseChest();
    }
}