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
        if (animatorController.parameters.Length > 0)
        {
            foreach (AnimatorParameter para in _animatorParameters)
            {
                foreach (AnimatorControllerParameter animPara in animatorController.parameters)
                {
                    if (string.Compare(para.Name, animPara.name) != 0)
                    {
                        //AnimatorControllerParameter parameter = new AnimatorControllerParameter();
                        //parameter.type = para.Type;
                        //parameter.name = para.Name;
                        //animatorController.AddParameter(parameter);
                    }
                }
            }
        }
        else
        {
            AnimatorControllerParameter parameter = new AnimatorControllerParameter();
            foreach (AnimatorParameter para in _animatorParameters)
            {
                parameter.type = para.Type;
                parameter.name = para.Name;
                animatorController.AddParameter(parameter);
            }
        }
    }


    void Play(string stateName)
    {
        _animator.Play(stateName);
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
