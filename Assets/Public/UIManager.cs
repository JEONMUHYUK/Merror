using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyLibrary;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject aim;

    private UIManager() { }
    #region Player Stamina UI
    [SerializeField] private Slider Stamina = null;

    public void UpdateStamina(float changed)
    {
        Stamina.value = changed;
    }
    #endregion

    private void Awake()
    {
        // Move UI to front
        aim.transform.SetAsLastSibling();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        aim.SetActive(false);
    }

    // Add Pointer Script

    private void Update()
    {
        if (gameObject.scene.name == "Chapter1")
        {
            aim.SetActive(true);
        }
    }



    // Raycast �и� ���� 
    // *** ray -> Player : interactorable �Ǵ� 
    // O : mytype -> ���콺 Ŀ�� ����
    // X : default ���콺 Ŀ���� ����
    //---------------------------------------------------------------------
    // Contact Mylibrary => interactable Script

    /// <summary>
    /// When player mouse ray enter or exit the object.
    /// Change mouse aim 
    /// </summary>
    /// <param name="value">Outline enabled state</param>
    public void ChangeAimIcon(string myType, bool value)
    {
        if (gameObject.scene.name == "Chapter1")
        {
            aim.SetActive(!value);
        }
    }

/*
    public virtual void Do_Outline(bool value)
    {
        if (_Outlinable != null)
            _Outlinable.enabled = value;
        GameManager.Instance.GetUI().ChangeIcon(myType, value);
    }
*/
}
