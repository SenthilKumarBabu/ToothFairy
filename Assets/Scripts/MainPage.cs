using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour
{
    [Header("Song Details")]
    public List<LanguageData> languageDataList;
    public Button noSongButton;
    public Outline noSongButtonOutline;
    public Button leftButton, rightButton;
    private int _songIndex;
    private int _musicLanguageIndex;
    [SerializeField] private GameObject songNamePrefab;
    
    [Header("Fairy Details")]
    public Button[] fairyButtons;
    public SoFrameList[] frameLists;
    private SoFrameList _currentFrameList;
    
    [Header("Tooth Placement Details")]
    public SpriteRenderer toothSpriteRenderer;

    [Header("Flash Button")] public DemoScript flashLight;
    public Button flashButton;
    public Sprite flashOnSprite,flashOffSprite;
    private bool _flashStatus;
    
    public Button recordButton;
    public GameObject infoText;

    [Header("Page Details")] 
    [SerializeField] private RecordingPage recordingPage;
    [SerializeField] private ArObjectPlacer arObjectPlacer;

    private void Awake()
    {
        for (int i = 0; i < fairyButtons.Length; i++)
        {
            var j = i;
            fairyButtons[i].onClick.AddListener(()=> OnFairyButtonClicked(j));
        }
        
        for (int i = 0; i < languageDataList.Count; i++)
        {
            for (int j = 0; j < languageDataList[i].soSong.songList.Count; j++)
            {
                var insSongNameObj = Instantiate(songNamePrefab,languageDataList[i].scrollViewContent.transform);
                var insSongName = insSongNameObj.GetComponent<SongName>();
                var insSongButton = insSongNameObj.GetComponent<Button>();
                var j1 = j;
                insSongButton.onClick.AddListener(() =>
                {
                    _songIndex = j1;
                    DisplaySongName();
                });
                insSongName.Initialize(languageDataList[i].soSong.songList[j].name);
                languageDataList[i].insSongName.Add(insSongName);
            }
        }
        
        noSongButton.onClick.AddListener(() =>
        {
            _musicLanguageIndex = 3;
            DisplaySongName();
        });
        
        for (int i = 0; i < languageDataList.Count; i++)
        {
            var i1 = i;
            languageDataList[i].languageButton.onClick.AddListener(() =>
            {
                _musicLanguageIndex = i1;
                DisplaySongName();
            });
        }
        
        recordButton.onClick.AddListener(ShowRecordingPage);
        flashButton.onClick.AddListener(FlashButtonClicked);
        
        _flashStatus = false;
    }

    public async void OnEnable()
    {
        _musicLanguageIndex = 0;
        _songIndex = 0;

        OnFairyButtonClicked(0);
        toothSpriteRenderer.gameObject.SetActive(false);
        infoText.SetActive(true);
        recordButton.interactable = false;
        arObjectPlacer.TogglePlaneDetection(true);  
        
        await Task.Delay(1000);
        DisplaySongName();
    }
    
    private void OnFairyButtonClicked(int index)
    {
        for (int i = 0; i < fairyButtons.Length; i++)
        {
            fairyButtons[i].transform.GetChild(0).gameObject.SetActive(i == index);
        }
        _currentFrameList = frameLists[index];
    }

    private void ShowRecordingPage()
    {
        recordingPage.musicBgSource.clip = languageDataList[_musicLanguageIndex].soSong.songList[_songIndex].clip;
        recordingPage.ShowPage(_currentFrameList);
    }
    
    public void NextButtonClicked(int offset)
    {
        _songIndex += offset;
        DisplaySongName();
    }
    
    private void DisplaySongName()
    {
        if (_musicLanguageIndex == 3)
        {
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
            for (int j = 0; j < languageDataList.Count; j++)
            {
                languageDataList[j].scrollViewParent.SetActive(false);
            }

            noSongButtonOutline.enabled = true;
            return;
        }
        
        leftButton.gameObject.SetActive(_songIndex != 0);
        rightButton.gameObject.SetActive(_songIndex != languageDataList[_musicLanguageIndex].soSong.songList.Count - 1);

        for (int i = 0; i < languageDataList[_musicLanguageIndex].insSongName.Count; i++)
        {
            languageDataList[_musicLanguageIndex].insSongName[i].highlightBg.SetActive(i == _songIndex);
        }
        
        for (int j = 0; j < languageDataList.Count; j++)
        {
            languageDataList[j].scrollViewParent.SetActive(j == _musicLanguageIndex);
            languageDataList[j].languageButtonsOutline.enabled = j == _musicLanguageIndex;
        }
        
        noSongButtonOutline.enabled = false;
    }

    private void FlashButtonClicked()
    {
        _flashStatus = !_flashStatus;
        FlashFunctionality();
    }

    private void FlashFunctionality()
    {
        flashLight.Toggle();
        _flashStatus = !_flashStatus;
        flashButton.image.sprite = _flashStatus ? flashOnSprite : flashOffSprite;
    }

    public void ObjectPlaced()
    {
        //toothSpriteRenderer.gameObject.SetActive(true);
        recordButton.interactable = true;
        infoText.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class LanguageData
{
    public SoSongList soSong;
    public Button languageButton;
    public Outline languageButtonsOutline;
    public GameObject scrollViewParent;
    public GameObject scrollViewContent;
    public List<SongName> insSongName;
}
