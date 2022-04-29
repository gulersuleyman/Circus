using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera firstCamera;
    [SerializeField] CinemachineVirtualCamera turnCamera;
    [SerializeField] CinemachineVirtualCamera trailCam;


    RouteSwitch _player;
    PlayerCollision _playerCollision;
    
    private void Awake()
    {
        _playerCollision = FindObjectOfType<PlayerCollision>();
        _player = FindObjectOfType<RouteSwitch>();
    }
    void OnEnable()
    {
        CinemachineSwitcher.Register(firstCamera);
        PlayerCollision.Instance.OnAction += ChangeCamera;
        PlayerCollision.Instance.OnActionEnded += ReturnToMainCamera;
    }
    private void OnDisable()
    {
        CinemachineSwitcher.Unregister(firstCamera);
        PlayerCollision.Instance.OnAction -= ChangeCamera;
        PlayerCollision.Instance.OnActionEnded -= ReturnToMainCamera;
    }


   
    void ChangeCamera(int cameraIndex)
    {
        if(cameraIndex==0)
        {
                ChangeToSelectedCamera(turnCamera);
                _player._follower.followSpeed = 290f;
        }


        if(cameraIndex==1)
        {
                ChangeToSelectedCamera(trailCam);
                _player._follower.followSpeed = 290f;
        }


        
    }
    void ReturnToMainCamera()
    {
        CinemachineSwitcher.SwitchCamera(firstCamera);
        _player._follower.followSpeed = _playerCollision.playerFirstSpeed;
        turnCamera.gameObject.SetActive(false);
        trailCam.gameObject.SetActive(false);
       
    }
    void ChangeToSelectedCamera(CinemachineVirtualCamera camera)
    {
        camera.gameObject.SetActive(true);
        camera.Priority = firstCamera.Priority + 1;
    }
}
