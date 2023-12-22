using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private uint cameraSpeed = 5;
    [SerializeField] private uint cameraRotationSpeed = 2;
    private Rigidbody _rigidbody;
    private float _horizontalAxis;
    private float _verticalAxis;
    private float _yAxisRotation;
    private bool _movementAllowed = true;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_movementAllowed)
        {
            return;
        }

        _horizontalAxis = Input.GetAxis("Horizontal");
        _verticalAxis = Input.GetAxis("Vertical");
        if (Input.GetMouseButton(0))
        {
            float h = cameraRotationSpeed * Input.GetAxis("Mouse X");
            transform.Rotate(0, h, 0);
        }

        _yAxisRotation = (float)(Math.PI * transform.rotation.eulerAngles.y) / 180;
    }

    private void FixedUpdate()
    {
        var xRotated = (float)(_horizontalAxis * Math.Cos(_yAxisRotation) + _verticalAxis * Math.Sin(_yAxisRotation));
        var zRotated = (float)(-_horizontalAxis * Math.Sin(_yAxisRotation) + _verticalAxis * Math.Cos(_yAxisRotation));

        _rigidbody.velocity = new Vector3(xRotated, 0, zRotated).normalized * cameraSpeed;
    }

    public void DisallowPlayerMovement()
    {
        _movementAllowed = false;
    }

    public void AllowPlayerMovement()
    {
        _movementAllowed = true;
    }

    public IEnumerator RotatePlayer(float maxAngle, float duration)
    {
        var initialRotationY = transform.eulerAngles.y;
        var targetPositiveRotationY = initialRotationY + maxAngle;
        var targetNegativeRotationY = initialRotationY - maxAngle;
        var currentRotationY = initialRotationY;
        var rotatingCCW = false;

        var rotationSpeed = 15f; // Degrees per second

        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            var continueRotation = rotatingCCW && currentRotationY < targetPositiveRotationY
                                   || !rotatingCCW && currentRotationY > targetNegativeRotationY;
            if (!continueRotation)
            {
                rotatingCCW = !rotatingCCW;
                continue;
            }

            var rotationDirection = rotatingCCW ? 1 : -1;
            var rotationStep = rotationDirection * rotationSpeed * Time.deltaTime;
            currentRotationY += rotationStep;

            var eulerAngles = transform.eulerAngles;
            eulerAngles = new Vector3(eulerAngles.x, currentRotationY, eulerAngles.z);
            transform.eulerAngles = eulerAngles;

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}