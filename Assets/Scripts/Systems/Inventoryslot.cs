namespace FF.Systems
{
    /// <summary>
    /// 인벤토리 한 칸의 상태. 어떤 아이템이 몇 개 들어있는지 저장한다.
    /// </summary>
    [System.Serializable]
    public class InventorySlot
    {
        public ItemData item;
        public int count;

        public bool IsEmpty => item == null || count <= 0;

        public void Clear()
        {
            item = null;
            count = 0;
        }
    }
}