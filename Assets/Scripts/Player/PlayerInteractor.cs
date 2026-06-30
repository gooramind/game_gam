using UnityEngine;
using FF.Interaction;

namespace FF.Player
{
    /// <summary>
    /// 플레이어 주변의 상호작용 가능한 오브젝트를 감지하고, 상호작용 키 입력을 처리한다.
    /// 가장 가까운 대상 하나만 골라서 상호작용을 건다.
    /// </summary>
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("상호작용 설정")]
        [SerializeField] private float interactRadius = 1f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private KeyCode interactKey = KeyCode.E;

        private IInteractable currentTarget;

        private void Update()
        {
            FindNearestInteractable();

            if (currentTarget != null && Input.GetKeyDown(interactKey))
            {
                currentTarget.Interact(gameObject);
            }
        }

        // 반경 안에 있는 상호작용 가능 오브젝트 중 가장 가까운 것을 찾는다
        private void FindNearestInteractable()
        {
            currentTarget = null;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactableLayer);

            float closestDistance = float.MaxValue;
            foreach (Collider2D hit in hits)
            {
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable == null) continue;

                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    currentTarget = interactable;
                }
            }
        }

        // 설명창 UI 등 다른 스크립트에서 현재 상호작용 가능 여부를 확인할 때 사용
        public bool HasTarget => currentTarget != null;
        public string CurrentPrompt => currentTarget?.GetInteractionPrompt();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactRadius);
        }
    }
}