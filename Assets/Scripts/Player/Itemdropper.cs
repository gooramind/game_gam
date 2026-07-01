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
        [SerializeField] private GameObject itemPickupPrefab;
        [SerializeField] private KeyCode dropKey = KeyCode.G;
        [SerializeField] private Vector3 dropItemScale = new Vector3(2f, 2f, 1f); // 드랍된 아이템 크기 (인스펙터에서 조절)

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

            Vector3 dropPosition = transform.position + new Vector3(0f, 1f, 0f);
            GameObject pickupObj = Instantiate(itemPickupPrefab, dropPosition, Quaternion.identity);

            // 크기 설정
            pickupObj.transform.localScale = dropItemScale;

            // 중력으로 떨어지게
            Rigidbody2D rb = pickupObj.GetComponent<Rigidbody2D>();
            if (rb == null) rb = pickupObj.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
            rb.freezeRotation = true; // 아이템이 회전하지 않게

            // 바닥과 부딪히는 물리 콜라이더 별도 추가
            // (기존 Is Trigger 콜라이더는 줍기 감지용으로 그대로 두고, 이건 물리 충돌 전용)
            BoxCollider2D physicsCol = pickupObj.AddComponent<BoxCollider2D>();
            physicsCol.isTrigger = false;
            physicsCol.size = new Vector2(0.5f, 0.5f);

            ItemPickup pickup = pickupObj.GetComponent<ItemPickup>();
            if (pickup != null)
            {
                pickup.Initialize(droppedItem, 1);
            }
        }
    }
}