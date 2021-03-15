using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerAnimationController : MonoBehaviour
{
    [SerializeField] SoFloat _inputDirection;
    [SerializeField] Animator _animatorTop, _animatorBottom;

    private void Start()
    {
        EventManager.Instance._OnIdle += IdleAnimation;
        EventManager.Instance._OnWalk += WalkAnimation;
        EventManager.Instance._OnRun += RunAnimation;
        EventManager.Instance._OnJump += JumpAnimation;
        EventManager.Instance._OnLand += LandAnimation;
        EventManager.Instance._OnCrouch += CrouchAnimation;
        EventManager.Instance._OnStand += StandAnimation;
        EventManager.Instance._OnShoot += ShootAnimation;
        EventManager.Instance._OnStopShoot += StopShootAnimation;
    }
  
    private void OnDestroy()
    {
        EventManager.Instance._OnIdle -= IdleAnimation;
        EventManager.Instance._OnWalk -= WalkAnimation;
        EventManager.Instance._OnRun -= RunAnimation;
        EventManager.Instance._OnJump -= JumpAnimation;
        EventManager.Instance._OnLand -= LandAnimation;
        EventManager.Instance._OnCrouch -= CrouchAnimation;
        EventManager.Instance._OnStand -= StandAnimation;
        EventManager.Instance._OnShoot -= ShootAnimation;
        EventManager.Instance._OnStopShoot += StopShootAnimation;
    }

    void IdleAnimation()
    {
        _animatorTop.SetFloat("IdleToWalk", Mathf.Abs(0));
        _animatorBottom.SetFloat("IdleToWalk", Mathf.Abs(0));
    }

    void WalkAnimation()
    {
        _animatorTop.SetFloat("IdleToWalk", Mathf.Abs(_inputDirection.Value));
        _animatorBottom.SetFloat("IdleToWalk", Mathf.Abs(_inputDirection.Value));
    }

    void RunAnimation()
    {
        _animatorTop.SetFloat("IdleToWalk", Mathf.Abs(_inputDirection.Value * 2));
        _animatorBottom.SetFloat("IdleToWalk", Mathf.Abs(_inputDirection.Value * 2));
    }

    void JumpAnimation()
    {
        _animatorTop.SetFloat("JumpShoot", 0);
        _animatorBottom.SetFloat("JumpShoot", 0);

        _animatorTop.SetTrigger("Jump");
        _animatorBottom.SetTrigger("Jump");
    }

    void LandAnimation()
    {
        _animatorTop.SetTrigger("Land");
        _animatorBottom.SetTrigger("Land");
    }

    void CrouchAnimation()
    {
        _animatorTop.SetBool("Crouch", true);
        _animatorBottom.SetBool("Crouch", true);
    }

    void StandAnimation()
    {
        _animatorTop.SetBool("Crouch", false);
        _animatorBottom.SetBool("Crouch", false);
    }

    void ShootAnimation()
    {
        _animatorTop.SetTrigger("Shoot");
        _animatorBottom.SetTrigger("Shoot");

        _animatorTop.SetFloat("JumpShoot", 1);
        _animatorBottom.SetFloat("JumpShoot", 1);
    }

    void StopShootAnimation()
    {
        _animatorTop.SetFloat("JumpShoot", 0);
        _animatorBottom.SetFloat("JumpShoot", 0);
    }
}
