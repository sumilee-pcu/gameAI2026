using UnityEngine;

namespace GameAI.Lab05
{
    /// <summary>
    /// 6장 Random/Probability: 가중치 기반 확률 선택 에이전트.
    ///
    /// 일정 주기마다 Patrol / Chase / Attack 을 가중치(50/30/20)에 따라 무작위로 고른다.
    /// - 시드(seed)를 고정하면 같은 선택 순서가 재현된다. (6.7 재현 가능한 랜덤)
    /// - 같은 행동이 연속으로 너무 많이 나오지 않도록 제한한다. (6.6 체감 공정성)
    /// 화면에 목표 가중치와 실제 누적 분포를 함께 표시해, 분포가 가중치로 수렴함을 보여 준다.
    /// </summary>
    public class ProbabilityAgent : MonoBehaviour
    {
        public enum ActionType { Patrol, Chase, Attack }

        [Header("가중치 (Weighted Random)")]
        [SerializeField] private float patrolWeight = 50f;
        [SerializeField] private float chaseWeight  = 30f;
        [SerializeField] private float attackWeight = 20f;

        [Header("선택 주기 / 반복 제한")]
        [SerializeField] private float selectInterval = 0.5f;
        [SerializeField] private int   maxRepeat = 3;       // 같은 행동 연속 허용 횟수

        [Header("재현 가능한 랜덤")]
        [SerializeField] private bool useSeed = true;
        [SerializeField] private int  seed = 12345;

        [Header("행동별 색상")]
        [SerializeField] private Color patrolColor = new Color(0.30f, 0.65f, 1f);
        [SerializeField] private Color chaseColor  = new Color(0.2f, 0.8f, 0.4f);
        [SerializeField] private Color attackColor = new Color(1f, 0.35f, 0.3f);

        private WeightedChoice _chooser;
        private readonly float[] _weights = new float[3];
        private readonly int[]   _counts  = new int[3];
        private int   _total;
        private float _timer;
        private ActionType _current;
        private int _repeatCount;

        private Renderer _renderer;
        private MaterialPropertyBlock _mpb;
        private Vector3 _home;
        private float _hop;

        private static readonly string[] Names = { "Patrol", "Chase", "Attack" };

        private void Awake()
        {
            _renderer = GetComponentInChildren<Renderer>();
            _mpb = new MaterialPropertyBlock();
            _home = transform.position;
            _chooser = useSeed ? new WeightedChoice(seed) : new WeightedChoice();
            _weights[0] = patrolWeight; _weights[1] = chaseWeight; _weights[2] = attackWeight;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = selectInterval;
                Select();
            }
            Animate();
        }

        private void Select()
        {
            int idx = _chooser.Pick(_weights);

            // 체감 공정성: 같은 행동이 maxRepeat 이상 연속되면 다시 뽑는다
            int guard = 0;
            while ((ActionType)idx == _current && _repeatCount >= maxRepeat && guard++ < 8)
                idx = _chooser.Pick(_weights);

            ActionType picked = (ActionType)idx;
            _repeatCount = (picked == _current) ? _repeatCount + 1 : 0;
            _current = picked;

            _counts[idx]++; _total++;
            _hop = 1f; // 선택 순간 살짝 튀어오름
        }

        private void Animate()
        {
            Color c = _current == ActionType.Patrol ? patrolColor
                    : _current == ActionType.Chase  ? chaseColor : attackColor;
            SetColor(c);

            _hop = Mathf.MoveTowards(_hop, 0f, Time.deltaTime * 3f);
            transform.position = _home + Vector3.up * (Mathf.Sin(_hop * Mathf.PI) * 0.6f);
            transform.Rotate(Vector3.up, 60f * Time.deltaTime, Space.World);
        }

        private void SetColor(Color c)
        {
            if (_renderer == null) return;
            _renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor("_BaseColor", c);
            _mpb.SetColor("_Color", c);
            _renderer.SetPropertyBlock(_mpb);
        }

        // ── 화면 표시: 목표 가중치 vs 실제 누적 분포 ────────────────
        private void OnGUI()
        {
            const int barW = 150, rowH = 24;
            int x = 16, y = 16;
            float wTotal = Mathf.Max(1f, _weights[0] + _weights[1] + _weights[2]);

            GUI.Box(new Rect(x - 8, y - 8, 360, rowH * 3 + 96), GUIContent.none);
            GUI.Label(new Rect(x, y, 360, 22), "Weighted Random — 가중치 vs 실제 분포");
            y += 26;
            GUI.Label(new Rect(x, y, 360, 20), $"seed={(useSeed ? seed.ToString() : "랜덤")}   총 선택 {_total}회");
            y += 24;

            for (int i = 0; i < 3; i++)
            {
                float targetPct = _weights[i] / wTotal;
                float actualPct = _total > 0 ? (float)_counts[i] / _total : 0f;

                GUI.color = Color.white;
                GUI.Label(new Rect(x, y, 60, rowH), (i == (int)_current ? "▶ " : "   ") + Names[i]);

                // 목표(연한) 막대
                GUI.color = new Color(0.6f, 0.75f, 1f, 0.5f);
                GUI.Box(new Rect(x + 60, y + 2, Mathf.Max(2f, barW * targetPct), 8), GUIContent.none);
                // 실제(진한) 막대
                GUI.color = new Color(0.2f, 0.8f, 0.4f);
                GUI.Box(new Rect(x + 60, y + 12, Mathf.Max(2f, barW * actualPct), 8), GUIContent.none);

                GUI.color = Color.white;
                GUI.Label(new Rect(x + 60 + barW + 8, y, 130, rowH),
                    $"목표 {targetPct * 100f:0}%  실제 {actualPct * 100f:0}%");
                y += rowH;
            }
            GUI.color = Color.white;
            GUI.Label(new Rect(x, y + 4, 360, 20), "연한=목표 가중치, 진한=실제 누적 (수렴 관찰)");
        }
    }
}
