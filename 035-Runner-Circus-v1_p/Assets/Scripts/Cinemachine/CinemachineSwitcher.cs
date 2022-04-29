using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Cinemachine;

public static class CinemachineSwitcher
{
    public static CinemachineVirtualCamera activeCamera = null;
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();


    public static bool IsActiveCamera(CinemachineVirtualCamera camera)
    {
        return camera == activeCamera;
    }

    public static void SwitchCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;
        activeCamera = camera;

        foreach (CinemachineVirtualCamera c in cameras)
        {
            if(c!=camera && c.Priority!=0)
            {
                c.Priority = 0;
            }
        }
    }

    public static void Register(CinemachineVirtualCamera camera)
    {
        cameras.Add(camera);
    }
    public static void Unregister(CinemachineVirtualCamera camera)
    {
        cameras.Remove(camera);
    }
}
