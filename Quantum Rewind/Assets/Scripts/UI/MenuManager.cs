using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject[] menus;

    #region Settings

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;
    [Header("Images")]
    public Image musicImg;
    public Image sfxImg;
    [Header("Sprites")]
    public Sprite musicOn;
    public Sprite musicOff;
    public Sprite sfxOn;
    public Sprite sfxOff;
    #endregion

    AudioManager audioM;

    void Start()
    {
        audioM = AudioManager.Instance;

        UpdateValues();
    }

    public void ChangeMenu(GameObject to)
    {
        for (int i = 0; i < menus.Length; i++)
            menus[i].SetActive(false);
        to.SetActive(true);

        audioM.PlaySFX("Click_01");
    }

    public void onClick_Play()
    {
        audioM.PlaySFX("Click_01");
        SceneManager.LoadScene(1);
    }

    #region Music
    public void onClick_Music()
    {
        if (audioM.isMusicMuted)
        {
            audioM.SetMusicVolume(1f);
            musicSlider.value = 1f;
        }
        else
        {
            audioM.SetMusicVolume(0f);
            musicSlider.value = 0f;
        }
        audioM.PlaySFX("Click_01");
    }

    public void onChange_MusicSlider()
    {
        audioM.SetMusicVolume(musicSlider.value);

        if (musicSlider.value > 0f)
            musicImg.sprite = musicOn;
        else
            musicImg.sprite = musicOff;
    }
    #endregion

    #region SFX
    public void onClick_SFX()
    {
        if (audioM.isSfxMuted)
        {
            audioM.SetSFXVolume(1f);
            sfxSlider.value = 1f;
        }
        else
        {
            audioM.SetSFXVolume(0f);
            sfxSlider.value = 0f;
        }
        audioM.PlaySFX("Click_01");
    }

    public void onChange_SFXSlider()
    {
        audioM.SetSFXVolume(sfxSlider.value);

        if (sfxSlider.value > 0f)
            sfxImg.sprite = sfxOn;
        else
            sfxImg.sprite = sfxOff;
    }
    #endregion

    void UpdateValues()
    {
        musicSlider.value = audioM.musicVolume;
        sfxSlider.value = audioM.sfxVolume;
    }
}
