using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CharacterEvents : MonoBehaviour
{

    private void OnEnable()
    {
       RouteSwitch.OnJump += TurnAround;
    }

    private void OnDisable()
    {
        RouteSwitch.OnJump -= TurnAround;
    }

    void TurnAround()
    {
        this.transform.DORotate(new Vector3(-180, 0, 0), 0.5f).OnComplete(() =>
        {
            this.transform.DORotate(new Vector3(-360, 0, 0), 0.5f);
        });
        
    }
}
