namespace FF.Interaction
{
    /// <summary>
    /// 플레이어와 상호작용할 수 있는 모든 오브젝트가 구현하는 인터페이스.
    /// 버튼, 로봇, 철문, 건전지 등 "상호작용(E키)"으로 동작하는 대상은 전부 이걸 구현한다.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// 플레이어가 상호작용 키를 눌렀을 때 호출된다.
        /// </summary>
        /// <param name="interactor">상호작용을 시도한 오브젝트 (보통 플레이어)</param>
        void Interact(UnityEngine.GameObject interactor);

        /// <summary>
        /// 화면에 표시할 안내 문구. 예: "E 키를 눌러 문 열기"
        /// </summary>
        string GetInteractionPrompt();
    }
}