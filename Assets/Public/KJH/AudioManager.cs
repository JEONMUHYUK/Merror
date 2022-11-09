using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    #region Singleton
    private static AudioManager instance = null;
    public static AudioManager Inst{ get { return instance; } }
    #endregion // Singleton
    //-----------------------------------------------------------------------------

    [SerializeField] ScriptableObj PlayerObj;
    [SerializeField] ScriptableObj PropsObj;
    [SerializeField] ScriptableObj EnviromentObj;
    [SerializeField] ScriptableObj DefaultObj;

    private Dictionary<string, AudioClip> PlayerClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> PropsClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> EnviromentClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> DefaultClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);

        AddAllClipsToDic(PlayerObj);
        AddAllClipsToDic(PropsObj);
        AddAllClipsToDic(EnviromentObj);
        AddAllClipsToDic(DefaultObj);
    }

    /// <summary>
    /// Add the clips held by the scriptable object to the Dictionary
    /// </summary>
    /// <param name="obj"> A scriptable object that contains audio clips. </param>
    private void AddAllClipsToDic(ScriptableObj obj)
    {
        switch (obj.name)
        {
            case "PlayerObj":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    PlayerClips.Add(PlayerObj.Sounds[i].name, PlayerObj.Sounds[i]);
                }
                break;

            case "PropsObj":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    PropsClips.Add(PropsObj.Sounds[i].name, PropsObj.Sounds[i]);
                }
                break;

            case "EnviromentObj":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    EnviromentClips.Add(EnviromentObj.Sounds[i].name, EnviromentObj.Sounds[i]);
                }
                break;

            case "DefaultObj":
                for (int i = 0; i < obj.Sounds.Length; i++)
                {
                    DefaultClips.Add(DefaultObj.Sounds[i].name, DefaultObj.Sounds[i]);
                }
                break;
        }
    }

    /// <summary>
    /// return requested Audioclip
    /// </summary>
    /// <param name="soundType"> The type containing the audio clip to request. </param>
    /// <param name="clipName"> The name of requested audioclip </param>
    /// <returns></returns>
    public AudioClip GetClip(Interactable.SoundType soundType, string clipName)
    {
        return CheckType(soundType).ContainsKey(clipName) ? CheckType(soundType)[clipName] : null;
    }

    /// <summary>
    /// Return the corresponding Dictionary via type
    /// </summary>
    /// <param name="soundType"> The type containing the audio clip to request. </param>
    /// <returns></returns>
    private Dictionary<string, AudioClip> CheckType(Interactable.SoundType soundType)
    {
        Dictionary<string, AudioClip> curdic = new Dictionary<string, AudioClip>();
        switch (soundType)
        {
            case Interactable.SoundType.Player:
                curdic = PlayerClips;
                break;

            case Interactable.SoundType.Props:
                curdic = PropsClips;
                break;

            case Interactable.SoundType.Enviroment:
                curdic= EnviromentClips;
                break;

            case Interactable.SoundType.Default:
                curdic = DefaultClips;
                break;
        }

        if (curdic == null)
        {
            Debug.LogError("The type does not exist");
            return null;
        }
        else return curdic;
    }
}
