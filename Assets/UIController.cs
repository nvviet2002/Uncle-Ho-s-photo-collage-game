using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    public static UIController Instance { get => instance; set => instance = value; }

    public RectTransform loadingBar;
    public RectTransform sceneContainer;
    public RectTransform menu;
    public RectTransform winPanel;
    public RectTransform winStarContainer;
    public CanvasGroup winPanelCanvas;
    public CanvasGroup startPanelCanvas;
    public RectTransform timerPanel;
    public TMPro.TMP_Text minuteText;
    public TMPro.TMP_Text secondText;
    public RectTransform soundButton;
    public RectTransform muteIcon;
    public RectTransform nextSceneButton;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
    }

    public void SetActiveLoadingBar(bool _active)
    {
        if (loadingBar != null)
        {
            loadingBar.gameObject.SetActive(_active);
        }
    }

    public void SetActiveSceneContainer(bool _active)
    {
        if (sceneContainer != null) sceneContainer.gameObject.SetActive(_active);
        if(_active == true)
        {
            foreach(RectTransform sceneBtn in sceneContainer)
            {
                sceneBtn.GetComponent<SceneButton>().LoadDataSceneButton();
            }
        }
    }

    public void SetActiveMenu(bool _active)
    {
        if (menu != null) menu.gameObject.SetActive(_active);
    }

    public void SetActiveWinPanel(bool _active,int _stars)
    {
        if (_active)
        {
            winPanelCanvas.alpha = 0;
            winPanelCanvas.DOFade(1, 1f);
            winPanel.localPosition = new Vector3(0, 500f, 0);
            winPanel.DOAnchorPosY(0, 1, true).SetEase(Ease.OutElastic);
            foreach(RectTransform star in winStarContainer)
            {
                if (_stars <= 0) break;
                star.GetComponent<Image>().sprite = GameController.Instance.gameInfo.yellowStar;
                _stars--;
            }
            return;
        }
        winPanelCanvas.alpha = 1;
        winPanelCanvas.DOFade(0, 0.5f);
        winPanel.DOAnchorPosY(-500f, 0.5f, true).SetEase(Ease.OutElastic);
    }

    public void SetActiveStartPanel(bool _active)
    {
        if (_active)
        {
            startPanelCanvas.alpha = 0;
            startPanelCanvas.blocksRaycasts = true;
            startPanelCanvas.DOFade(1f, 1f);
            return;
        }
        startPanelCanvas.alpha = 1;
        startPanelCanvas.blocksRaycasts = false;
        startPanelCanvas.DOFade(0f, 1f);
    }

    public void SetActiveSoundButton(bool _active)
    {
        soundButton.gameObject.SetActive(_active);
    }

    public void SetActiveTimerPanel(bool _active)
    {
        timerPanel.gameObject.SetActive(_active);
        minuteText.text = "00:";
        secondText.text = "00";
    }

    public void InteractableNextButton(bool _enable)
    {
        nextSceneButton.GetComponent<Button>().interactable = _enable;
    }

    public void SetActiveMuteIcon(bool _active)
    {
        muteIcon.gameObject.SetActive(_active);
    }

    public void ClickStartButton()
    {
        Debug.Log("Clicked");
        UIController.Instance.SetActiveStartPanel(false);
        SceneController.Instance.StartScene();
        GameController.Instance.EnableClickSound(true);
    }

    public void ClickSceneMenuButton()
    {
        UIController.Instance.SetActiveWinPanel(false,0);
        GameController.Instance.LoadScene(0);
        GameController.Instance.EnableClickSound(true);
    }

    public void ClickResetSceneButton()
    {
        UIController.Instance.SetActiveWinPanel(false,0);
        GameController.Instance.LoadScene(SceneController.Instance.sceneIndex);
        GameController.Instance.EnableClickSound(true);
    }

    public void ClickNextSceneButton()
    {
        if (SceneController.Instance == null) return;
        UIController.Instance.SetActiveWinPanel(false,0);
        GameController.Instance.LoadScene(SceneController.Instance.sceneIndex + 1);
        GameController.Instance.EnableClickSound(true);
    }

    public void ClickSoundButton()
    {
        GameController.Instance.ActiveSound();
        GameController.Instance.ReloadActiveSound();
        GameController.Instance.EnableClickSound(true);
    }

    public void ChangeValueTimer(float _time)
    {
        minuteText.text = ((int)(_time/60)).ToString("00")+ ":";
        secondText.text = ((int)_time%60).ToString("00") ;
    }

}
