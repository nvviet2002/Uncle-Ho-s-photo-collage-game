using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get => instance; set => instance = value; }

    public GameInfomation gameInfo;

    public AudioSource mainSound;
    public AudioSource snapSound;
    public AudioSource winSound;
    public AudioSource clickSound;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        LoadData();
        ReloadActiveSound();
    }

    public void LoadData()
    {
        if(PlayerPrefs.GetInt("Sound",0) == 0)
        {
            UIController.Instance.SetActiveMuteIcon(true);
        }
        else if(PlayerPrefs.GetInt("Sound",0) == 1)
        {
            UIController.Instance.SetActiveMuteIcon(false);
        }
    }

    public void ActiveSound()
    {
        if(PlayerPrefs.GetInt("Sound") == 0)
        {
            PlayerPrefs.SetInt("Sound",1);
            LoadData();
            return;
        }
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
            LoadData();
        }
    }

    public void Win(string _sceneName , int _point)
    {
        Debug.Log("Win scene " + _sceneName);
        PlayerPrefs.SetInt(_sceneName, _point);
        UIController.Instance.SetActiveWinPanel(true,_point);
        EnableWinSound(true);
    }

    public void LoadScene(int _sceneIndex)
    {
        UIController.Instance.SetActiveLoadingBar(true);
        if(_sceneIndex == 0)
        {
            UIController.Instance.SetActiveSceneContainer(true);
        }
        else
        {
            UIController.Instance.SetActiveSceneContainer(false);
        }
        
        StartCoroutine(AsyncLoadScene(_sceneIndex));
        
    }

    private IEnumerator AsyncLoadScene(int _index)
    {
        AsyncOperation asyncOpe = SceneManager.LoadSceneAsync(_index,LoadSceneMode.Single);
        asyncOpe.allowSceneActivation = false;
        float progress = 0f;
        for (; asyncOpe.isDone == false;)
        {
            progress = Mathf.MoveTowards(progress, asyncOpe.progress, Time.deltaTime);
            if (progress >= 0.9f)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                asyncOpe.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        Debug.Log("Loaded" + _scene.name);
        UIController.Instance.SetActiveLoadingBar(false);
        if(_scene.buildIndex != 0)
        {
            UIController.Instance.SetActiveStartPanel(true);
            UIController.Instance.SetActiveTimerPanel(true);
            EnableMainSound(false);
        }
        else
        {
            UIController.Instance.SetActiveTimerPanel(false);
            EnableMainSound(true);
            EnableWinSound(false);
        }
        if (_scene.buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            UIController.Instance.InteractableNextButton(false);
        }
        else
        {
            UIController.Instance.InteractableNextButton(true);
        }
    }

    public void EnableMainSound(bool _enable)
    {
        if (_enable) mainSound.Play();
        else mainSound.Stop();
    }

    public void EnableSnapSound(bool _enable)
    {
        if (_enable) snapSound.Play();
    }

    public void EnableClickSound(bool _enable)
    {
        if (_enable) clickSound.Play();
    }

    public void EnableWinSound(bool _enable)
    {
        if (_enable) winSound.Play();
    }

    public void ReloadActiveSound()
    {
        if (PlayerPrefs.GetInt("Sound",0) == 0)
        {
            mainSound.volume = 0;
            snapSound.volume = 0;
            clickSound.volume = 0;
            winSound.volume = 0;
        }
        else
        {
            mainSound.volume = 0.5f;
            snapSound.volume = 1;
            clickSound.volume = 1;
            winSound.volume = 1;
        }
    }

}
