using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerCollision : MonoBehaviour
{
    


    RouteSwitch _player;

    
    public float playerFirstSpeed;

    public static PlayerCollision Instance { get; private set; }


    public event System.Action<int> OnAction;
    public event System.Action OnActionEnded;
    private void Awake()
    {
        SingletonThisGameObject();
        _player = GetComponentInParent<RouteSwitch>();
        
    }
    private void Start()
    {
        playerFirstSpeed = _player._follower.followSpeed;
        
    }
    public void SingletonThisGameObject()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }



    private void OnTriggerEnter(Collider other)
     {
         if(other.gameObject.CompareTag("Turn"))
         {
            OnAction?.Invoke(0);
         }
         if(other.gameObject.CompareTag("TurnEnd"))
         {
            OnActionEnded?.Invoke();
         }
         
     }
   
}
