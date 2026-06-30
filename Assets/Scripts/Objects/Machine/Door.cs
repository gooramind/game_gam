using UnityEngine;

namespace FF.Objects.Machine
{
    /// <summary>
    /// 철문. Open()이 호출되면 더 이상 길을 막지 않는다. (로봇이 작동하면 호출함)
    /// </summary>
    public class Door : MonoBehaviour
    {
        [SerializeField] private Collider2D doorCollider;
        [SerializeField] private SpriteRenderer doorSprite; // 비워둬도 동작은 함 (나중에 애니메이션으로 교체 가능)

        public bool IsOpen { get; private set; } = false;

        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;

            if (doorCollider != null)
            {
                doorCollider.enabled = false; // 더 이상 막지 않음
            }

            if (doorSprite != null)
            {
                doorSprite.enabled = false; // 일단 사라지는 것으로 표현
            }
        }
    }
}