using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class AnimatorParameter
{
    [SerializeField] string _name;
    [SerializeField] ParameterType _type;
    public string Name => _name;

    public AnimatorControllerParameterType Type
    {
        get
        {
            var paraType = new AnimatorControllerParameterType();

            switch (_type)
            {
                case ParameterType.FLOAT:
                    paraType = AnimatorControllerParameterType.Float;
                    break;
                case ParameterType.BOOL:
                    paraType = AnimatorControllerParameterType.Bool;
                    break;
            }

            return paraType;
        }
    }

    public enum ParameterType
    {
        FLOAT,
        BOOL,
    }
}
