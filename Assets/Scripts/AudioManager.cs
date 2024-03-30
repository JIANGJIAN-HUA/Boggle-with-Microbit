using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public string AudioName;
    bool isnotAddAudioSource = true;

    [Header("Audio Stuff")]
    public AudioSource Audio;
    public AudioClip SaveAudio;
    public string SoundPath;

    void GetAudioFilePath()
    {
        if (isnotAddAudioSource)
        {
            Audio = gameObject.AddComponent<AudioSource>();
            Audio.playOnAwake = false;
            isnotAddAudioSource = false;
        }

        AudioName = GetVoc.Voc[ViewVoc.index].English + ".mp3";

#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX
        SoundPath = "file://" + Application.streamingAssetsPath + "/Sound/";
#elif UNITY_ANDROID
        SoundPath = "jar:file://" + Application.dataPath + "!/assets/Sound/";
#elif UNITY_IOS
        SoundPath = "file://" + Application.dataPath + "/Raw/Sound/";
#endif

        StartCoroutine(LoadAudio());
    }
    IEnumerator LoadAudio()
    {
        WWW request = GetAudioFromFile(SoundPath,AudioName);
        yield return request;

        SaveAudio = request.GetAudioClip();
        SaveAudio.name = AudioName;

        PlayAudioFile();
    }

    WWW GetAudioFromFile(string path,string filename)
    {
        string audioToLoad = string.Format(path + "{0}",filename);
        WWW request = new WWW(audioToLoad);
        return request;
    }

    void PlayAudioFile()
    {
        Audio.clip = SaveAudio;
        Audio.Play();
    }

    public void PlayAudio()
    {
        GetAudioFilePath();
    }
}
