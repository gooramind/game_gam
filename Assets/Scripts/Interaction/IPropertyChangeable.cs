namespace FF.Interaction
{
    /// <summary>
    /// 성질변환이 가능한 자연/기계 오브젝트가 구현하는 인터페이스.
    /// 예: 돌(무거움 ↔ 가벼움), 물, 불, 버튼, 건전지 등.
    /// </summary>
    public interface IPropertyChangeable
    {
        void ChangeProperty();
        string GetPropertyPrompt();
    }
}