using UnityEngine;
using FF.Interaction;
using FF.Systems; // 🌟 인벤토리 시스템 접근을 위해 추가

namespace FF.Objects.Nature
{
    /// <summary>
    /// 씨앗. 인벤토리에 물 아이템이 있는 상태로 상호작용(E)하면 자라난다.
    /// </summary>
    public class Seed : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject treeObject;
        [SerializeField] private ItemData waterItemData; // 🌟 성장에 필요한 물 데이터

        public bool IsGrown { get; private set; } = false;

        // 🌟 IInteractable 물 주기 기능 구현
        public void Interact(GameObject interactor)
        {
            if (IsGrown) return;

            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory == null) return;

            // 인벤토리에 물이 있는지 확인하고 1개 소모
            if (inventory.HasItem(waterItemData, 1))
            {
                inventory.RemoveItem(waterItemData, 1);
                Grow();
            }
        }

        public string GetInteractionPrompt()
        {
            return IsGrown ? "이미 다 자란 나무입니다." : "E 키를 눌러 물 주기";
        }

        public void Grow()
        {
            if (IsGrown) return;
            IsGrown = true;

            if (treeObject != null)
            {
                treeObject.SetActive(true);
            }

            gameObject.SetActive(false); // 씨앗은 나무로 대체됨
        }
    }
}