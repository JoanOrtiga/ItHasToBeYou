using UnityEngine;


[CreateAssetMenu(fileName = "HeadBobData", menuName = "FirstPersonController/HeadBobData")]
public class HeadBobData : ScriptableObject
{
    #region Variables

    public AnimationCurve xCurve;
    public AnimationCurve yCurve;

    [Space] public float xAmplitude;
    public float yAmplitude;

    [Space] public float xFrequency;
    public float yFrequency;

    [Space] public float runAmplitudeMultiplier;
    public float runFrequencyMultiplier;

    [Space] public float crouchAmplitudeMultiplier;
    public float crouchFrequencyMultiplier;

    #endregion

    #region Properties

    public float MoveBackwardsFrequencyMultiplier { get; set; }
    public float MoveSideFrequencyMultiplier { get; set; }

    #endregion
}