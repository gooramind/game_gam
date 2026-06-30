using UnityEngine;
using System.Collections;
using TMPro; // 🌟 UI 텍스트(TextMeshPro)를 다루기 위해 필수!

namespace FF.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class StateTransformer : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private int maxUsesPerStage = 3;
        [SerializeField] private float transformationDuration = 10f;
        [SerializeField] private float maxTargetRange = 5f;

        [Header("Stone Settings")]
        [SerializeField] private string stoneTag = "Stone";
        [SerializeField] private float transformedStoneMass = 1f;

        [Header("Button Reference")]
        public ButtonMechanism linkedButton;

        [Header("UI Reference")]
        // 🌟 횟수를 표시할 UI 텍스트 변수 추가
        public TextMeshProUGUI usesTextUI;

        private int currentUses;
        private bool isTransformationActive;
        private Coroutine activeCoroutine;

        void Start()
        {
            currentUses = 0;
            isTransformationActive = false;

            if (linkedButton == null)
            {
                Debug.LogWarning("버튼이 연결되지 않았습니다!");
            }

            // 🌟 게임 시작 시 UI 텍스트 초기화
            UpdateUsesUI();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isTransformationActive)
            {
                AttemptTransformation();
            }
        }

        void AttemptTransformation()
        {
            if (currentUses >= maxUsesPerStage)
            {
                Debug.Log("스테이지 사용 횟수(3회)를 모두 소모했습니다.");
                return;
            }

            GameObject closestStone = FindClosestObjectWithinRange(stoneTag);

            if (closestStone != null)
            {
                ActivateTransformation();
            }
            else
            {
                Debug.Log("주변에 변환할 대상(돌)이 없습니다!");
            }
        }

        void ActivateTransformation()
        {
            currentUses++;
            isTransformationActive = true;

            // 🌟 스킬을 사용했으므로 UI 텍스트 즉시 업데이트
            UpdateUsesUI();

            SetStonesPushable(true);

            if (linkedButton != null)
            {
                linkedButton.isToggleButton = true;
                linkedButton.ResetButtonToUnpushedState();
            }

            if (activeCoroutine != null) StopCoroutine(activeCoroutine);
            activeCoroutine = StartCoroutine(TransformationDurationCoroutine());
        }

        void DeactivateTransformation()
        {
            isTransformationActive = false;

            SetStonesPushable(false);

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
                        stoneRb.bodyType = RigidbodyType2D.Dynamic;
                        stoneRb.mass = transformedStoneMass;
                        stoneRb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                    else
                    {
                        stoneRb.bodyType = RigidbodyType2D.Kinematic;
                        stoneRb.constraints = RigidbodyConstraints2D.None;
                        stoneRb.linearVelocity = Vector2.zero;
                    }
                }
            }
        }

        // 🌟 남은 횟수를 계산하여 UI 텍스트를 바꿔주는 전용 함수
        void UpdateUsesUI()
        {
            if (usesTextUI != null)
            {
                int usesLeft = maxUsesPerStage - currentUses;
                usesTextUI.text = $"Uses: {usesLeft} / {maxUsesPerStage}";
            }
        }
    }
}