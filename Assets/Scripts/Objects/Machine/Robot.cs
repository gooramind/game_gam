using UnityEngine;
using FF.Interaction;
using FF.Systems;
using FF.Objects.Nature;

namespace FF.Objects.Machine
{
    /// <summary>
    /// 로봇. 건전지를 넣으면(E키) 목표 지점까지 직접 걸어가서 동작을 수행한다.
    /// - linkedBigStone이 연결되어 있으면: 큰 돌 쪽으로 걸어가서 밀기
    /// - linkedDoor가 연결되어 있으면: 철문 쪽으로 걸어가서 열기
    /// - 둘 다 연결되어 있으면: 큰 돌 먼저 처리
    /// </summary>
    public class Robot : MonoBehaviour, IInteractable
    {
        [Header("연결")]
        [SerializeField] private ItemData batteryItemData;
        [SerializeField] private Door linkedDoor;
        [SerializeField] private BigStone linkedBigStone;

        [Header("이동 설정")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float actionDistance = 0.8f; // 이 거리 안에 들어오면 동작 수행

        private bool isPowered = false;
        private bool isMoving = false;
        private bool hasActed = false;
        private Transform actionTarget;
        private Animator anim;

        public bool IsPowered => isPowered;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!isMoving || hasActed || actionTarget == null) return;

            float distX = Mathf.Abs(actionTarget.position.x - transform.position.x);

            // 목표에 충분히 가까워지면 동작 수행
            if (distX <= actionDistance)
            {
                PerformAction();
                return;
            }

            // 목표 방향으로 X축 이동
            float dirX = actionTarget.position.x > transform.position.x ? 1f : -1f;
            transform.position += new Vector3(dirX * moveSpeed * Time.deltaTime, 0f, 0f);

            // 이동 방향에 따라 스프라이트 뒤집기
            Vector3 scale = transform.localScale;
            scale.x = dirX > 0f ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;

            // 걷기 애니메이션 (Animator가 없으면 무시됨)
            if (anim != null)
            {
                anim.SetBool("isWalking", true);
            }
        }

        private void PerformAction()
        {
            hasActed = true;
            isMoving = false;

            if (anim != null)
            {
                anim.SetBool("isWalking", false);
                anim.SetTrigger("doAction"); // 동작 애니메이션 (없으면 무시됨)
            }

            if (linkedBigStone != null)
            {
                linkedBigStone.MoveAside();
            }

            if (linkedDoor != null)
            {
                linkedDoor.Open();
            }
        }

        private void PowerOn()
        {
            isPowered = true;

            // 목표 대상 결정 (큰돌 우선, 없으면 철문)
            if (linkedBigStone != null)
            {
                actionTarget = linkedBigStone.transform;
            }
            else if (linkedDoor != null)
            {
                actionTarget = linkedDoor.transform;
            }

            if (actionTarget != null)
            {
                isMoving = true;
            }
        }

        public void Interact(GameObject interactor)
        {
            if (isPowered) return;

            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory == null) return;

            if (inventory.HasItem(batteryItemData, 1))
            {
                inventory.RemoveItem(batteryItemData, 1);
                PowerOn();
            }
        }

        public string GetInteractionPrompt()
        {
            return isPowered ? "로봇이 작동 중입니다" : "E 키를 눌러 건전지 넣기";
        }
    }
}