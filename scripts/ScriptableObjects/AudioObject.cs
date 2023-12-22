using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Object", menuName = "Assets/New Audio Object")]
public class AudioObject : ScriptableObject
{
    [SerializeField] public AudioClip mainClip;
    public AudioClip[] audios;
    public string[] subtitles;
}