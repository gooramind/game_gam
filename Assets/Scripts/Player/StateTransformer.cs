using UnityEngine;
using TMPro;
using FF.Interaction;

namespace FF.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class StateTransformer : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private int maxUsesPerStage = 1;   // 🌟 3 → 1 로 변경
        [SerializeField] private float maxTargetRange = 5f;

        [Header("대상 설정")]
        [SerializeField] private string targetTag = "Object";

        [Header("UI Reference")]
        public TextMeshProUGUI usesTextUI;

        private int currentUses;

        void Start()
        {
            currentUses = 0;
            UpdateUsesUI();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                AttemptTransformation();
            }
        }

        void AttemptTransformation()
        {
            if (currentUses >= maxUsesPerStage)
            {
                Debug.Log("스테이지 사용 횟수를 모두 소모했습니다.");
                return;
            }

            ITransformable target = FindClosestTransformable();

            if (target != null && !target.IsTransformed)
            {
                target.Transform();   // 실제 효과는 각 오브젝트가 담당
                currentUses++;
                UpdateUsesUI();
            }
            else
            {
                Debug.Log("주변에 변환할 대상이 없습니다!");
            }
        }

        ITransformable FindClosestTransformable()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
            ITransformable closest = null;
            float minDistance = Mathf.Infinity;
            Vector2 currentPosition = transform.position;

            foreach (GameObject obj in targets)
            {
                ITransformable t = obj.GetComponent<ITransformable>();
                if (t == null) continue;

                float distance = Vector2.Distance(currentPosition, obj.transform.position);
                if (distance < minDistance && distance <= maxTargetRange)
                {
                    minDistance = distance;
                    closest = t;
                }
            }
            return closest;
        }

        // 🌟 남은 횟수만 간단히 표시: Use 1 → Use 0
        void UpdateUsesUI()
        {
            if (usesTextUI != null)
            {
                int usesLeft = maxUsesPerStage - currentUses;
                usesTextUI.text = $"Use {usesLeft}";
            }
        }
    }
}