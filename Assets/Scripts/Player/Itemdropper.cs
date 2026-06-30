using UnityEngine;
using FF.Systems;

namespace FF.Player
{
    /// <summary>
    /// 현재 선택된 아이템을 바닥에 내려놓는다(드랍). G 키를 누르면 동작한다.
    /// </summary>
    public class ItemDropper : MonoBehaviour
    {
        [Header("드랍 설정")]
        [SerializeField] private Inventory inventory;
        [SerializeField] private GameObject itemPickupPrefab; // Sprite Renderer + Collider2D + ItemPickup이 붙은 프리팹
        [SerializeField] private KeyCode dropKey = KeyCode.G;
        [SerializeField] private float dropDistance = 1f; // 플레이어 앞쪽으로 얼마나 떨어뜨릴지

        private void Update()
        {
            if (Input.GetKeyDown(dropKey))
            {
                TryDrop();
            }
        }

        private void TryDrop()
        {
            if (inventory == null || itemPickupPrefab == null) return;
            if (!inventory.TryDropSelected(out ItemData droppedItem)) return;

            // 캐릭터가 보는 방향(스프라이트 좌우 반전 기준)으로 한 칸 앞에 내려놓는다
            float direction = transform.localScale.x >= 0f ? 1f : -1f;
            Vector3 dropPosition = transform.position + new Vector3(direction * dropDistance, 0f, 0f);

            GameObject pickupObj = Instantiate(itemPickupPrefab, dropPosition, Quaternion.identity);
            ItemPickup pickup = pickupObj.GetComponent<ItemPickup>();
            if (pickup != null)
            {
                pickup.Initialize(droppedItem, 1);
            }
        }
    }
}