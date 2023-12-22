using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(AudioObject))]
public class AudioObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AudioObject audioObject = (AudioObject)target;

        if (audioObject.mainClip == null) return;

        string audioClipName = audioObject.mainClip.name;

        AudioObjectGenerator.AssignAudioObjectChildren(audioClipName, audioObject);

        audioObject.name = audioClipName;
        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(audioObject), audioClipName);
    }
}