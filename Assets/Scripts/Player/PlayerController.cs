using UnityEngine;

namespace FF.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("이동 설정")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("점프 설정")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.15f;
        [SerializeField] private LayerMask groundLayer;

        [Header("충돌 다듬기")]
        [SerializeField] private float maxFallSpeed = 18f;

        private Rigidbody2D rb;
        private Animator anim; // 🌟 추가: 애니메이터 컴포넌트 변수
        private float moveInput;
        private bool isGrounded;
        private bool facingRight = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>(); // 🌟 추가: 애니메이터 가져오기
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

            // 🌟 추가: 애니메이터에 현재 상태 전달
            if (anim != null)
            {
                anim.SetFloat("Speed", Mathf.Abs(moveInput)); // 이동 속도 (절댓값으로 0 또는 1)
                anim.SetBool("isGrounded", isGrounded);       // 바닥 여부
                anim.SetFloat("vSpeed", rb.linearVelocity.y); // 수직 속도 (점프/낙하)
            }
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