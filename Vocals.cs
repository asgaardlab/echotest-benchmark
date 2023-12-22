using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Vocals : MonoBehaviour
{
    public static Vocals Instance;
    private AudioSource _source;
    private IEnumerator _playing;
    public UnityEvent StopRecording;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _source = gameObject.AddComponent<AudioSource>();
    }

    public float Say(List<AudioObject> audioObjects)
    {
        if (_source.isPlaying)
        {
            _source.Stop();
            StopCoroutine(_playing);
        }

        _playing = PlayAudioObjectsCoroutine(audioObjects);
        StartCoroutine(_playing);
        return audioObjects.Aggregate(0, (float combinedAudioObjectLength, AudioObject audioObject) =>
            combinedAudioObjectLength + audioObject.audios.Aggregate(0,
                (float totalClipLength, AudioClip clip) => totalClipLength + clip.length)) + audioObjects.Count();
    }

    private IEnumerator PlayAudioObjectsCoroutine(List<AudioObject> audioObjects)
    {
        foreach (var audioObject in audioObjects)
        {
            yield return new WaitForSeconds(1);
            var subtitleObjectName = Subtitle.Instance.subtitleObject.name;
            Recorder.Instance.Record(audioObject.mainClip.name + "-" + subtitleObjectName);
            for (int i = 0; i < audioObject.audios.Length; i++)
            {
                _source.PlayOneShot(audioObject.audios[i]);
                Subtitle.Instance.SetSubtitle(audioObject.subtitles[i], audioObject.audios[i].length);

                yield return new WaitForSeconds(audioObject.audios[i].length);
            }

            Recorder.Instance.StopRecording();
        }
        Instance.StopRecording.Invoke();
    }
}