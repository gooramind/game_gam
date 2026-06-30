using UnityEngine;

namespace FF.Player
{
    /// <summary>
    /// 플레이어의 사망/리스폰을 담당한다. 지금은 단순하게 씬이 시작된 위치로 되돌리는 방식이다.
    /// 불(Fire) 등 위험 오브젝트가 이 컴포넌트의 Die()를 호출한다.
    /// </summary>
    public class PlayerRespawn : MonoBehaviour
    {
        private Vector3 spawnPoint;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spawnPoint = transform.position; // 씬이 시작될 때 위치를 스폰 지점으로 기억해둔다
        }

        public void Die()
        {
            transform.position = spawnPoint;

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            Debug.Log("플레이어 사망 - 시작 위치로 돌아갑니다.");
            // TODO: 나중에 사망 연출(이펙트, 사운드)이나 결과화면 연결은 여기에 추가
        }
    }
}