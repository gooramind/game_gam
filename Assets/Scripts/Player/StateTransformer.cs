using UnityEngine;
using System.Collections;

namespace FF.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class StateTransformer : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private int maxUsesPerStage = 3;      // 스테이지당 최대 사용 횟수
        [SerializeField] private float transformationDuration = 10f; // 변환 지속 시간
        [SerializeField] private float maxTargetRange = 5f;       // 타겟(돌) 감지 최대 거리

        [Header("Stone Settings")]
        [SerializeField] private string stoneTag = "Stone";       // 바위 태그
        [SerializeField] private float transformedStoneMass = 1f; // 🌟 무게를 더 가볍게 조정 (잘 밀리도록)

        [Header("Button Reference")]
        public ButtonMechanism linkedButton;

        private int currentUses;
        private bool isTransformationActive;
        private Coroutine activeCoroutine;

        void Start()
        {
            currentUses = 0;
            isTransformationActive = false;

            if (linkedButton == null)
            {
                Debug.LogWarning("버튼이 연결되지 않았습니다! Inspector 창에서 Linked Button을 넣어주세요.");
            }
        }

        void Update()
        {
            // Q 키 입력 처리 (변환 중이 아닐 때만)
            if (Input.GetKeyDown(KeyCode.Q) && !isTransformationActive)
            {
                AttemptTransformation();
            }
        }

        void AttemptTransformation()
        {
            // 사용 횟수 제한 체크
            if (currentUses >= maxUsesPerStage)
            {
                Debug.Log("스테이지 사용 횟수(3회)를 모두 소모했습니다.");
                return;
            }

            // 🌟 적 대신 '가장 가까운 돌'을 찾습니다.
            GameObject closestStone = FindClosestObjectWithinRange(stoneTag);

            if (closestStone != null)
            {
                // 근처에 돌이 있다면 변환 시작!
                ActivateTransformation();
            }
            else
            {
                Debug.Log("주변에 변환할 대상(돌)이 없습니다! (거리 " + maxTargetRange + " 이내)");
            }
        }

        void ActivateTransformation()
        {
            currentUses++;
            isTransformationActive = true;
            Debug.Log($"상태 변환 시작! 남은 횟수: {maxUsesPerStage - currentUses}");

            // 1. 돌(Stone)을 밀 수 있게 변경
            SetStonesPushable(true);

            // 2. 버튼을 토글 모드로 변경
            if (linkedButton != null)
            {
                linkedButton.isToggleButton = true;
                linkedButton.ResetButtonToUnpushedState();
            }

            // 지속 시간 체크 코루틴 시작
            if (activeCoroutine != null) StopCoroutine(activeCoroutine);
            activeCoroutine = StartCoroutine(TransformationDurationCoroutine());
        }

        void DeactivateTransformation()
        {
            isTransformationActive = false;
            Debug.Log("상태 변환 종료! 원래대로 돌아갑니다.");

            // 1. 돌을 다시 못 밀게 변경
            SetStonesPushable(false);

            // 2. 버튼을 다시 홀드 모드로 변경
            if (linkedButton != null)
            {
                linkedButton.isToggleButton = false;
                linkedButton.ResetButtonToUnpushedState();
            }

            activeCoroutine = null;
        }

        IEnumerator TransformationDurationCoroutine()
        {
            yield return new WaitForSeconds(transformationDuration);
            DeactivateTransformation();
        }

        // 특정 태그를 가진 가장 가까운 오브젝트 찾기
        GameObject FindClosestObjectWithinRange(string targetTag)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
            GameObject closest = null;
            float minDistance = Mathf.Infinity;
            Vector2 currentPosition = transform.position;

            foreach (GameObject target in targets)
            {
                float distance = Vector2.Distance(currentPosition, target.transform.position);

                if (distance < minDistance && distance <= maxTargetRange)
                {
                    minDistance = distance;
                    closest = target;
                }
            }
            return closest;
        }

        void SetStonesPushable(bool isPushable)
        {
            GameObject[] stones = GameObject.FindGameObjectsWithTag(stoneTag);
            foreach (GameObject stone in stones)
            {
                Rigidbody2D stoneRb = stone.GetComponent<Rigidbody2D>();
                if (stoneRb != null)
                {
                    if (isPushable)
                    {
                        // 🌟 Dynamic으로 바꾸고 회전을 막아 안정적으로 밀리게 설정
                        stoneRb.bodyType = RigidbodyType2D.Dynamic;
                        stoneRb.mass = transformedStoneMass;
                        stoneRb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                    else
                    {
                        // 🌟 다시 Kinematic으로 변경하여 고정
                        stoneRb.bodyType = RigidbodyType2D.Kinematic;
                        stoneRb.constraints = RigidbodyConstraints2D.None;
                        stoneRb.linearVelocity = Vector2.zero; // Unity 6 최신 문법 (이전 버전이면 velocity로 수정)
                    }
                }
            }
        }
    }
}