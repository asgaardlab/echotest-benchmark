using System.IO;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Encoder;
using UnityEditor.Recorder.Input;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    private RecorderController _recorderController;
    private MovieRecorderSettings _recorderSettings;
    public readonly DirectoryInfo MediaOutputFolder;
    public static Recorder Instance;

    private FileInfo OutputFile
    {
        get
        {
            var fileName = $"{_recorderSettings.OutputFile}.mp4";
            return new FileInfo(fileName);
        }
    }

    private void Awake()
    {
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        _recorderController = new RecorderController(controllerSettings);
        
        _recorderSettings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        _recorderSettings.Enabled = true;
        
        _recorderSettings.EncoderSettings = new CoreEncoderSettings
        {
            EncodingQuality = CoreEncoderSettings.VideoEncodingQuality.High,
            Codec = CoreEncoderSettings.OutputCodec.MP4
        };
        _recorderSettings.CaptureAlpha = true;

        _recorderSettings.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = 1920,
            OutputHeight = 1080
        };
        
        RecorderOptions.VerboseMode = false;

        controllerSettings.AddRecorderSettings(_recorderSettings);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 60.0f;
        
        Instance = this;
    }

    public void Record(string outputFileName)
    {
        Instance._recorderSettings.OutputFile = MediaOutputFolder.FullName + "/" + outputFileName;
        Instance._recorderController.PrepareRecording();
        Instance._recorderController.StartRecording();
        Debug.Log($"Started recording for file {Instance.OutputFile.FullName}");
    }

    public void StopRecording()
    {
        Instance._recorderController.StopRecording();
    }
}