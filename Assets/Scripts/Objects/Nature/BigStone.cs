using UnityEngine;

namespace FF.Objects.Nature
{
    /// <summary>
    /// 큰돌. 플레이어는 절대 밀 수 없고, 오직 로봇이 작동했을 때만 옆으로 비켜난다.
    /// 비켜나면서 그 뒤/밑에 숨겨둔 다른 오브젝트(덩쿨 등)로 가는 길이 자연스럽게 열린다.
    /// </summary>
    public class BigStone : MonoBehaviour
    {
        [SerializeField] private Vector2 moveOffset = new Vector2(3f, 0f); // 비켜날 방향과 거리
        [SerializeField] private float moveSpeed = 2f;

        private Vector3 targetPosition;
        private bool isMoving = false;
        private bool hasMoved = false;

        private void Awake()
        {
            targetPosition = transform.position;
        }

        public void MoveAside()
        {
            if (hasMoved) return;
            hasMoved = true;

            targetPosition = transform.position + (Vector3)moveOffset;
            isMoving = true;
        }

        private void Update()
        {
            if (!isMoving) return;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }
}