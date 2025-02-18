using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    public CinemachineCamera CurrentCamera;

    private void Awake()
    {
        Instance = this;
    }

    public void SwapCams( CinemachineCamera newCam)
    {
        newCam.Priority = 2;

        // change priority of old camera 
        if(CurrentCamera != null)
        {
            CurrentCamera.Priority = 1;
        }

        // update new cam to be the current camera
        CurrentCamera = newCam;

    }
}
