using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for sound effects
/// </summary>
public class SoundScript : MonoBehaviour
{
    // sound for closing the ads
    [SerializeField] AudioClip SfxClose;
    [SerializeField] AudioSource SrcClose;

    // sound for the ad being downloaded
    [SerializeField] AudioClip SfxDownload;
    [SerializeField] AudioSource SrcDownload;

    // sound for the download error
    [SerializeField] AudioClip SfxError;
    [SerializeField] AudioSource SrcError;

    [SerializeField] AudioClip SfxFix;
    [SerializeField] AudioSource SrcFix;

    [SerializeField] AudioClip SfxGoodDownload;
    [SerializeField] AudioSource SrcGoodDownload;

    // method that plays the sound of closing the ad
    public void PlayClick()
    {
        SrcClose.clip = SfxClose;
        SrcClose.Play();
    }

    // method that plays the sound of downloading the ad
    public void PlayDownload()
    {
        SrcDownload.clip = SfxDownload;
        SrcDownload.Play();
    }

    public void PlayError()
    {
        SrcError.clip = SfxError;
        SrcError.Play();
    }

    public void PlayFix()
    {
        SrcFix.clip = SfxFix;
        SrcFix.Play();
    }

    public void PlayGoodDownload()
    {
        SrcGoodDownload.clip = SfxGoodDownload;
        SrcGoodDownload.Play();
    }

    public void StopGoodDownload()
    {
        SrcGoodDownload.clip = SfxGoodDownload;
        SrcGoodDownload.Stop();
    }


}
