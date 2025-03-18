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
        if(Instance != this)        // �ν��Ͻ� �ߺ� ����
        {
            Destroy(gameObject); return;
        }DontDestroyOnLoad(gameObject);
    }
    private static LoadingScript Init() // Resource������ �ε�Ui �������� ������.
    {
        return Instantiate(Resources.Load<LoadingScript>("LoadingUi"));
    }
    IEnumerator Fade(bool isFade)       // �ڿ������� fadeȿ�� �־���
    {
        float time = 0f;
        while(time <= 1f)
        {
            time += Time.unscaledDeltaTime* 2.5f;
            canvasGroup.alpha = isFade ? Mathf.Lerp(0f, 1f, time) : Mathf.Lerp(1f, 0f, time);
            yield return null;
        }
        if(!isFade) gameObject.SetActive(false);    // fade���°� �ƴϸ� false
    }
    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoadFinished;    // �� �ε� �Ϸ��ϸ� �̺�Ʈ �߰�
        loadScene = sceneName;
        StartCoroutine("LoadSceneCo");
    }

    IEnumerator LoadSceneCo()   // �񵿱� �� �ε� ����
    {
        slider.value = 0f;
        yield return StartCoroutine(Fade(true));        // ���̵� �� ����
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadScene);
        operation.allowSceneActivation = false; // �ε��� �� �ɶ����� �� �̵� X
        float time = 0f;
        while (!operation.isDone)   // �ε��� �ʹ� ���� ä������ �ȵǱ� ������
        {                                   // 90%�� �� ���� 1�ʵ����� ���� 10% ä�쵵�� ��
            if (operation.progress < 0.9f)
            {
                slider.value = operation.progress;  // �����̴��� �����Ȳ ����
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
        if(arg0.name == loadScene)  // �ε��� �������� ȿ��
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoadFinished;
        }
    }
}