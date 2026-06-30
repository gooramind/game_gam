using System;
using System.Collections.Generic;
using UnityEngine;

namespace FF.Systems
{
    /// <summary>
    /// 플레이어의 인벤토리. 고정된 칸 수만큼 아이템을 담을 수 있다.
    /// 아이템 획득/사용/드랍은 전부 이 컴포넌트를 통해 처리한다.
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        [Header("인벤토리 설정")]
        [SerializeField] private int slotCount = 8;

        private List<InventorySlot> slots;

        // 인벤토리 내용이 바뀔 때마다 UI 등에서 구독해서 갱신할 수 있는 이벤트
        public event Action OnInventoryChanged;

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

            // 1. 이미 같은 아이템이 있고 스택 여유가 있는 칸부터 채운다
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

            // 2. 남은 수량은 빈 칸에 새로 채운다
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

        public IReadOnlyList<InventorySlot> Slots => slots;
    }
}