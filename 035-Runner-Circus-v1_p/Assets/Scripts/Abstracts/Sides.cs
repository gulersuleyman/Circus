using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
public abstract class Sides : MonoBehaviour
{

    [SerializeField] Transform firstTransform;

    public bool isFront=false;
    public bool isRight=false;
    public bool isLeft=false;


    public double targetPercent;

    public SplineProjector _projector;
    public SplineComputer _nextSpline;
    public SplineFollower _follower;
    public BoxCollider _collider;

    RouteSwitch _player;
    // Start is called before the first frame update
    void Start()
    {
        _projector = GetComponent<SplineProjector>();
        _follower = GetComponent<SplineFollower>();
        _player = FindObjectOfType<RouteSwitch>();
        _collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(_follower.result.percent >=0.98)
        {
            _projector.spline = _player._follower.spline;
            _follower.follow = false;
            _follower.spline = _player._follower.spline;
            isFront = false;
            isRight = false;
            isLeft = false;

            transform.position = firstTransform.position;
        }
    }


    public virtual void Swipe(Collider other)
    {
        _player.canJumpBetweenSplines = true;

        _nextSpline = other.gameObject.GetComponentInParent<SplineComputer>();
        _follower.spline = _nextSpline;
        _follower.follow = true;
        _follower.SetPercent(0.0);
        _follower.followSpeed = _player._follower.followSpeed;
        _projector.spline = _nextSpline;
        Destroy(other.gameObject);

    }
}
