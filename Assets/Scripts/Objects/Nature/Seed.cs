using UnityEngine;
using FF.Interaction;
using FF.Systems;

namespace FF.Objects.Nature
{
    /// <summary>
    /// 씨앗. 두 가지 방식으로 성장할 수 있다.
    /// 1. 월드 물 오브젝트(Water.cs)가 닿으면 자동으로 Grow() 호출 (기존 방식)
    /// 2. 인벤토리에 물 아이템을 들고 E키로 상호작용 → 물 소모 후 Grow() 호출 (새 방식)
    ///
    /// treeObject가 Inspector에서 연결되지 않은 경우,
    /// Awake에서 자식 오브젝트 중 "Tree"라는 이름을 자동으로 찾아 연결한다.
    /// (Seed 프리팹 안에 Tree를 자식으로 넣으면 자동 연결됨)
    /// </summary>
    public class Seed : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject treeObject;     // 나무 오브젝트 (비워두면 자식에서 자동 탐색)
        [SerializeField] private ItemData waterItemData;    // 인벤토리에서 소모할 물 아이템 데이터

        public bool IsGrown { get; private set; } = false;

        private void Awake()
        {
            // treeObject가 연결되지 않은 경우 자식 중 "Tree" 이름을 가진 오브젝트를 자동으로 찾음
            if (treeObject == null)
            {
                Transform treeTransform = transform.Find("Tree");
                if (treeTransform != null)
                {
                    treeObject = treeTransform.gameObject;
                }
            }
        }

        // ===== 월드 물 오브젝트로 성장 (Water.cs에서 호출) =====
        public void Grow()
        {
            if (IsGrown) return;
            IsGrown = true;

            if (treeObject != null)
            {
                treeObject.SetActive(true);
            }

            // 씨앗 스프라이트와 콜라이더만 끄고 오브젝트는 유지 (자식인 나무가 살아있어야 하므로)
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;

            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }

        // ===== 인벤토리의 물 아이템으로 성장 (E키) =====
        public void Interact(GameObject interactor)
        {
            if (IsGrown) return;

            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory == null) return;

            if (!inventory.HasItem(waterItemData, 1))
            {
                Debug.Log("[Seed] 물이 없습니다. 물을 인벤토리에 들고 오세요.");
                return;
            }

            inventory.RemoveItem(waterItemData, 1);
            Grow();
        }

        public string GetInteractionPrompt()
        {
            if (waterItemData == null) return string.Empty;
            return $"E 키를 눌러 물 주기 ({waterItemData.displayName} 소모)";
        }
    }
}