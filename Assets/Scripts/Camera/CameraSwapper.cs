using Unity.Cinemachine;
using UnityEngine;

public class CameraSwapper : MonoBehaviour
{
    public CinemachineCamera SwitchCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CameraManager.Instance.SwapCams(SwitchCamera);
    }
}
