using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApplicationManager : MonoBehaviour {

    public GameObject menuScreen;
    public GameObject loadingScreen;
    public GameObject UIWalls;
    public Slider slider;
    public Text progressText;

    public void Begin()
    {
        menuScreen.SetActive(false);
        loadingScreen.SetActive(true);
        UIWalls.SetActive(true);
        StartCoroutine(LoadLevel(1));
    }

    IEnumerator LoadLevel(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = progress*100f+"%";
            yield return null;
        }
        
    }
}
