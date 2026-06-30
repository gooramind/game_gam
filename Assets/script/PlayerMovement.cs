using UnityEngine;

public class TempPlayerMove : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    private Rigidbody2D rb;
    private float moveInput;

    void Start()
    {
        // 플레이어에 붙어있는 물리 엔진(Rigidbody2D)을 가져옵니다.
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 왼쪽/오른쪽 방향키 (또는 A, D키) 입력을 받습니다.
        // 왼쪽이면 -1, 가만히 있으면 0, 오른쪽이면 1이 들어옵니다.
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        // 위아래(Y축)로 떨어지는 속도는 그대로 두고, 양옆(X축) 속도만 입력값에 맞춰 바꿉니다.
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }
}