namespace Bau.Libraries.BauGame.Engine.Tools.MathTools.Curves;

public class CurveSegment(AbstractCurve curve, float duration, Easing.EasingFunctionsHelper.EasingType? type)
{
    public AbstractCurve Curve { get; } = curve;
    public float Duration { get; } = duration;
    public Easing.EasingFunctionsHelper.EasingType? Easing { get; } = type;
}
