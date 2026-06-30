using UnityEngine;
using FF.Systems;

namespace FF.Player
{
    /// <summary>
    /// 숫자 키(1~9)와 마우스 휠로 인벤토리 칸을 선택한다.
    /// Inventory 자체는 입력을 모르게 하고, 이 스크립트가 입력 → Inventory 호출만 담당한다.
    /// </summary>
    public class InventoryInput : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;

        private static readonly KeyCode[] NumberKeys =
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9
        };

        private void Update()
        {
            if (inventory == null) return;

            for (int i = 0; i < NumberKeys.Length && i < inventory.SlotCount; i++)
            {
                if (Input.GetKeyDown(NumberKeys[i]))
                {
                    inventory.SelectSlot(i);
                }
            }

            float scrollY = Input.mouseScrollDelta.y;
            if (scrollY > 0f)
            {
                inventory.SelectPrevious();
            }
            else if (scrollY < 0f)
            {
                inventory.SelectNext();
            }
        }
    }
}