using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public partial class Interactable : MonoBehaviour
{
    [Header("More Function")]
    [SerializeField] private bool Gettable = false;
    [SerializeField] private float Inventory_Scale = 1f;
    [SerializeField] private bool Special = false;
    public void SetSpecial(bool value) => Special = value;
    [SerializeField] private Light _Light = null;

    /// <summary>
    /// Do something special fuction.
    /// </summary>
    public virtual void Do_Special()
    {
        switch (myType)
        {
            case ObjectType.Toilet_Flush:
                {
                    Do_Flush();
                    break;
                }
            case ObjectType.Door:
                {
                    Do_Door();
                    break;
                }
            case ObjectType.Switch:
                {
                    Do_Light();
                    break;
                }
            case ObjectType.PuzzleGuessWho:
                {
                    Do_Puzzle_GuessWho();
                    break;
                }
            case ObjectType.PuzzleIsabellRoom:
                {
                    Do_Puzzme_IsabellRoom();
                    break;
                }
            case ObjectType.Mirror:
                {
                    Do_Break_Mirror();
                    break;
                }
            case ObjectType.PuzzleDecryption:
                {
                    Do_Puzzle_Decryption();
                    break;
                }
            case ObjectType.PuzzleFreemasonCipher:
                {
                    Do_PuzzleFreemasonCipher();
                    break;
                }
            case ObjectType.JukeBox:
                {
                    Do_JukeBox();
                    break;
                }

            default: break;
        }
    }
    /// <summary>
    /// Flush Toilet water.
    /// </summary>
    protected void Do_Flush()
    {
        if (TryGetComponent<Toilet>(out Toilet toilet))
        {
            StartCoroutine(toilet.Flush());
        }
        else
        {
            Debug.Log("This Object not have Flush Script.");
        }
    }
    /// <summary>
    /// Turn on / off light if this object is switch type.
    /// </summary>
    protected void Do_Light()
    {
        if (_Light != null)
        {
            _Light.enabled = !_Light.enabled;
        }
    }

    public virtual void Do_Puzzle_GuessWho()
    {
        if (TryGetComponent<PuzzleGuessWho>(out PuzzleGuessWho puzzleGuessWho))
        {
            puzzleGuessWho.SendMessage("CheckAnswer",SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Debug.Log("This Object not have PuzzleGuessWho Script.");
        }
    }

    /// <summary>
    /// Check Puzzle in IsabellRoom
    /// </summary>
    protected void Do_Puzzme_IsabellRoom()
    {
        if (TryGetComponent<Puzzle_IsabellRoom>(out Puzzle_IsabellRoom puzzle_IsabellRoom))
        {
            puzzle_IsabellRoom.SendMessage("CheckDolls", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Debug.Log("This Object not have Puzzle_IsabellRoom Script.");
        }
    }
    protected void Do_Break_Mirror()
    {
        if (TryGetComponent<Mirror>(out Mirror mirror))
        {
            Debug.Log("GetMirror");
            mirror.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Debug.Log("This Object not have Mirror Script.");
        }
    }

    protected virtual void Do_Puzzle_Decryption()
    {
        if(TryGetComponent<Decryption_Puzzle>(out Decryption_Puzzle puzzleDecryption))
        {
            puzzleDecryption.SendMessage("VisibleUI",SendMessageOptions.DontRequireReceiver);
        }
        else if(TryGetComponent<ZoomInHint>(out ZoomInHint zoomInHint))
        {
            zoomInHint.SendMessage("ZoomIn",SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Debug.Log("This is not Puzzle Decryption");
        }
    }

    protected virtual void Do_PuzzleFreemasonCipher()
    {
        if (TryGetComponent<FreemasonCipher>(out FreemasonCipher freemasonCipher))
        {
            freemasonCipher.SendMessage("inputBoard", SendMessageOptions.DontRequireReceiver);
        }
        
        else
        {
            Debug.LogError("Require to CMJ & SSH");
        }
    }

    protected virtual void Do_JukeBox()
    { 
        CameraState cameraState = FindObjectOfType<CameraState>();
        cameraState.TurnOnState(CameraState.CamState.PANIC);
        cameraState.callBackPanic = () => { Debug.Log("Call Isabel"); };
    }

    protected virtual void Do_Door()
    {
        _MySource.Stop();
        if (IsUsed) _MyClip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Interactable, "Door_Close");
        else _MyClip = GameManager.Instance.GetAudio().GetClip(AudioManager.Type.Interactable, "Door_Open");

        _MySource.clip = _MyClip;
        _MySource.Play();
    }
}
