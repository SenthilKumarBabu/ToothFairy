using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageSequencePlayer : MonoBehaviour
{
    public AudioSource musicBgSource;
    public SoSongList[] songList;
    public SoFrameList[] frameLists;
    public Button[] fairyButtons;
    private int _songIndex;
    public TMP_Dropdown musicBgDropdown;
    public TMP_Text currentSongText;
    public Button leftButton, rightButton;
    private SoFrameList _currentFrameList;
    public float frameRate = 12f;
    public SpriteRenderer spriteRenderer,arObjectSpriteRenderer;
    public Button recordButton;
    public Button musicSetupButton, placeToothButton;
    public GameObject mainPage,musicPage,placeObjectPage,resultPage;

    [SerializeField] private ExampleScreenRecorder recorder;
    [SerializeField] private ArObjectPlacer arObjectPlacer;

    public void Start()
    {
        for (int i = 0; i < fairyButtons.Length; i++)
        {
            var j = i;
            fairyButtons[i].onClick.AddListener(()=> OnFairyButtonClicked(j));
        }

        OnFairyButtonClicked(1);
        recordButton.onClick.AddListener(PlaySequence);
        musicSetupButton.onClick.AddListener(MusicButtonClicked);
        placeToothButton.onClick.AddListener(PlaceToothButtonClicked);
        musicBgDropdown.value = 1;
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

    private void MusicButtonClicked()
    {
        mainPage.SetActive(false);
        musicPage.SetActive(true);
    }

    private void PlaceToothButtonClicked()
    {
        mainPage.SetActive(false);
        placeObjectPage.SetActive(true);
        arObjectPlacer.TogglePlaneDetection(true);
    }

    public void MusicPageBackButtonClicked()
    {
        mainPage.SetActive(true);
        musicPage.SetActive(false);
    }

    public void PlaceObjectPageBackButtonClicked()
    {
        placeObjectPage.SetActive(false);
        mainPage.SetActive(true);
        arObjectPlacer.TogglePlaneDetection(false);
    }

    public void OnLanguageDropdownValueChanged(int value)
    {
        DisplaySongName();
    }

    public void NextButtonClicked(int offset)
    {
        _songIndex += offset;
        DisplaySongName();
    }

    private void DisplaySongName()
    {
        leftButton.gameObject.SetActive(_songIndex != 0);
        rightButton.gameObject.SetActive(musicBgDropdown.value == 0 || _songIndex != songList[musicBgDropdown.value].songList.Count - 1);

        if (musicBgDropdown.value == 0)
        {
            currentSongText.text = "";
        }
        else
        {
            var clipData = songList[musicBgDropdown.value].songList[_songIndex];
            currentSongText.text = clipData.name;
        }
    }

    private async void PlaySequence()
    {
        musicBgSource.clip = songList[musicBgDropdown.value].songList[_songIndex].clip;
        DisplaySongName();
        musicBgSource.Play();
        //CopyFrom(spriteRenderer.transform,arObjectSpriteRenderer.transform);
        spriteRenderer.gameObject.SetActive(true);
        arObjectSpriteRenderer.gameObject.SetActive(false);
        mainPage.gameObject.SetActive(false);
        recorder.StartRecording();

        int delay = Mathf.RoundToInt(1000f / frameRate);

        foreach (var t in _currentFrameList.frameList)
        {
            spriteRenderer.sprite = t;
            await Task.Delay(delay);
        }
        
        recorder.StopRecording();
        spriteRenderer.gameObject.SetActive(false);
        resultPage.SetActive(true);
        musicBgSource.Stop();
    }
}