using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FF.Systems;

namespace FF.UI
{
    /// <summary>
    /// Inventory의 내용을 화면에 칸 형태로 보여준다.
    /// slotPrefab을 인벤토리 칸 수만큼 복제해서 slotContainer 아래에 배치하고,
    /// Inventory가 바뀔 때마다(아이템 변화, 선택 변화) 아이콘/개수/선택 표시를 갱신한다.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("연결")]
        [SerializeField] private Inventory inventory;
        [SerializeField] private GameObject slotPrefab;   // Image(배경) + 자식 "Icon"(Image) + 자식 "CountText"(TextMeshProUGUI)
        [SerializeField] private Transform slotContainer; // Horizontal Layout Group 등이 붙은 부모

        [Header("선택 표시 색상")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color selectedColor = new Color(1f, 0.85f, 0.3f);

        private class SlotUIRefs
        {
            public Image background;
            public Image icon;
            public TextMeshProUGUI countText;
        }

        private readonly List<SlotUIRefs> slotUIList = new List<SlotUIRefs>();

        private void Start()
        {
            BuildSlotUI();

            if (inventory != null)
            {
                inventory.OnInventoryChanged += RefreshUI;
                inventory.OnSelectionChanged += RefreshUI;
            }

            RefreshUI();
        }

        private void OnDestroy()
        {
            if (inventory != null)
            {
                inventory.OnInventoryChanged -= RefreshUI;
                inventory.OnSelectionChanged -= RefreshUI;
            }
        }

        // 인벤토리 칸 수만큼 slotPrefab을 복제해서 UI 칸을 미리 만들어둔다
        private void BuildSlotUI()
        {
            if (inventory == null || slotPrefab == null || slotContainer == null) return;

            for (int i = 0; i < inventory.SlotCount; i++)
            {
                GameObject slotObj = Instantiate(slotPrefab, slotContainer);

                SlotUIRefs refs = new SlotUIRefs
                {
                    background = slotObj.GetComponent<Image>(),
                    icon = slotObj.transform.Find("Icon")?.GetComponent<Image>(),
                    countText = slotObj.transform.Find("CountText")?.GetComponent<TextMeshProUGUI>()
                };

                slotUIList.Add(refs);
            }
        }

        // 인벤토리 데이터를 읽어서 화면 표시를 갱신한다
        private void RefreshUI()
        {
            if (inventory == null) return;

            IReadOnlyList<InventorySlot> slots = inventory.Slots;

            for (int i = 0; i < slotUIList.Count && i < slots.Count; i++)
            {
                SlotUIRefs ui = slotUIList[i];
                InventorySlot slot = slots[i];

                if (ui.icon != null)
                {
                    ui.icon.enabled = !slot.IsEmpty;
                    ui.icon.sprite = slot.IsEmpty ? null : slot.item.icon;
                }

                if (ui.countText != null)
                {
                    ui.countText.text = (!slot.IsEmpty && slot.count > 1) ? slot.count.ToString() : string.Empty;
                }

                if (ui.background != null)
                {
                    ui.background.color = (i == inventory.SelectedIndex) ? selectedColor : normalColor;
                }
            }
        }
    }
}