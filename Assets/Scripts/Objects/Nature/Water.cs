using UnityEngine;
using FF.Interaction;

namespace FF.Objects.Nature
{
    /// <summary>
    /// 물. 두 가지 상태를 가진다: 안전(데미지 없음, 씨앗 성장 가능) / 위험(데미지 있음).
    /// 안전한 상태에서 씨앗과 닿아있으면 씨앗을 성장시킨다.
    /// Collider2D를 Is Trigger로 설정해서 사용한다.
    /// </summary>
    public class Water : MonoBehaviour, IPropertyChangeable
    {
        [SerializeField] private bool isSafe = true; // 기본 상태: 안전(씨앗 성장 가능)

        public void ChangeProperty()
        {
            isSafe = !isSafe;
        }

        public string GetPropertyPrompt()
        {
            return isSafe ? "Q 키를 눌러 위험하게 변환" : "Q 키를 눌러 안전하게 변환";
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!isSafe) return;

            Seed seed = other.GetComponent<Seed>();
            if (seed != null)
            {
                seed.Grow();
            }
        }

        public bool IsSafe => isSafe;
    }
}