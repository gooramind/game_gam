using UnityEngine;

namespace FF.Systems
{
    /// <summary>
    /// 게임 내 아이템 하나의 정보를 담는 데이터.
    /// 돌, 씨앗, 건전지, 덩쿨 등 인벤토리에 들어갈 수 있는 모든 아이템을 이 ScriptableObject로 만든다.
    /// Project 창에서 우클릭 > Create > F&F > Item Data 로 생성할 수 있다.
    /// </summary>
    [CreateAssetMenu(fileName = "NewItemData", menuName = "F&F/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("기본 정보")]
        public string itemId;          // 코드에서 비교할 고유 ID (예: "battery", "stone")
        public string displayName;     // 화면에 보여줄 이름
        public Sprite icon;            // 인벤토리 칸에 표시할 아이콘

        [Header("스택 설정")]
        public int maxStack = 1;       // 한 칸에 최대 몇 개까지 쌓일 수 있는지
    }
}