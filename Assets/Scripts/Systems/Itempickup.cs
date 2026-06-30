using UnityEngine;
using FF.Interaction;

namespace FF.Systems
{
    /// <summary>
    /// 바닥에 떨어진 아이템. 플레이어가 가까이서 E를 누르면 인벤토리에 들어가고 사라진다.
    /// IInteractable을 구현하므로 PlayerInteractor가 자동으로 감지한다.
    /// </summary>
    public class ItemPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData item;
        [SerializeField] private int amount = 1;

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