using UnityEngine;

namespace GameAI.Lab04
{
    /// <summary>
    /// 5장 Utility AI: Response Curve(반응 곡선).
    /// 입력값(0~1)을 효용 점수(0~1)로 바꾸는 함수 모음.
    /// 같은 입력이라도 어떤 곡선을 쓰느냐에 따라 AI의 성격이 달라진다. (그림 5-1)
    /// </summary>
    public enum CurveType
    {
        Linear,           // 선형 증가
        InverseLinear,    // 선형 감소
        Quadratic,        // 급격한 증가(체감 곡선의 반대)
        InverseQuadratic, // 완만한 증가
        Logistic          // S자(로지스틱): 특정 구간에서만 급상승
    }

    public static class ResponseCurve
    {
        /// <summary>입력 x(0~1)를 곡선에 따라 점수(0~1)로 변환한다.</summary>
        public static float Evaluate(CurveType type, float x)
        {
            x = Mathf.Clamp01(x);
            switch (type)
            {
                case CurveType.Linear:           return x;
                case CurveType.InverseLinear:    return 1f - x;
                case CurveType.Quadratic:        return x * x;
                case CurveType.InverseQuadratic: return 1f - (1f - x) * (1f - x);
                case CurveType.Logistic:         return 1f / (1f + Mathf.Exp(-12f * (x - 0.5f)));
                default:                         return x;
            }
        }
    }
}
