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
        EventManager.Instance.AddListener("OnHang", HangAnimation);
        EventManager.Instance.AddListener("OnFall", FallAnimation);
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
        EventManager.Instance.RemoveListener("OnHang", HangAnimation);
        EventManager.Instance.RemoveListener("OnFall", FallAnimation);
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

        _animatorTop.SetFloat("HangBlend", 0);
        _animatorBottom.SetFloat("HangBlend", 0);

        _animatorTop.SetTrigger("Jump");
        _animatorBottom.SetTrigger("Jump");

        _animatorTop.SetBool("Land", false);
        _animatorBottom.SetBool("Land", false);
    }

    void HangAnimation()
    {
        _animatorTop.SetBool("Hang", true);
        _animatorBottom.SetBool("Hang", true);
    }

    void FallAnimation()
    {
        _animatorTop.SetBool("Hang", false);
        _animatorBottom.SetBool("Hang", false);
    }

    void LandAnimation()
    {
        _animatorTop.SetBool("Land", true);
        _animatorBottom.SetBool("Land", true);
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

        _animatorTop.SetFloat("HangBlend", 1);
        _animatorBottom.SetFloat("HangBlend", 1);
    }

    void StopShootAnimation()
    {
        _animatorTop.SetFloat("JumpShoot", 0);
        _animatorBottom.SetFloat("JumpShoot", 0);

        StopGrabShooting();
    }

    void StopGrabShooting()
    {
        _animatorTop.SetFloat("HangBlend", 0);
        _animatorBottom.SetFloat("HangBlend", 0);
    }
}
