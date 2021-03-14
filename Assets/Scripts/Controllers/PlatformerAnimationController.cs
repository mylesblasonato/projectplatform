using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerAnimationController : MonoBehaviour
{
    [SerializeField] SoFloat _inputDirection;
    [SerializeField] Animator _animatorTop, _animatorBottom;

    private void Start()
    {
        EventManager.Instance.GetComponent<EventManager>()._OnIdle += IdleAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnWalk += WalkAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnRun += RunAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnJump += JumpAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnLand += LandAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnCrouch += CrouchAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnStand += StandAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnShoot += ShootAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnStopShoot += StopShootAnimation;
    }
  
    private void OnDisable()
    {
        EventManager.Instance.GetComponent<EventManager>()._OnIdle -= IdleAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnWalk -= WalkAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnRun -= RunAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnJump -= JumpAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnLand -= LandAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnCrouch -= CrouchAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnStand -= StandAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnShoot -= ShootAnimation;
        EventManager.Instance.GetComponent<EventManager>()._OnStopShoot += StopShootAnimation;
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
