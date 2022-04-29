using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{


    Animator _animator;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void JumpAnimation(bool isJumping)
    {
        if (isJumping == _animator.GetBool("isJumping")) return;

        _animator.SetBool("isJumping", isJumping);
    }
}
