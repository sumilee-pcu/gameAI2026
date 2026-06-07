using UnityEngine;

namespace GameAI.Lab04
{
    /// <summary>
    /// 데모용 플레이어 타깃 이동기.
    /// 에이전트(중심) 주위를 돌면서 거리를 천천히 가깝게/멀게 변화시켜,
    /// UtilityAgent가 Patrol → Chase → Attack → Flee 를 모두 보여 주도록 한다.
    /// </summary>
    public class Lab04PlayerMover : MonoBehaviour
    {
        [SerializeField] private Transform center;     // 에이전트(없으면 원점)
        [SerializeField] private float angularSpeed = 50f;
        [SerializeField] private float minRadius = 1.5f;
        [SerializeField] private float maxRadius = 13f;
        [SerializeField] private float radiusSpeed = 1.5f;

        private float _angle;
        private float _t;

        private void Update()
        {
            Vector3 c = center != null ? center.position : Vector3.zero;
            _angle += angularSpeed * Time.deltaTime;
            _t     += radiusSpeed  * Time.deltaTime;
            float radius = Mathf.Lerp(minRadius, maxRadius, (Mathf.Sin(_t) + 1f) * 0.5f);
            float rad = _angle * Mathf.Deg2Rad;
            transform.position = c + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * radius;
        }
    }
}
