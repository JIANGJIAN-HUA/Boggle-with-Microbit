using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressSceneLoader : MonoBehaviour
{
    [SerializeField]
    private Text progressText;
    [SerializeField]
    private Slider slider;

    private AsyncOperation operation;
    private Canvas canvas;

    private static ProgressSceneLoader _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            canvas = GetComponentInChildren<Canvas>(true);
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(ushort sceneID)
    {
        UpdateProgressUI(0);
        canvas.gameObject.SetActive(true);

        StartCoroutine(BeginLoad(sceneID));
    }

    private IEnumerator BeginLoad(ushort sceneID)
    {
        int UIProgress = 0;
        int loadProgress;
        operation = SceneManager.LoadSceneAsync(sceneID);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            loadProgress = (int)operation.progress * 100;
            while(UIProgress < loadProgress)
            {
                UpdateProgressUI(++UIProgress);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        loadProgress = 100;
        while(UIProgress < loadProgress)
        {
            UpdateProgressUI(++UIProgress);
            yield return new WaitForEndOfFrame();
        }

        operation.allowSceneActivation = true;
        operation = null;
        canvas.gameObject.SetActive(false);
    }

    private void UpdateProgressUI(int progress)
    {
        slider.value = (float)progress/100f;
        progressText.text = progress + "%";
    }
}