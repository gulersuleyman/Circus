using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;
public class EnemyFront : MonoBehaviour
{

    public bool canJumpFront;

    public SplineFollower _follower;


    private void Start()
    {
        _follower = GetComponent<SplineFollower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Start"))
        {
            canJumpFront = true;
            _follower.spline = other.gameObject.GetComponentInParent<SplineComputer>();
            _follower.follow = true;
            _follower.followSpeed = 20;
            
        }
    }
}
