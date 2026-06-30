using UnityEngine;
using FF.Interaction;
using FF.Systems;
using FF.Objects.Nature;

namespace FF.Objects.Machine
{
    /// <summary>
    /// 로봇. 인벤토리에 건전지가 있는 상태로 상호작용(E)하면 건전지 1개를 소모하고 작동을 시작한다.
    /// 작동하면 연결된 철문(Door)을 열고/또는 연결된 큰돌(BigStone)을 옆으로 옮긴다.
    /// 두 필드 다 선택사항이라, 스테이지마다 필요한 것만 연결해서 쓰면 된다.
    /// </summary>
    public class Robot : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData batteryItemData; // 인벤토리에서 소모할 아이템
        [SerializeField] private Door linkedDoor;           // 작동하면 열어줄 철문 (선택)
        [SerializeField] private BigStone linkedBigStone;   // 작동하면 옮길 큰돌 (선택)

        public bool IsPowered { get; private set; } = false;

        public void Interact(GameObject interactor)
        {
            if (IsPowered) return; // 이미 작동 중이면 또 넣을 필요 없음

            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory == null) return;

            if (inventory.HasItem(batteryItemData, 1))
            {
                inventory.RemoveItem(batteryItemData, 1);
                PowerOn();
            }
        }

        private void PowerOn()
        {
            IsPowered = true;

            if (linkedDoor != null)
            {
                linkedDoor.Open();
            }

            if (linkedBigStone != null)
            {
                linkedBigStone.MoveAside();
            }
        }

        public string GetInteractionPrompt()
        {
            return IsPowered ? "로봇이 작동 중입니다" : "E 키를 눌러 건전지 넣기";
        }
    }
}