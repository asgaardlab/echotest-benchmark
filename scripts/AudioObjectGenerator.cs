using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class AudioObjectGenerator
{
    public static void GenerateAudioObject(string wavFilePath)
    {
        if (wavFilePath.Split("-")[1] != "1.wav")
        {
            Debug.Log("Audio Clip is not the parent");
            return;
        }
        
        AudioClip mainClip = LoadAudioClip(wavFilePath);

        if (mainClip == null)
        {
            Debug.LogError("Failed to load main clip.");
            return;
        }

        AudioObject audioObject = ScriptableObject.CreateInstance<AudioObject>();
        audioObject.mainClip = mainClip;

        var assetPath = $"Assets/{mainClip.name}.asset";
        if (AssetDatabase.GUIDToAssetPath(assetPath) != "")
        {
            Debug.Log("Asset already exists");
            return;
        }

        if (AssignAudioObjectChildren(mainClip.name, audioObject))
        {
            var destinationPath = $"Assets/Resources/AudioObjects/{mainClip.name}.asset";
            AssetDatabase.CreateAsset(audioObject, destinationPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"AudioObject created at: {destinationPath}");
        }
        else
        {
            Debug.LogError("Error assigning audio object subtitles or audios");
        }
    }

    public static bool AssignAudioObjectChildren(string mainClipFileName, AudioObject audioObject)
    {
        var baseName = mainClipFileName.Split("-")[0];
        audioObject.audios = FindAudioClips(baseName);
        audioObject.subtitles = FindSubtitles(baseName);
        return audioObject.subtitles.Length == audioObject.audios.Length && audioObject.audios.Length != 0;
    }

    private static AudioClip LoadAudioClip(string filePath)
    {
        return AssetDatabase.LoadAssetAtPath<AudioClip>(filePath);
    }
    
    private static AudioClip[] FindAudioClips(string baseName)
    {
        var audioClips = new List<AudioClip>();
        var clipNumber = 1;

        while (true)
        {
            var clipPath = $"Assets/AudioClips/{baseName}-{clipNumber}.wav";
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(clipPath);
            if (clip != null) audioClips.Add(clip);
            else break;
            clipNumber++;
        }

        return audioClips.ToArray();
    }

    private static string[] FindSubtitles(string baseName)
    {
        var subtitles = new List<string>();
        var subtitleNumber = 1;

        while (true)
        {
            var subtitlePath = $"Assets/AudioClips/{baseName}-{subtitleNumber}.txt";

            if (File.Exists(subtitlePath))
            {
                var subtitleText = File.ReadAllText(subtitlePath);
                subtitles.Add(subtitleText);
            }
            else break; 
            subtitleNumber++;
        }

        return subtitles.ToArray();
    }
}