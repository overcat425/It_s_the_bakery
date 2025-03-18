using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading;

public class LoadingScript : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Slider slider;
    string loadScene;
    private static LoadingScript instance;
    public static LoadingScript Instance
    {
        get
        {
            if (instance == null)
            {
                var inst = FindObjectOfType<LoadingScript>();
                if (inst != null)
                {
                    instance = inst;
                }else
                {
                    instance = Init();
                }
            }return instance;
        }
    }
    void Awake()
    {
        if(Instance != this)        // 인스턴스 중복 방지
        {
            Destroy(gameObject); return;
        }DontDestroyOnLoad(gameObject);
    }
    private static LoadingScript Init() // Resource폴더의 로딩Ui 프리팹을 생성함.
    {
        return Instantiate(Resources.Load<LoadingScript>("LoadingUi"));
    }
    IEnumerator Fade(bool isFade)       // 자연스럽게 fade효과 넣어줌
    {
        float time = 0f;
        while(time <= 1f)
        {
            time += Time.unscaledDeltaTime* 2.5f;
            canvasGroup.alpha = isFade ? Mathf.Lerp(0f, 1f, time) : Mathf.Lerp(1f, 0f, time);
            yield return null;
        }
        if(!isFade) gameObject.SetActive(false);    // fade상태가 아니면 false
    }
    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoadFinished;    // 씬 로드 완료하면 이벤트 추가
        loadScene = sceneName;
        StartCoroutine("LoadSceneCo");
    }

    IEnumerator LoadSceneCo()   // 비동기 씬 로딩 시작
    {
        slider.value = 0f;
        yield return StartCoroutine(Fade(true));        // 페이드 인 시작
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadScene);
        operation.allowSceneActivation = false; // 로딩이 다 될때까지 씬 이동 X
        float time = 0f;
        while (!operation.isDone)   // 로딩이 너무 빨리 채워지면 안되기 때문에
        {                                   // 90%가 다 차면 1초동안은 남은 10% 채우도록 함
            if (operation.progress < 0.9f)
            {
                slider.value = operation.progress;  // 슬라이더로 진행상황 전시
            }
            else
            {
                time += Time.unscaledDeltaTime;
                slider.value = Mathf.Lerp(0.9f, 1f, time);
                if(slider.value >= 1f)
                {
                    operation.allowSceneActivation = true;
                    yield break;
                }
            }yield return null;
        }
    }

    private void OnSceneLoadFinished(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == loadScene)  // 로딩이 끝났을때 효과
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoadFinished;
        }
    }
}