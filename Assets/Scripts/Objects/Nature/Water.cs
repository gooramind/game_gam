using UnityEngine;
using FF.Interaction;
using FF.Systems; // 🌟 인벤토리 시스템 접근을 위해 추가

namespace FF.Objects.Nature
{
    /// <summary>
    /// 물 아이템. 안전한 상태(Q)일 때만 주울 수 있다(E).
    /// </summary>
    public class Water : MonoBehaviour, IPropertyChangeable, IInteractable
    {
        [SerializeField] private bool isSafe = true; // 기본 상태: 안전
        [SerializeField] private ItemData waterItemData; // 🌟 인벤토리에 들어갈 아이템 데이터

        public void ChangeProperty()
        {
            isSafe = !isSafe;
        }

        public string GetPropertyPrompt()
        {
            return isSafe ? "Q 키를 눌러 위험하게 변환" : "Q 키를 눌러 안전하게 변환";
        }

        // 🌟 IInteractable 줍기 기능 구현
        public void Interact(GameObject interactor)
        {
            if (!isSafe) return; // 안전한 물만 주울 수 있음

            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory == null) return;

            bool added = inventory.AddItem(waterItemData, 1);
            if (added)
            {
                gameObject.SetActive(false); // 주우면 맵에서 사라짐
            }
        }

        public string GetInteractionPrompt()
        {
            return isSafe ? "E 키를 눌러 물 획득" : "위험한 물은 주울 수 없어요 (Q로 정화)";
        }

        public bool IsSafe => isSafe;
    }
}