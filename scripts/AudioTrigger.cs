using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class AudioTrigger : MonoBehaviour
{

    [SerializeField] private uint numberOfAudiosTriggered = 1;
    [SerializeField] private Light directionalLight;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var totalDuration = Vocals.Instance.Say(AudioObjectSelector.SelectAudioObjects(numberOfAudiosTriggered));
            CameraController cameraController = other.GetComponent<CameraController>();
            if (cameraController)
            {
                cameraController.DisallowPlayerMovement();
                StartCoroutine(cameraController.RotatePlayer(90, totalDuration));
                cameraController.AllowPlayerMovement();
            }

            if (directionalLight)
            {
                var lightController = directionalLight.GetComponent<LightController>();
                StartCoroutine(lightController.DimLight(totalDuration));
            }
        }
    }
}
