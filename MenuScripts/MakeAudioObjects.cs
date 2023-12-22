using System.IO;
using UnityEditor;
using UnityEngine;

public class MakeAudioObjects
{
    [MenuItem("Scripts/Create Audio Objects")]
    static void CreateAudioObjects()
    {
        var audioFiles = Directory.GetFiles("Assets/AudioClips/", "*.wav");
        foreach (var audioFile in audioFiles)
        {
            AudioObjectGenerator.GenerateAudioObject(audioFile);
        }
    }
}