namespace FF.Interaction
{
    /// <summary>
    /// Q키 상태변환으로 변환될 수 있는 모든 오브젝트가 구현하는 인터페이스.
    /// 변환됐을 때 무슨 일이 일어나는지는 각 오브젝트가 직접 작성한다.
    /// </summary>
    public interface ITransformable
    {
        /// <summary>
        /// 상태변환이 발동됐을 때 호출된다. 각자 자기만의 효과를 구현한다.
        /// </summary>
        void Transform();

        /// <summary>
        /// 이미 변환된 상태인지 (중복 변환 방지용)
        /// </summary>
        bool IsTransformed { get; }
    }
}