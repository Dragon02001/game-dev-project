using UnityEngine;
using Cinemachine;

public class Cam : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float pitchClampAngle = 80f;

    private float pitch = 0f;
    private float yaw = 0f;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineComposer composer;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, -pitchClampAngle, pitchClampAngle);

        composer.m_TrackedObjectOffset.y = pitch;
        transform.eulerAngles = new Vector3(0f, yaw, 0f);
    }
}
