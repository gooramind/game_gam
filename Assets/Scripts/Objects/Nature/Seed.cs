using UnityEngine;

namespace FF.Objects.Nature
{
    /// <summary>
    /// 씨앗. 물(안전한 상태)이 닿으면 나무로 성장한다.
    /// 같은 위치에 미리 비활성화 상태로 놓아둔 나무 오브젝트(treeObject)를 활성화하는 방식으로 구현했다.
    /// (프리팹을 새로 생성하는 대신, 씨앗과 나무를 같은 자리에 겹쳐 놓고 켜고 끄는 게 에디터에서 다루기 더 쉽다)
    /// </summary>
    public class Seed : MonoBehaviour
    {
        [SerializeField] private GameObject treeObject; // 같은 위치에 미리 놓아둔, 비활성화된 나무 오브젝트

        public bool IsGrown { get; private set; } = false;

        public void Grow()
        {
            if (IsGrown) return;
            IsGrown = true;

            if (treeObject != null)
            {
                treeObject.SetActive(true);
            }

            gameObject.SetActive(false); // 씨앗은 사라지고 나무로 대체된다
        }
    }
}