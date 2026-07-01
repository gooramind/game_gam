using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    [Header("BGM 음원 파일 넣기")]
    public AudioClip startAudio;
    public AudioClip gameAudio;

    [Header("🔊 BGM 볼륨 조절")]
    [Tooltip("0(무음)부터 1(최대 소리)까지 조절 가능합니다.")]
    [Range(0f, 1f)]
    public float bgmVolume = 0.3f;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = bgmVolume;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName == "StartScene" || sceneName == "MenuScene")
        {
            PlayBGM(startAudio);
        }
        else if (sceneName.StartsWith("Stage"))
        {
            PlayBGM(gameAudio);
        }
    }

    public void PlayBGM(AudioClip newClip)
    {
        if (newClip == null) return;
        if (audioSource.clip == newClip) return;

        audioSource.clip = newClip;
        audioSource.Play();
    }
}