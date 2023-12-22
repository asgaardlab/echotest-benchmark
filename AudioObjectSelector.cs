using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AudioObjectSelector
{
    /// <summary>
    /// Loads audio objects from the Assets folder and returns a list of randomly selected audio objects
    /// </summary>
    /// <param name="numAudioObjects">Number of audio objects to select (default: 1)</param>
    /// <returns>A list of randomly selected Audio Objects</returns>
    public static List<AudioObject> SelectAudioObjects(uint numAudioObjects = 1)
    {
        var audioObjects = new List<AudioObject>(Resources.LoadAll<AudioObject>("AudioObjects"));

        List<AudioObject> selectedAudioObjects = new List<AudioObject>();

        while (selectedAudioObjects.Count < numAudioObjects)
        {
            if (audioObjects.Count == 0)
            {
                Debug.LogWarning("No audio objects left.");
                break;
            }

            int randomIndex = Random.Range(0, audioObjects.Count);
            var audioObject = audioObjects[randomIndex];
            var subtitles = Subtitle.Instance;
            var subtitleObjectName = subtitles.subtitleObject.name;
            var fileName = Recorder.Instance.MediaOutputFolder.FullName + "/" + audioObject.name + "-" + subtitleObjectName + ".mp4";
            if (!File.Exists(fileName))
            {
                selectedAudioObjects.Add(audioObject);
            }

            audioObjects.RemoveAt(randomIndex);
        }

        return selectedAudioObjects;
    }
}