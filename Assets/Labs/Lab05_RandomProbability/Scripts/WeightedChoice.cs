using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Lab05
{
    /// <summary>
    /// 6장 Random/Probability: 가중치 기반 무작위 선택(Weighted Random).
    /// 총 가중치 합 안에서 난수를 뽑아, 가중치 비율대로 인덱스를 고른다.
    /// System.Random 을 쓰므로 시드(seed)를 주면 같은 순서가 재현된다. (6.7)
    /// </summary>
    public class WeightedChoice
    {
        private readonly System.Random _rng;

        public WeightedChoice(int seed)  { _rng = new System.Random(seed); }
        public WeightedChoice()          { _rng = new System.Random(); }

        /// <summary>가중치 비율에 따라 인덱스를 하나 고른다.</summary>
        public int Pick(IReadOnlyList<float> weights)
        {
            float total = 0f;
            for (int i = 0; i < weights.Count; i++) total += Mathf.Max(0f, weights[i]);
            if (total <= 0f) return 0;

            double r = _rng.NextDouble() * total;
            float acc = 0f;
            for (int i = 0; i < weights.Count; i++)
            {
                acc += Mathf.Max(0f, weights[i]);
                if (r < acc) return i;
            }
            return weights.Count - 1;
        }
    }
}
