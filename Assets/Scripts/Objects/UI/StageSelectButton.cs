using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FF.Systems;

namespace FF.UI
{
    /// <summary>
    /// 스테이지 선택 화면의 버튼 하나. 해당 스테이지가 잠겨있으면 클릭이 안 되고 자물쇠 아이콘을 보여준다.
    /// 잠금이 풀려있으면 클릭 시 그 스테이지 씬을 불러온다.
    /// </summary>
    public class StageSelectButton : MonoBehaviour
    {
        [Header("스테이지 정보")]
        [SerializeField] private int stageNumber = 1;
        [SerializeField] private string sceneName; // 이 스테이지의 씬 이름 (Build Settings에 등록되어 있어야 함)

        [Header("연결")]
        [SerializeField] private Button button;
        [SerializeField] private GameObject lockIcon; // 잠겨있을 때 보여줄 자물쇠 아이콘 (꺼둔 오브젝트를 연결)

        private void Start()
        {
            Refresh();
        }

        // 화면이 다시 보일 때(스테이지 깨고 돌아왔을 때 등) 호출하면 잠금 상태를 다시 반영한다
        public void Refresh()
        {
            bool unlocked = StageProgress.IsUnlocked(stageNumber);

            if (button != null)
            {
                button.interactable = unlocked;
            }

            if (lockIcon != null)
            {
                lockIcon.SetActive(!unlocked);
            }
        }

        // 버튼의 OnClick() 이벤트에 이 메서드를 연결해서 쓴다
        public void OnClickPlay()
        {
            if (!StageProgress.IsUnlocked(stageNumber)) return;
            if (string.IsNullOrEmpty(sceneName)) return;

            SceneManager.LoadScene(sceneName);
        }
    }
}