using UnityEngine;
using FF.Interaction;

public class ButtonTransformable : MonoBehaviour, ITransformable
{
    private bool isTransformed = false;
    public bool IsTransformed => isTransformed;

    // 🌟 Q키(상태변환)를 맞았을 때 실행되는 함수
    public void Transform()
    {
        if (isTransformed) return; // 이미 변환되었다면 중복 실행 방지

        isTransformed = true;

        // 같은 게임오브젝트에 있는 ButtonMechanism 컴포넌트를 가져옴
        ButtonMechanism buttonMechanism = GetComponent<ButtonMechanism>();

        if (buttonMechanism != null)
        {
            // 토글 모드를 켜줌 (한 번 밟으면 계속 눌린 상태 유지)
            buttonMechanism.isToggleButton = true;
            Debug.Log(gameObject.name + " : 누름판이 토글 모드로 변환됨!");
        }
        else
        {
            Debug.LogError("ButtonMechanism 컴포넌트를 찾을 수 없습니다!");
        }
    }
}