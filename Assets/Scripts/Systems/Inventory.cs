using System;
using System.Collections.Generic;
using UnityEngine;

namespace FF.Systems
{
    /// <summary>
    /// 플레이어의 인벤토리. 고정된 칸 수만큼 아이템을 담을 수 있다.
    /// 아이템 획득/제거/선택(들기)/드랍 데이터 처리를 전부 이 컴포넌트가 담당한다.
    /// 입력(키보드)이나 화면 표시(UI)에는 직접 관여하지 않는다 — 그건 InventoryInput, InventoryUI가 맡는다.
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        [Header("인벤토리 설정")]
        [SerializeField] private int slotCount = 8;

        private List<InventorySlot> slots;
        private int selectedIndex = 0;

        // 칸 내용(개수, 아이템 종류)이 바뀔 때 호출
        public event Action OnInventoryChanged;
        // 선택된 칸(들고 있는 아이템)이 바뀔 때 호출
        public event Action OnSelectionChanged;

        private void Awake()
        {
            slots = new List<InventorySlot>(slotCount);
            for (int i = 0; i < slotCount; i++)
            {
                slots.Add(new InventorySlot());
            }
        }

        /// <summary>
        /// 아이템을 인벤토리에 추가한다. 같은 아이템이 이미 있고 스택 여유가 있으면 그 칸에 더하고,
        /// 그래도 남으면 빈 칸에 새로 채운다. 자리가 모자라 일부만 들어갔거나 전혀 못 들어가면 false.
        /// </summary>
        public bool AddItem(ItemData item, int count = 1)
        {
            if (item == null || count <= 0) return false;

            foreach (InventorySlot slot in slots)
            {
                if (!slot.IsEmpty && slot.item == item && slot.count < item.maxStack)
                {
                    int spaceLeft = item.maxStack - slot.count;
                    int amountToAdd = Mathf.Min(spaceLeft, count);
                    slot.count += amountToAdd;
                    count -= amountToAdd;

                    if (count <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        Debug.Log($"[Inventory] {item.displayName} 획득. 현재 보유: {GetItemCount(item)}개");
                        return true;
                    }
                }
            }

            foreach (InventorySlot slot in slots)
            {
                if (slot.IsEmpty)
                {
                    int amountToAdd = Mathf.Min(item.maxStack, count);
                    slot.item = item;
                    slot.count = amountToAdd;
                    count -= amountToAdd;

                    if (count <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        Debug.Log($"[Inventory] {item.displayName} 획득. 현재 보유: {GetItemCount(item)}개");
                        return true;
                    }
                }
            }

            OnInventoryChanged?.Invoke();
            return false; // 인벤토리에 자리가 모자람
        }

        /// <summary>
        /// 아이템을 인벤토리에서 제거한다. 보유 수량이 모자라면 아무것도 빼지 않고 false를 반환한다.
        /// </summary>
        public bool RemoveItem(ItemData item, int count = 1)
        {
            if (item == null || count <= 0) return false;
            if (GetItemCount(item) < count) return false;

            int remaining = count;
            foreach (InventorySlot slot in slots)
            {
                if (slot.IsEmpty || slot.item != item) continue;

                int amountToRemove = Mathf.Min(slot.count, remaining);
                slot.count -= amountToRemove;
                remaining -= amountToRemove;

                if (slot.count <= 0)
                {
                    slot.Clear();
                }

                if (remaining <= 0) break;
            }

            OnInventoryChanged?.Invoke();
            return true;
        }

        public int GetItemCount(ItemData item)
        {
            int total = 0;
            foreach (InventorySlot slot in slots)
            {
                if (!slot.IsEmpty && slot.item == item)
                {
                    total += slot.count;
                }
            }
            return total;
        }

        public bool HasItem(ItemData item, int count = 1)
        {
            return GetItemCount(item) >= count;
        }

        // ===== 선택(들고 있는 아이템) =====
        // 성질변환 우선순위 1순위("들고 있는거")에서 사용할 "현재 선택된 아이템" 개념.

        public int SelectedIndex => selectedIndex;
        public InventorySlot SelectedSlot => (selectedIndex >= 0 && selectedIndex < slots.Count) ? slots[selectedIndex] : null;
        public ItemData SelectedItem => (SelectedSlot != null && !SelectedSlot.IsEmpty) ? SelectedSlot.item : null;

        public void SelectSlot(int index)
        {
            if (index < 0 || index >= slots.Count) return;
            if (index == selectedIndex) return;

            selectedIndex = index;
            OnSelectionChanged?.Invoke();
        }

        public void SelectNext()
        {
            SelectSlot((selectedIndex + 1) % slots.Count);
        }

        public void SelectPrevious()
        {
            SelectSlot((selectedIndex - 1 + slots.Count) % slots.Count);
        }

        // ===== 드랍 =====

        /// <summary>
        /// 현재 선택된 칸에서 아이템 1개를 인벤토리 데이터에서 빼낸다.
        /// 실제로 바닥에 오브젝트를 생성하는 건 ItemDropper가 처리하고, 여기서는 데이터만 책임진다.
        /// </summary>
        public bool TryDropSelected(out ItemData droppedItem)
        {
            droppedItem = SelectedItem;
            if (droppedItem == null) return false;

            return RemoveItem(droppedItem, 1);
        }

        public int SlotCount => slotCount;
        public IReadOnlyList<InventorySlot> Slots => slots;
    }
}