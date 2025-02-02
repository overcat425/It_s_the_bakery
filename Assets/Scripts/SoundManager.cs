using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("브금")]
    [SerializeField] AudioClip[] bgmClips;
    AudioSource[] bgmSources;
    [SerializeField] float bgmVolume;
    AudioHighPassFilter bgmHighPassFilter;

    [Header("효과음")]
    [SerializeField] AudioClip[] effectClips;
    [SerializeField] float effectVolume;
    AudioSource[] effectSources;
    [SerializeField] int channels;
    int channelIndex;
    public enum Effect { Click, Btn, Customer, Counter, Horn, Horn2, Gauge, Scale }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject);}
        //instance = this;
        Init();
    }
    private void Start()
    {
        SoundManager.instance.PlayBgm(true);
    }
    void Init()
    {
        GameObject bgmObject = new GameObject("BgmObject");
        bgmObject.transform.parent = transform;     // 사운드매니저 자식 오브젝트로 생성
        bgmSources = new AudioSource[2];        // 랜덤 브금 2개
        for (int i = 0; i < bgmSources.Length; i++)
        {
            bgmSources[i] = bgmObject.AddComponent<AudioSource>();  // bgm 실행 시 bgm의 컴포넌트로
            bgmSources[i].playOnAwake = true;
            bgmSources[i].loop = true;
            bgmSources[i].clip = bgmClips[i];
            bgmSources[i].volume = bgmVolume;
        }
        bgmHighPassFilter = Camera.main.GetComponent<AudioHighPassFilter>();

        GameObject effectObject = new GameObject("EffectObject");
        effectObject.transform.parent = transform;
        effectSources = new AudioSource[channels];
        for (int i = 0; i < effectSources.Length; i++)
        {
            effectSources[i] = effectObject.AddComponent<AudioSource>();
            effectSources[i].playOnAwake = false;
            effectSources[i].volume = effectVolume;
            effectSources[i].bypassListenerEffects = true; // 오디오하이패스필터 적용
        }
    }
    public void PlaySound(Effect effect)
    {
        for (int i = 0; i < effectSources.Length; i++)
        {
            int loopChannel = (i + channelIndex) % effectSources.Length;    // 채널수를 넘어가지 않도록 함
            if (effectSources[loopChannel].isPlaying) continue; // 같은 오디오컴포넌트 사용 방지
            channelIndex = loopChannel;
            effectSources[loopChannel].clip = effectClips[(int)effect];
            effectSources[loopChannel].Play();
            break;
        }
    }
    public void PlayBgm(bool isPlaying)     // 브금 선정
    {
        if (isPlaying)
        {
            //bgmSources[1].Stop();
            bgmSources[0].Play();
        }
        else
        {
            bgmSources[0].Stop();
            //bgmSources[1].Play();
        }
    }
    public void StopBgm(bool isPlaying)
    {
        bgmHighPassFilter.enabled = isPlaying;
    }
    public void BtnSound()
    {
        PlaySound(Effect.Click);
    }
    public void ToggleMute()
    {
        if (AudioListener.volume > 0)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }
}
