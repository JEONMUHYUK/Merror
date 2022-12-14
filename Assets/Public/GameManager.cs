using Cinemachine;
using MyLibrary;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Unity.VisualScripting;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        InitAudioManager();
        InitUIManager();
        InitPuzzleManager();
        InitVideoPlayerManager();
        InitIdentityManager();
        InitCaptureManager();
        DefaultInit();
        //InitCutScenePlayer();
        SceneManager.activeSceneChanged -= OnSceneChanged;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }
    private void Update()
    {
        GameInput.UpdateKey();
        GetVideoPlayer().SkipVideo();
    }
    #region Check current scene and initialize something work
    private void OnSceneChanged(Scene previous, Scene current) // Called when scene changed
    {
        if (current.isLoaded)
        {
            switch (current.name)
            {
                default: { ToggleCursor(false); break; }
                case "Chapter1": { StartCoroutine(WaitLoad(1)); break; }
                case "Chapter2": { StartCoroutine(WaitLoad(2)); break; }
                    //case "Chapter1_KYM": { InitChapter(1); break; } // test
            }
        }
    }
    /// <summary>
    /// Initialize chapter information by chapter number
    /// </summary>
    /// <param name="chapterNum">Chapter number</param>

    private int chapterNum;
    public int ChapterNum { get { return chapterNum; } }
    public bool completeLoad = false;

    private IEnumerator InitChapter(int chapterNum)
    {
        ToggleCursor(true); // Lock cursor
        FindPlayer(); // Find my player
        FindPlayerCamera(); // Find camera
        InitInventory(); // Init inventory
        _Puzzle.InitPuzzle(chapterNum); // Init puzzles
        yield return StartCoroutine(_UIManager.InitAcquisitionNotification());
        this.chapterNum = chapterNum;

        if (isFirstPlay == false)
        {
            LoadData(chapterNum);

        }
        //else
        //{
        //    //Test op video
        //    GetVideoPlayer().CallPlayVideo(
        //        GetVideoPlayer().getVideoClips.getChapter1.OP,
        //    () =>
        //    { 
        //        Debug.Log("OPVideOff");
        //    });
        //}

        yield return null;
    }

    private IEnumerator WaitLoad(int chapterNum)
    {
        TimeControl.Pause();
        yield return InitChapter(chapterNum);
        TimeControl.Play();
        completeLoad = true;
        _IdentityManager.InstIdentity();
    }
    #endregion
    #region Player & Camera Management
    private Player MyPlayer = null;
    public Player GetPlayer() { return MyPlayer; }
    public void FindPlayer() { MyPlayer = FindObjectOfType<Player>(); }

    private CinemachineVirtualCamera PlayerCam = null;
    public CinemachineVirtualCamera GetPlayerCamera() { return PlayerCam; }
    public void FindPlayerCamera()
    {
        if (MyPlayer != null)
        {
            if (MyPlayer.transform.GetChild(0).TryGetComponent<CinemachineVirtualCamera>(out CinemachineVirtualCamera cam))
            {
                PlayerCam = cam;
                return;
            }
            else Debug.LogError("Find Player Camera : Failed to get component of player virtual camera");
            return;
        }
        Debug.Log("Find Player Camera : My Player is null ptr");
    }
    #endregion // Find my player and save reference and camera.
    #region Inventory Management
    public bool ShowInven = false;
    private Inventory MyInventory = null;
    public Inventory GetInventory() { return MyInventory; }
    private Canvas InventoryCanvas = null;
    private TextMeshProUGUI Notice = null;
    /// <summary>
    /// Initialize inventory function. Get notice TMP object and inventory canvas.
    /// </summary>
    public void InitInventory()
    {
        TryGetComponent<Inventory>(out Inventory inventory);
        MyInventory = inventory == null ? null : inventory;
        MyInventory ??= this.AddComponent<Inventory>();
        MyInventory.InitInventory();
    }
    /// <summary>
    /// Input key 'I', player can toggle inventory UI.
    /// </summary>
    public void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInven = !ShowInven;
            if (ShowInven)
            {
                MyInventory.ShowInventory();
                ToggleCursor(false);
            }
            else
            {
                MyInventory.HideInventory();
                ToggleCursor(true);
            }
        }
        if (ShowInven)
        {
            if (MyInventory.GetItemCount > 0)
            {
                MyInventory.RotationItem();
            }
        }
    }
    public void ClearPuzzle(string puzzleName, GameObject puzzleItem, float scale)
    {
        GameObject obj = Instantiate(puzzleItem, Vector3.zero, Quaternion.identity);
        Debug.Log(obj.name);
        MyInventory.InsertItem(obj, scale);
        SetClear(puzzleName);
    }
    #endregion // Set inventory
    #region Audio Management
    private AudioManager _AudioManager = null;
    private void InitAudioManager() => _AudioManager = GetComponent<AudioManager>();
    public AudioManager GetAudio() => _AudioManager;
    #endregion
    #region UI Management
    [SerializeField] private UIManager _UIManager = null;
    private void InitUIManager() => _UIManager = GetComponent<UIManager>();
    public UIManager GetUI() => _UIManager;
    #endregion
    #region Puzzle Management
    private PuzzleManager _Puzzle = null;
    private void InitPuzzleManager() => _Puzzle = GetComponent<PuzzleManager>();
    public PuzzleManager GetPuzzle() => _Puzzle;

    public void SetClear(string puzzleName)
    {
        _Puzzle.SetClear(puzzleName, true);
    }


    #endregion
    #region Cut Scene
    private VideoPlayerManager _VideoPlayer = null;
    private void InitVideoPlayerManager() => _VideoPlayer = GetComponent<VideoPlayerManager>();
    public VideoPlayerManager GetVideoPlayer() => _VideoPlayer;
    private VideoPlayer CutScenePlayer = null;
    public VideoClip CutSceneClip = null;
    private void InitCutScenePlayer()
    {
        CutScenePlayer = GetComponent<VideoPlayer>();
        CutScenePlayer.enabled = false;
    }
    public void PlayCutScene(int CutSceneIndex)
    {
        // GetMyClip from data
        // CutSceneClip = GetClip();
        CutScenePlayer.enabled = true;
        CutScenePlayer.targetCamera = Camera.main;
        CutScenePlayer.clip = CutSceneClip;
        if (CutSceneClip == null) { Debug.Log("There's no cut scene clip."); return; }
        CutScenePlayer.Play();
        StartCoroutine(WhenCutSceneEnd());
    }
    private IEnumerator WhenCutSceneEnd()
    {
        yield return new WaitForSecondsRealtime(1f);
        while (true)
        {
            if (CutScenePlayer.isPlaying == false)
            {
                Debug.Log("Cut scene playing finished.");
                CutScenePlayer.clip = null;
                CutScenePlayer.enabled = false;
                yield break;
            }
            yield return null;
        }
    }
    #endregion
    #region SaveLoad Management
    private SaveData Data;
    public SaveData GetData() => Data;
    public bool isFirstPlay = true;
    public void Save()
    {
        if (SetSaveData() == true)
            SaveLoad.Save(Data);
        else
        {
            Debug.LogError("Can Not Save Data Request BK");
            return;
        }
    }

    public void Load()
    {
        SaveData? loadData = SaveLoad.Load();

        if (loadData != null)
        {
            Data = (SaveData)loadData;
            isFirstPlay = false;
            Debug.Log("Load Complete");
            if (SceneManager.GetActiveScene().name == "Chapter" + chapterNum.ToString())
            {
                CameraState cameraState = GameObject.Find("PostProcess").GetComponent<CameraState>();
                if (cameraState != null) cameraState.TurnOnState(CameraState.CamState.FADEIN);
            }

        }
        else
        {
            isFirstPlay = true;
            return;
        }
    }

    private void DefaultInit()
    {
        Data.chapter = 1;
    }

    #region Make Save Data
    private bool SetSaveData()
    {
        Data = new SaveData();
        GameObject Player = FindObjectOfType<Player>().gameObject;

        Data.chapter = chapterNum;
        if (Player == null)
            return false;
        Data.playerPos = Player.transform.position;
        Data.playerRot = Player.transform.rotation;

        if (_Puzzle == null)
            return false;
        Data.puzzles = _Puzzle.GetCurrentPuzzle().chapterPuzzleDatas;
        SetSaveDoorDatas();
        SetSaveInvenItems();
        SetSaveMirrors();
        Data.CanLight = true;
        return true;
    }

    private void SetSaveDoorDatas()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        Data.isDoorOpen = new Door[doors.Length];

        for (int i = 0; i < doors.Length; i++)
        {
            Data.isDoorOpen[i].doorName = doors[i].name;
            Data.isDoorOpen[i].isOpened = !doors[i].GetComponent<Interactable>().IsLocked;
        }
    }

    private void SetSaveInvenItems()
    {
        Data.invenItems = new string[MyInventory.GetItemCount];

        for (int i = 0; i < MyInventory.GetItemCount; i++)
        {
            Data.invenItems[i] = MyInventory.GetInvenItem(i).name;
        }
    }

    private void SetSaveMirrors()
    {
        MirrorManager mm = FindObjectOfType<MirrorManager>();
        Data.isBroken = new bool[mm.GetMirrorsCount()];

        for (int i = 0; i < Data.isBroken.Length; i++)
        {
            Data.isBroken[i] = mm.GetMirrorsIsBroken(i);
        }
    }
    #endregion
    #region Response Load Data
    private void LoadData(int chapterNum)
    {
        MyPlayer.transform.position = Data.playerPos;
        MyPlayer.transform.rotation = Data.playerRot;
        LoadInvenItems();
        LoadPuzzleItmes(chapterNum);
        LoadMirrorsData();
        LoadDoor();
        HospitalTrigger ht = FindObjectOfType<HospitalTrigger>();
        ht.LoadHospital();
        GetPlayer().CanLight = Data.CanLight;
    }

    private void LoadInvenItems()
    {
        for (int i = 0; i < Data.invenItems.Length; i++)
        {
            GameObject item = GameObject.Find(Data.invenItems[i]);
            if (item != null)
                MyInventory.InsertItem(item, item.GetComponent<Interactable>().GetInvenScale());
        }
    }

    private void LoadPuzzleItmes(int chapterNum)
    {
        for (int i = 0; i < Data.puzzles.Length; i++)
        {
            if (Data.puzzles[i].isCleared == true)
            {
                GameObject obj = _Puzzle.FindClearPuzzleObj(chapterNum, Data.puzzles[i].puzzleName);
                _Puzzle.GetCurrentPuzzle().chapterPuzzleDatas[i].isCleared = true;
                Interactable inter = null;
                if (obj.TryGetComponent<Interactable>(out inter) == true)
                {
                    inter.SetSpecial(false);
                    inter.NonInteractable();
                }
                obj.SendMessage("InsertItemInventory", SendMessageOptions.RequireReceiver);
            }
        }
    }

    private void LoadMirrorsData()
    {
        MirrorManager mm = FindObjectOfType<MirrorManager>();
        for (int i = 0; i < mm.GetMirrorsCount(); i++)
        {
            if (Data.isBroken[i] == true)
                mm.SetMirrorsIsBroken(i, Data.isBroken[i]);
        }
    }

    private void LoadDoor()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        for (int i = 0; i < doors.Length; i++)
        {
            if (Data.isDoorOpen[i].isOpened == true)
                doors[i].GetComponent<Interactable>().IsLocked = false;
            else
                doors[i].GetComponent<Interactable>().IsLocked = true;
        }
    }
    #endregion


    #endregion
    #region IdentityManagement
    private IdentitiesManager _IdentityManager = null;
    public void InitIdentityManager() => _IdentityManager = GetComponent<IdentitiesManager>();
    public IdentitiesManager GetIdentityManager() => _IdentityManager;

    #endregion
    #region CaptureManagement
    private CaptureManager _captureManager = null;
    public void InitCaptureManager() => _captureManager = GetComponent<CaptureManager>();
    public CaptureManager GetCaptureManager() => _captureManager;

    #endregion



    /// <summary>
    /// Toggle cursor lock state.
    /// </summary>
    /// <param name="value">Is cursor locked?</param>
    public void ToggleCursor(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
