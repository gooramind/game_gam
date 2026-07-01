using UnityEngine;
using FF.Interaction;
using FF.Systems;

namespace FF.Objects.Nature
{
    /// <summary>
    /// 씨앗을 심을 수 있는 자리. 인벤토리에 씨앗 아이템이 있는 상태로 상호작용(E)하면
    /// 씨앗 아이템을 1개 소모하고, SeedSlot의 위치에 씨앗 프리팹을 생성한다.
    ///
    /// 이전 방식(비활성화된 오브젝트를 켜는)과 달리, Instantiate로 프리팹을 생성하기 때문에
    /// SeedSlot 위치에 항상 정확하게 씨앗이 심어진다.
    ///
    /// 씨앗 프리팹 구조:
    ///   Seed (Seed.cs + Sprite Renderer + Collider2D + Layer: Interactable)
    ///    └─ Tree (Sprite Renderer + Climbable + Collider2D Is Trigger, 처음엔 비활성화)
    /// </summary>
    public class SeedSlot : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData seedItemData;
        [SerializeField] private GameObject seedPrefab; // 심으면 이 자리에 생성될 씨앗 프리팹

        private bool isPlanted = false;

        public void Interact(GameObject interactor)
        {
            if (isPlanted) return;

            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory == null) return;

            if (!inventory.HasItem(seedItemData, 1))
            {
                Debug.Log("[SeedSlot] 씨앗이 없습니다.");
                return;
            }

            inventory.RemoveItem(seedItemData, 1);
            Plant();
        }

        private void Plant()
        {
            isPlanted = true;

            if (seedPrefab != null)
            {
                // SeedSlot 위치에 씨앗 프리팹을 생성한다
                Instantiate(seedPrefab, transform.position, Quaternion.identity);
            }

            gameObject.SetActive(false); // 빈 자리 표시는 사라진다
        }

        public string GetInteractionPrompt()
        {
            return "E 키를 눌러 씨앗 심기";
        }
    }
}