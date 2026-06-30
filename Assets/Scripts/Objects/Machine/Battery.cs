using UnityEngine;
using FF.Interaction;
using FF.Systems;

namespace FF.Objects.Machine
{
    /// <summary>
    /// 건전지. 기본값은 전력 없음. 성질변환(Q)으로 충전할 수 있고,
    /// 충전된 상태에서만 주울 수 있다(E). 인벤토리에 들어간 건전지는 항상 충전된 상태로 취급한다.
    /// </summary>
    public class Battery : MonoBehaviour, IPropertyChangeable, IInteractable
    {
        [SerializeField] private bool hasPower = false; // 기본값: 전력 없음
        [SerializeField] private ItemData batteryItemData; // 인벤토리에 들어갈 때 쓸 아이템 데이터

        public void ChangeProperty()
        {
            hasPower = !hasPower;
        }

        public string GetPropertyPrompt()
        {
            return hasPower ? "Q 키를 눌러 방전시키기" : "Q 키를 눌러 충전하기";
        }

        public void Interact(GameObject interactor)
        {
            if (!hasPower) return; // 충전되지 않은 건전지는 주울 수 없음

            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory == null) return;

            bool added = inventory.AddItem(batteryItemData, 1);
            if (added)
            {
                gameObject.SetActive(false);
            }
        }

        public string GetInteractionPrompt()
        {
            return hasPower ? "E 키를 눌러 건전지 획득" : "충전된 건전지만 주울 수 있어요 (Q로 충전)";
        }
    }
}