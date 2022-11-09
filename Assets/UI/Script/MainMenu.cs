using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MainMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject soundManager = null;
    //Image[] im = null;

    Button[] transButton;

    GameObject button;

    private void Awake()
    {
        soundManager = GameObject.Find("SoundOption");
        transButton = GameObject.Find("Menu").GetComponentsInChildren<Button>();
        //button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
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
#if UNITY_EDITOR
        // Works only in UnityEditor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        // Works only in Applications
        // Application.Quit();
    }

    // SoundButton Close
    public void OnClickClose()
    {
        soundManager.SetActive(false);
    }
/*
    public void PointerEnterHandler()
    {
        //NameCheck(button.name);
        Debug.Log(UnityEngine.EventSystems.EventSystem.current.gameObject.name);
        //UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        //buttonImage.enabled = true;

    }

    public void PointerExitHandler()
    {
        Debug.Log("������");
        //buttonImage.enabled = false;

    }

    private void NameCheck(string Name)
    {
        switch (Name)
        {
            case "StartButton":

                break;
        }
    }
*/
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
    }
}
