using UnityEngine;

namespace FF.Player
{
    /// <summary>
    /// 플레이어가 땅에 붙어서 이동 중일 때 Walk 사운드를 반복 재생한다.
    /// 점프해서 공중에 뜨거나 멈추면 Pause(일시정지), 다시 걷기 시작하면 멈췄던 지점부터 이어서 재생한다.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class PlayerWalkAudio : MonoBehaviour
    {
        [Header("연결")]
        [SerializeField] private PlayerController playerController;

        [Header("사운드 설정")]
        [SerializeField] private AudioClip walkClip; // Assets/Sounds/Walk
        [SerializeField] private float volume = 0.7f;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = volume;
            audioSource.clip = walkClip;

            if (playerController == null)
            {
                playerController = GetComponent<PlayerController>();
            }

            if (playerController == null)
            {
                Debug.LogWarning("[PlayerWalkAudio] PlayerController를 찾을 수 없습니다. 연결을 확인하세요.");
            }

            if (walkClip == null)
            {
                Debug.LogWarning("[PlayerWalkAudio] Walk Clip이 비어있습니다. 인스펙터에서 연결해주세요.");
            }
        }

        private void Update()
        {
            if (playerController == null || walkClip == null) return;

            bool shouldWalk = playerController.IsGrounded && playerController.IsMoving;

            if (shouldWalk)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.UnPause(); // 일시정지 상태였다면 이어서 재생
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play(); // 처음 재생하는 경우
                        Debug.Log("[PlayerWalkAudio] Walk 사운드 재생 시작");
                    }
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Pause(); // 점프/정지 시 일시정지 (위치 유지)
                    Debug.Log("[PlayerWalkAudio] Walk 사운드 일시정지 (점프 또는 정지)");
                }
            }
        }
    }
}