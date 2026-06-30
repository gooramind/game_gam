using UnityEngine;
using FF.Interaction;
using FF.Player;

namespace FF.Objects.Nature
{
    /// <summary>
    /// 불. 두 가지 상태를 가진다: 위험(데미지 있음, 기본값) / 안전(데미지 없음).
    /// 위험한 상태에서 플레이어가 닿으면 사망(리스폰) 처리한다.
    /// Collider2D를 Is Trigger로 설정해서 사용한다.
    /// </summary>
    public class Fire : MonoBehaviour, IPropertyChangeable
    {
        [SerializeField] private bool isDangerous = true; // 기본 상태: 데미지 있음

        public void ChangeProperty()
        {
            isDangerous = !isDangerous;
        }

        public string GetPropertyPrompt()
        {
            return isDangerous ? "Q 키를 눌러 불 끄기" : "Q 키를 눌러 다시 불 붙이기";
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isDangerous) return;

            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                respawn.Die();
            }
        }

        public bool IsDangerous => isDangerous;
    }
}