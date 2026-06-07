using UnityEngine;

namespace GameAI.Lab04
{
    /// <summary>
    /// 5장 Utility AI: 효용 기반 의사결정 에이전트.
    ///
    /// 매 프레임 Patrol / Chase / Attack / Flee 네 행동의 점수(0~1)를 계산하고,
    /// 가장 높은 점수를 가진 행동을 선택(argmax)해 실행한다.
    /// 점수는 거리·체력·공격 쿨다운·위협 같은 Consideration을 Response Curve로
    /// 변환한 뒤 조합해서 만든다. (교재 5.7 / 그림 5-2, 5-3)
    /// </summary>
    public class UtilityAgent : MonoBehaviour
    {
        public enum ActionType { Patrol, Chase, Attack, Flee }

        [Header("참조")]
        [SerializeField] private Transform player;

        [Header("범위 설정")]
        [SerializeField] private float detectionRange = 12f; // 탐지 범위
        [SerializeField] private float attackRange    = 3f;  // 공격 범위

        [Header("이동")]
        [SerializeField] private float moveSpeed   = 4f;
        [SerializeField] private float rotateSpeed = 200f;

        [Header("상태(시뮬레이션)")]
        [Range(0f, 1f)] [SerializeField] private float health = 0.6f; // 체력 비율
        [SerializeField] private float attackCooldown = 1.2f;
        [SerializeField] private float threat = 0.2f;                 // 주변 위협(0~1)

        [Header("행동별 색상")]
        [SerializeField] private Color patrolColor = new Color(0.7f, 0.7f, 0.7f);
        [SerializeField] private Color chaseColor  = new Color(0.30f, 0.65f, 1f);
        [SerializeField] private Color attackColor = new Color(1f, 0.35f, 0.3f);
        [SerializeField] private Color fleeColor   = new Color(1f, 0.85f, 0.2f);

        // 내부 상태
        private readonly float[] _scores = new float[4];
        private ActionType _current = ActionType.Patrol;
        private float _cooldownTimer;
        private Vector3 _patrolCenter;
        private float _wanderAngle;
        private Renderer _renderer;
        private MaterialPropertyBlock _mpb;

        private void Awake()
        {
            _renderer = GetComponentInChildren<Renderer>();
            _mpb = new MaterialPropertyBlock();
            _patrolCenter = transform.position;
            if (player == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) player = p.transform;
            }
        }

        private void Update()
        {
            EvaluateScores();
            _current = ArgMax();
            Execute(_current);

            // 체력/쿨다운 시뮬레이션 (데모가 네 행동을 모두 보여주도록)
            _cooldownTimer -= Time.deltaTime;
            float dist = PlayerDistance();
            if (dist <= attackRange) health = Mathf.Max(0f, health - 0.12f * Time.deltaTime); // 근접 시 체력 감소
            else                     health = Mathf.Min(1f, health + 0.06f * Time.deltaTime); // 멀어지면 회복
            threat = 1f - Mathf.Clamp01(dist / detectionRange);
        }

        // ── 효용 점수 계산 ───────────────────────────────────────────
        private void EvaluateScores()
        {
            float dist     = PlayerDistance();
            float distNorm = Mathf.Clamp01(dist / detectionRange);          // 0=붙음, 1=탐지 끝
            float inAttack = 1f - Mathf.Clamp01(dist / attackRange);        // 1=공격범위 안
            float midBand  = 1f - Mathf.Abs(distNorm - 0.45f) * 2f;         // 중거리에서 최대
            bool  canAttack = _cooldownTimer <= 0f;

            // Patrol: 플레이어가 멀수록 / 위협이 없을수록 높음
            _scores[(int)ActionType.Patrol] =
                ResponseCurve.Evaluate(CurveType.Quadratic, distNorm) *
                ResponseCurve.Evaluate(CurveType.InverseLinear, threat);

            // Chase: 탐지 범위 안이지만 공격 범위 밖(중거리)에서 높음 + 체력이 너무 낮지 않을 때
            _scores[(int)ActionType.Chase] =
                Mathf.Clamp01(midBand) *
                ResponseCurve.Evaluate(CurveType.InverseLinear, inAttack) *
                ResponseCurve.Evaluate(CurveType.InverseQuadratic, health);

            // Attack: 플레이어가 가깝고(공격범위 안) 쿨다운이 끝났을 때 높음
            _scores[(int)ActionType.Attack] =
                ResponseCurve.Evaluate(CurveType.Logistic, inAttack) *
                (canAttack ? 1f : 0.2f) *
                ResponseCurve.Evaluate(CurveType.Linear, health);

            // Flee: 체력이 낮을수록 / 주변 위협이 클수록 높음
            _scores[(int)ActionType.Flee] =
                ResponseCurve.Evaluate(CurveType.Quadratic, 1f - health) *
                ResponseCurve.Evaluate(CurveType.InverseQuadratic, threat);
        }

        private ActionType ArgMax()
        {
            int best = 0;
            for (int i = 1; i < _scores.Length; i++)
                if (_scores[i] > _scores[best]) best = i;
            return (ActionType)best;
        }

        // ── 행동 실행 ────────────────────────────────────────────────
        private void Execute(ActionType action)
        {
            if (player == null && action != ActionType.Patrol)
            {
                DoPatrol(); SetColor(patrolColor); return;
            }
            switch (action)
            {
                case ActionType.Patrol: DoPatrol(); SetColor(patrolColor); break;
                case ActionType.Chase:  MoveToward(player.position); SetColor(chaseColor); break;
                case ActionType.Attack: FacePlayer(); DoAttack();  SetColor(attackColor); break;
                case ActionType.Flee:   MoveAwayFrom(player.position); SetColor(fleeColor); break;
            }
        }

        private void DoPatrol()
        {
            _wanderAngle += Time.deltaTime * 0.7f;
            Vector3 target = _patrolCenter + new Vector3(Mathf.Cos(_wanderAngle), 0f, Mathf.Sin(_wanderAngle)) * 4f;
            MoveToward(target);
        }

        private void DoAttack()
        {
            if (_cooldownTimer <= 0f)
            {
                _cooldownTimer = attackCooldown; // 쿨다운 동안에만 공격 수행
                Debug.Log("[UtilityAgent] Attack!");
            }
        }

        private void MoveToward(Vector3 pos)
        {
            Vector3 dir = pos - transform.position; dir.y = 0f;
            if (dir.sqrMagnitude < 0.01f) return;
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            FaceDir(dir);
        }

        private void MoveAwayFrom(Vector3 pos)
        {
            Vector3 dir = transform.position - pos; dir.y = 0f;
            if (dir.sqrMagnitude < 0.01f) return;
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            FaceDir(dir);
        }

        private void FacePlayer()
        {
            if (player == null) return;
            FaceDir(player.position - transform.position);
        }

        private void FaceDir(Vector3 dir)
        {
            dir.y = 0f;
            if (dir == Vector3.zero) return;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
        }

        private float PlayerDistance() =>
            player == null ? detectionRange : Vector3.Distance(transform.position, player.position);

        private void SetColor(Color c)
        {
            if (_renderer == null) return;
            _renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor("_BaseColor", c); // URP Lit
            _mpb.SetColor("_Color", c);     // Built-in Standard
            _renderer.SetPropertyBlock(_mpb);
        }

        // ── 화면 점수 표시 (디버그/캡쳐용) ──────────────────────────
        private static readonly string[] Names = { "Patrol", "Chase", "Attack", "Flee" };

        private void OnGUI()
        {
            const int w = 260, barW = 170, rowH = 26;
            int x = 16, y = 16;
            GUI.skin.label.richText = true;
            GUI.color = Color.white;
            GUI.Box(new Rect(x - 8, y - 8, w + 16, rowH * 4 + 56), GUIContent.none);
            GUI.Label(new Rect(x, y, w, 22), "Utility AI — 행동 점수(argmax)");
            y += 28;

            ActionType best = ArgMax();
            for (int i = 0; i < 4; i++)
            {
                bool sel = (int)best == i;
                GUI.color = sel ? new Color(0.2f, 0.8f, 0.3f) : new Color(0.6f, 0.75f, 1f);
                GUI.Box(new Rect(x + 70, y + 4, Mathf.Max(2f, barW * _scores[i]), 16), GUIContent.none);
                GUI.color = Color.white;
                GUI.Label(new Rect(x, y, 70, rowH), (sel ? "▶ " : "   ") + Names[i]);
                GUI.Label(new Rect(x + 70 + barW + 6, y, 60, rowH), _scores[i].ToString("0.00"));
                y += rowH;
            }
            GUI.Label(new Rect(x, y + 2, w, 22), $"체력 {health:0.00}  거리 {PlayerDistance():0.0}");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 1f, 0f, 0.12f); Gizmos.DrawSphere(transform.position, detectionRange);
            Gizmos.color = Color.yellow;                 Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = new Color(1f, 0f, 0f, 0.18f);  Gizmos.DrawSphere(transform.position, attackRange);
            Gizmos.color = Color.red;                    Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
