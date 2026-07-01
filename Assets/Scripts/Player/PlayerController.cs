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
        [SerializeField] private float jumpCooldown = 0.2f;    // 점프 후 이 시간 동안은 다시 점프 불가 (어떤 원인이든 연속 점프 방지용 안전장치)

        private Rigidbody2D rb;
        private Animator anim; // 애니메이터 컴포넌트
        private float moveInput;
        private bool isGrounded;
        private bool facingRight = true;
        private float lastJumpTime = -999f;
        public bool IsGrounded => isGrounded;
        public bool IsMoving => Mathf.Abs(moveInput) > 0.01f;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            // 좌우 입력 받기: Input Manager의 축 이름에 의존하지 않고 키를 직접 확인
            moveInput = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                moveInput = -1f;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                moveInput = 1f;
            }

            // 바닥에 닿아있는지 체크
            // 콜라이더가 겹쳐있어도 위로 솟아오르는 중(점프 직후)이면 바닥으로 인정하지 않는다.
            // 이게 없으면, 점프한 직후 물리적으로 아직 자리를 못 떠난 한두 프레임 사이에
            // 점프를 또 눌렀을 때 2단 점프처럼 인정되는 버그가 생길 수 있다.
            bool touchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            isGrounded = touchingGround && rb.linearVelocity.y <= 0.05f;

            // 점프 입력 처리 (Space, W, 위쪽 화살표 중 하나 + 바닥에 있을 때만 + 쿨다운 끝났을 때만)
            bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
            bool cooldownReady = Time.time >= lastJumpTime + jumpCooldown;
            if (jumpPressed && isGrounded && cooldownReady)
            {
                Jump();
                lastJumpTime = Time.time;
            }

            HandleFlip();

            // 애니메이터에 현재 상태 전달
            if (anim != null)
            {
                anim.SetFloat("Speed", Mathf.Abs(moveInput)); // 이동 속도 (절댓값으로 0 또는 1)
                anim.SetBool("isGrounded", isGrounded);       // 바닥 여부
                anim.SetFloat("vSpeed", rb.linearVelocity.y); // 수직 속도 (점프/낙하)
            }
        }

        private void FixedUpdate()
        {
            // 실제 이동은 물리 연산 주기인 FixedUpdate에서 처리
            float verticalVelocity = rb.linearVelocity.y;

            // 낙하 속도가 maxFallSpeed를 넘지 않도록 제한
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

        // 이동 방향에 따라 스프라이트를 좌우로 뒤집는다
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

        // 에디터 화면에서 바닥 체크 범위를 눈으로 확인할 수 있게 표시
        private void OnDrawGizmosSelected()
        {
            if (groundCheck == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}