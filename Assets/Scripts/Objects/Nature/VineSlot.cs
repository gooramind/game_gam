using UnityEngine;
using FF.Interaction;
using FF.Systems;

namespace FF.Objects.Nature
{
    /// <summary>
    /// 덩쿨을 설치할 수 있는 자리(벽 등). 인벤토리에 덩쿨 아이템이 있는 상태로 상호작용(E)하면
    /// 덩쿨 아이템을 1개 소모하고, 같은 위치에 미리 비활성화로 놓아둔 덩쿨 오브젝트(Climbable 포함)를 활성화한다.
    /// 씨앗-나무와 같은 "겹쳐놓고 켜고 끄기" 방식을 사용한다.
    /// </summary>
    public class VineSlot : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData vineItemData;
        [SerializeField] private GameObject vineObject; // 같은 위치에 미리 놓아둔, 비활성화된 덩쿨 오브젝트 (Climbable 포함)

        private bool isInstalled = false;

        public void Interact(GameObject interactor)
        {
            if (isInstalled) return;

            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory == null) return;

            if (inventory.HasItem(vineItemData, 1))
            {
                inventory.RemoveItem(vineItemData, 1);
                Install();
            }
        }

        private void Install()
        {
            isInstalled = true;

            if (vineObject != null)
            {
                vineObject.SetActive(true);
            }

            gameObject.SetActive(false); // 빈 슬롯 표시는 사라지고 실제 덩쿨로 대체된다
        }

        public string GetInteractionPrompt()
        {
            return "E 키를 눌러 덩쿨 설치";
        }
    }
}