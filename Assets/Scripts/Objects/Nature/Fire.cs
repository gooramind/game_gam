using UnityEngine;
using UnityEngine.SceneManagement;
using FF.Interaction;
using FF.Systems; // 🌟 인벤토리 시스템 접근을 위해 추가

namespace FF.Objects.Nature
{
    /// <summary>
    /// 불. 위험(데미지 있음) / 안전(데미지 없음) 상태를 가진다.
    /// 인벤토리에 물이 있을 때 E 키를 누르면 물을 1개 소모하고 불을 완전히 없앤다.
    /// 위험한 상태에서 플레이어가 닿으면 즉시 스테이지를 재시작한다.
    /// </summary>
    public class Fire : MonoBehaviour, IPropertyChangeable, IInteractable
    {
        [SerializeField] private bool isDangerous = true;
        [SerializeField] private ItemData waterItemData; // 🌟 불을 끌 때 사용할 물 아이템 데이터

        public void ChangeProperty()
        {
            isDangerous = !isDangerous;
        }

        public string GetPropertyPrompt()
        {
            // UI 폰트 깨짐 현상을 방지하기 위해 텍스트는 영문으로 출력합니다.
            return isDangerous ? "Press Q to Extinguish" : "Press Q to Ignite";
        }

        // 🌟 IInteractable: 물을 사용해 불을 완전히 없애는 기능
        public void Interact(GameObject interactor)
        {
            if (!isDangerous) return;

            Inventory inventory = interactor.GetComponent<Inventory>();
            if (inventory == null) return;

            // 인벤토리에 물이 있는지 확인하고 1개 소모
            if (inventory.HasItem(waterItemData, 1))
            {
                inventory.RemoveItem(waterItemData, 1);
                gameObject.SetActive(false); // 불 오브젝트를 맵에서 완전히 비활성화(제거)
            }
        }

        public string GetInteractionPrompt()
        {
            return isDangerous ? "Press E to use Water" : "";
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isDangerous) return;

            if (other.CompareTag("Player"))
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }

        public bool IsDangerous => isDangerous;
    }
}