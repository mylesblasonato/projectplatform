using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerAnimationController : MonoBehaviour
{
    [SerializeField] SoFloat _inputDirection;
    [SerializeField] Animator _animatorTop, _animatorBottom;

    private void Start()
    {
        EventManager.Instance.AddListener("OnIdle", IdleAnimation);
        EventManager.Instance.AddListener("OnWalk", WalkAnimation);
        EventManager.Instance.AddListener("OnRun", RunAnimation);
        EventManager.Instance.AddListener("OnJump", JumpAnimation);
        EventManager.Instance.AddListener("OnLand", LandAnimation);
        EventManager.Instance.AddListener("OnCrouch", CrouchAnimation);
        EventManager.Instance.AddListener("OnStand", StandAnimation);
        EventManager.Instance.AddListener("OnShoot", ShootAnimation);
        EventManager.Instance.AddListener("OnStopShoot", StopShootAnimation);
    }
  
    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener("OnIdle", IdleAnimation);
        EventManager.Instance.RemoveListener("OnWalk", WalkAnimation);
        EventManager.Instance.RemoveListener("OnRun", RunAnimation);
        EventManager.Instance.RemoveListener("OnJump", JumpAnimation);
        EventManager.Instance.RemoveListener("OnLand", LandAnimation);
        EventManager.Instance.RemoveListener("OnCrouch", CrouchAnimation);
        EventManager.Instance.RemoveListener("OnStand", StandAnimation);
        EventManager.Instance.RemoveListener("OnShoot", ShootAnimation);
        EventManager.Instance.RemoveListener("OnStopShoot", StopShootAnimation);
    }

    void IdleAnimation()
    {
        _animatorTop.SetFloat("IdleToWalk", Mathf.Abs(0));
        _animatorBottom.SetFloat("IdleToWalk", Mathf.Abs(0));
    }

    void WalkAnimation()
    {
        _animatorTop.SetFloat("IdleToWalk", Mathf.Abs(Mathf.Clamp(_inputDirection.Value, -0.5f, 0.5f)));
        _animatorBottom.SetFloat("IdleToWalk", Mathf.Abs(Mathf.Clamp(_inputDirection.Value, -0.5f, 0.5f)));
    }

    void RunAnimation()
    {
        _animatorTop.SetFloat("IdleToWalk", Mathf.Abs(_inputDirection.Value));
        _animatorBottom.SetFloat("IdleToWalk", Mathf.Abs(_inputDirection.Value));
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
