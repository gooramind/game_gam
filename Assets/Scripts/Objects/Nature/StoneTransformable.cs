using UnityEngine;
using FF.Interaction;

public class StoneTransformable : MonoBehaviour, ITransformable
{
    [SerializeField] private float transformedMass = 1f;

    private bool isTransformed = false;
    public bool IsTransformed => isTransformed;

    // 🌟 돌이 변환되면: 밀 수 있는 상태(Dynamic)로 바뀜
    public void Transform()
    {
        if (isTransformed) return;
        isTransformed = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.mass = transformedMass;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        Debug.Log(gameObject.name + " : 돌이 밀 수 있게 변환됨!");
    }
}