using UnityEngine;

public class ChestMechanism : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // 시작할 때 강제로 닫힌 상태를 재생하거나 아무것도 안 함
        animator.Play("Idle");
    }

    public void OpenChest()
    {
        Debug.Log("상자 열기 신호 받음!"); // 이 메시지가 콘솔창에 뜨는지 확인하세요!
        animator.SetTrigger("Open");
    }

    public void CloseChest()
    {
        // 다시 닫히는 기능을 만들려면 Animator에서 역재생하거나 
        // 닫힌 상태로 돌아가는 애니메이션(Idle)을 연결해야 합니다.
        animator.Play("Idle");
    }
}