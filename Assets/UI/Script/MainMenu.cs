using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    GameObject soundManager = null;
    //Image[] im = null;

    private void Awake()
    {
        soundManager = GameObject.Find("SoundOption");
        //im = gameObject.GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        /*for (int i = 0; i < im.Length; i++)
        {
            im[i].enabled = false;
        }*/
    }

    // NewGameButton
    public void OnClickNewGame()
    {
        Debug.Log("���� ����");
        // LodingScene AsyncLoad
        SceneManager.LoadSceneAsync("LodingScene");
    }

    // OptionButton
    public void OnClickOption()
    {
        Debug.Log("�ɼ�");
        // Activate SoundManager UI
        soundManager.SetActive(true);
    }

    // SaveButton
    public void OnClickSave()
    {
        Debug.Log("�ҷ�����");
    }

    // ExitButton
    public void OnClickExit()
    {
        // Works only in UnityEditor
        UnityEditor.EditorApplication.isPlaying = false;

        // Works only in Applications
        // Application.Quit();
    }

    // SoundButton Close
    public void OnClickClose()
    {
        soundManager.SetActive(false);
    }

    public void PointerEnterHandler()
    {
        /*Image AA = this.gameObject.GetComponent<Image>();
        AA.enabled = true;*/


        Debug.Log("�ö�");
    }
}
