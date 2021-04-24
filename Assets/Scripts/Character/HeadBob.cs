using UnityEngine;

public class HeadBob
{
    HeadBobData headBobData;

    float xScroll;
    float yScroll;

    bool resetted;

    public float currentStateHeight = 0f;

    private float moveBackwardsFrequencyMultiplier;
    private float moveSideFrequencyMultiplier;

    public Vector3 finalOffset;
    public bool Resetted => resetted;
    

    public HeadBob(HeadBobData _data, float _moveBackwardsMultiplier, float _moveSideMultiplier)
    {
        headBobData = _data;

        moveBackwardsFrequencyMultiplier = _moveBackwardsMultiplier;
        moveSideFrequencyMultiplier = _moveSideMultiplier;

        xScroll = 0f;
        yScroll = 0f;

        resetted = false;
        finalOffset = Vector3.zero;
    }

    public void ScrollHeadBob(Vector2 input)
    {
        resetted = false;
        
        float additionalMultiplier = input.y == -1 ? moveBackwardsFrequencyMultiplier : 1f;
        additionalMultiplier = input.x != 0 & input.y == 0 ? moveSideFrequencyMultiplier : additionalMultiplier;

        xScroll += Time.deltaTime * headBobData.xFrequency;
        yScroll += Time.deltaTime * headBobData.yFrequency;

        float _xValue;
        float _yValue;

        _xValue = headBobData.xCurve.Evaluate(xScroll);
        _yValue = headBobData.yCurve.Evaluate(yScroll);
        
    

        finalOffset.x = _xValue * headBobData.xAmplitude * additionalMultiplier;
        finalOffset.y = _yValue * headBobData.yAmplitude * additionalMultiplier;
    }

    public void ResetHeadBob()
    {
        resetted = true;

        xScroll = 0f;
        yScroll = 0f;

        finalOffset = Vector3.zero;
    }
}