using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] GameObject allLevelButton;
    [SerializeField] GameObject warningMessage;

    private void OnEnable()
    {
        for (int btn = 0; btn < allLevelButton.transform.childCount; btn++)
        {
            allLevelButton.transform.GetChild(btn).GetChild(0).GetComponent<Text>().text = (btn + 1).ToString();
            int val = btn + 1;
            allLevelButton.transform.GetChild(btn).GetComponent<Button>().onClick.AddListener(() => OnButtonClkEvent(val));
        }
    }

    private void OnButtonClkEvent(int btnNo)
    {//CHECKING IF THE LEVEL IS UNLOCK OR NOT
        if (btnNo <= PlayerPrefs.GetInt(ResourceManager.TotalUnlockLevel))
        {
            PlayerPrefs.SetInt(ResourceManager.Level, btnNo);
            GameManager.instance.LoadLevel();
        }
        else
        {
            StartCoroutine(ShowWarning());
        }
    }
    IEnumerator ShowWarning()
    {
        warningMessage.SetActive(true);
        yield return new WaitForSeconds(1f);
        warningMessage.SetActive(false);
    }
}
