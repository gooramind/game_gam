using UnityEngine;
using FF.Interaction;

namespace FF.Systems
{
    /// <summary>
    /// 바닥에 떨어진 아이템. 플레이어가 가까이서 E를 누르면 인벤토리에 들어가고 사라진다.
    /// IInteractable을 구현하므로 PlayerInteractor가 자동으로 감지한다.
    /// 인스펙터에서 미리 Item을 지정해도 되고(맵에 미리 배치), Initialize()로 코드에서 동적으로
    /// 만들 수도 있다(플레이어가 드랍할 때 ItemDropper가 이 방식을 사용한다).
    /// </summary>
    public class ItemPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData item;
        [SerializeField] private int amount = 1;

        // 코드에서 아이템 종류를 동적으로 지정할 때 사용 (드랍된 아이템 생성 등)
        public void Initialize(ItemData newItem, int newAmount)
        {
            item = newItem;
            amount = newAmount;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && item != null)
            {
                spriteRenderer.sprite = item.icon;
            }
        }

        public void Interact(GameObject interactor)
        {
            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory == null)
            {
                Debug.LogWarning("[ItemPickup] interactor에 Inventory 컴포넌트가 없습니다.");
                return;
            }

            bool added = inventory.AddItem(item, amount);
            if (added)
            {
                gameObject.SetActive(false); // 일단 비활성화. 풀링이 필요해지면 Destroy 대신 재사용 가능
            }
        }

        public string GetInteractionPrompt()
        {
            return item != null ? $"E 키를 눌러 {item.displayName} 획득" : "E 키를 눌러 획득";
        }
    }
}