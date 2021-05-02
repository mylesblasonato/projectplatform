using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class PlatformerAnimatorController : MonoSingleton<PlatformerAnimatorController>
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] Animator _animator;
    [SerializeField] List<AnimatorParameter> _animatorParameters;
    #endregion

    public void AddParameters()
    {
        var animatorController = (AnimatorController)_animator.runtimeAnimatorController;
        AnimatorControllerParameter parameter = new AnimatorControllerParameter();
        foreach (AnimatorParameter para in _animatorParameters)
        {
            parameter.type = para.Type;
            parameter.name = para.Name;
            animatorController.AddParameter(parameter);
        }
    }

    public void RemoveParameters()
    {
        var animatorController = (AnimatorController)_animator.runtimeAnimatorController;
        for (int i = 0; i <= animatorController.parameters.Length - 1; ++i)
        {
                animatorController.RemoveParameter(i);
        }
    }

    public void SetFloat(string parameterName, float value)
    {
        _animator.SetFloat(parameterName, value);
    }

    public void SetBool(string parameterName, bool value)
    {
        _animator.SetBool(parameterName, value);
    }

    #region UNITY
    #endregion

    #region HELPERS
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        //Gizmos.DrawRay(new Vector2(transform.localPosition.x, transform.localPosition.y), new Vector2(0, _groundCheckDistance));
    }
    #endregion
}
