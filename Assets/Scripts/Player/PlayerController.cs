using UnityEngine;

namespace FF.Player
{
    /// <summary>
    /// 플레이어의 좌우 이동, 점프, 바닥 판정을 담당하는 기본 컨트롤러.
    /// Rigidbody2D의 물리 연산을 이용해 이동을 처리한다.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("이동 설정")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("점프 설정")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private Transform groundCheck;       // 발밑에 둘 빈 오브젝트
        [SerializeField] private float groundCheckRadius = 0.15f;
        [SerializeField] private LayerMask groundLayer;        // 바닥으로 인식할 레이어

        [Header("충돌 다듬기")]
        [SerializeField] private float maxFallSpeed = 18f;     // 낙하 속도 제한 (너무 빠르면 얇은 발판을 뚫고 지나갈 수 있음)

        private Rigidbody2D rb;
        private float moveInput;
        private bool isGrounded;
        private bool facingRight = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            moveInput = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                moveInput = -1f;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                moveInput = 1f;
            }

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
            if (jumpPressed && isGrounded)
            {
                Jump();
            }

            HandleFlip();
        }

        private void FixedUpdate()
        {
            float verticalVelocity = rb.linearVelocity.y;

            if (verticalVelocity < -maxFallSpeed)
            {
                verticalVelocity = -maxFallSpeed;
            }

            rb.linearVelocity = new Vector2(moveInput * moveSpeed, verticalVelocity);
        }

        private void Jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        private void HandleFlip()
        {
            if (moveInput > 0f && !facingRight)
            {
                Flip();
            }
            else if (moveInput < 0f && facingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheck == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}