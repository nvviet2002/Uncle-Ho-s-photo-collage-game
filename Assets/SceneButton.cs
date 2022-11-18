using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
{
    public int sceneIndex;
    public string sceneName;
    public RectTransform startContainer;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Click);

        LoadDataSceneButton();
    }

    private void Click()
    {
        GameController.Instance.LoadScene(sceneIndex);
    }

    public void LoadDataSceneButton()
    {
        int point = PlayerPrefs.GetInt(sceneName,0);
        if (point <= 0) return;
        foreach(RectTransform star in startContainer)
        {
            if (point == 0) break;
            star.GetComponent<Image>().sprite = GameController.Instance.gameInfo.yellowStar;
            point--;
        }
    }
}