using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{   
    [Header("Music")]
    public Image musicImage;
    public Sprite[] musicSprite;
    [Header("Sfx")]
    public Image sfxImage;
    public Sprite[] sfxSprite;
    [Header("Vibration")]
    public Image vibrationImage;
    public Sprite[] vibrationSprite;
    private int mId,sfId,vId;

    public void UpdateSettings()
    {
        UpdateMusicSprite(GameController.instance.musicIndex);
        UpdateSfxSprite(GameController.instance.sfxIndex);
       //UpdateVibrationSprite(GameController.instance.vibrationIndex);
    }

    public void UpdateMusicSetting()
    {
        mId=GameController.instance.musicIndex>0?0:1;
        GameController.instance.musicIndex=mId;
        SaveSystemHandler.instance.UpdateData();
        HandleAudio();
    }

    void UpdateMusicSprite(int id)
    {
        musicImage.sprite=musicSprite[id];
    }

    public void UpdateSfxSetting()
    {
        sfId=GameController.instance.sfxIndex>0?0:1;
        GameController.instance.sfxIndex=sfId;
        SaveSystemHandler.instance.UpdateData();
        HandleAudio();
    }

    void UpdateSfxSprite(int id)
    {
        sfxImage.sprite=sfxSprite[id];
    }

    public void UpdateVibrationSetting()
    {
        vId=GameController.instance.vibrationIndex>0?0:1;
        GameController.instance.vibrationIndex=vId;
        SaveSystemHandler.instance.UpdateData();
        HandleAudio();
    }

    void UpdateVibrationSprite(int id)
    {
        vibrationImage.sprite=vibrationSprite[id];
    }

    void HandleAudio()
    {
        if(GameAudioHandler.instance!=null)
        {
            GameAudioHandler.instance.PlayButtonClickSound();
        }
    }
}
