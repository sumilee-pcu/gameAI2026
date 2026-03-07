using System.Text;
using UnityEngine;

/// <summary>
/// Lab10: Q-러닝 강화학습 에이전트 (ML-Agents 없이 직접 구현).
/// 4x4 그리드 환경에서 목표(+10)를 찾고 함정(-10)을 피하는 법을 학습합니다.
///
/// Q(s,a) ← Q(s,a) + α [ r + γ · max Q(s',a') − Q(s,a) ]
/// </summary>
public class QLearningAgent : MonoBehaviour
{
    // =========================================================================
    // 하이퍼파라미터
    // =========================================================================

    [Header("그리드 설정")]
    [SerializeField] private int gridWidth  = 4;
    [SerializeField] private int gridHeight = 4;

    [Header("학습 파라미터")]
    [SerializeField] private float learningRate   = 0.1f;  // α
    [SerializeField] private float discountFactor = 0.9f;  // γ
    [SerializeField] private float epsilon        = 0.9f;  // ε (탐험률)
    [SerializeField] private float epsilonDecay   = 0.995f;
    [SerializeField] private float minEpsilon      = 0.01f;
    [SerializeField] private int   maxEpisodes    = 5000;
    [SerializeField] private int   maxSteps       = 100;

    // =========================================================================
    // 환경 설정
    // =========================================================================

    private int[] _goalStates = { 15 };         // 목표: 오른쪽 하단 코너
    private int[] _trapStates = { 5, 9, 11 };   // 함정

    // =========================================================================
    // 내부 상태
    // =========================================================================

    private enum Action { Up = 0, Down = 1, Left = 2, Right = 3 }
    private const int ActionCount = 4;

    private float[,] _qTable;  // [state, action]
    private int      _stateCount;
    private int      _episode;
    private int      _currentState;
    private bool     _training;

    // =========================================================================
    // Unity 생명주기
    // =========================================================================

    private void Start()
    {
        _stateCount = gridWidth * gridHeight;
        _qTable = new float[_stateCount, ActionCount];
        _training = true;
        _episode = 0;

        Debug.Log($"[QLearning] 훈련 시작. 총 {maxEpisodes} 에피소드");
        StartCoroutine(TrainingLoop());
    }

    // =========================================================================
    // 훈련 루프
    // =========================================================================

    private System.Collections.IEnumerator TrainingLoop()
    {
        while (_episode < maxEpisodes)
        {
            yield return StartCoroutine(RunEpisode());
            _episode++;
            epsilon = Mathf.Max(minEpsilon, epsilon * epsilonDecay);

            if (_episode % 500 == 0)
                Debug.Log($"[QLearning] 에피소드 {_episode}, ε={epsilon:F3}");
        }

        _training = false;
        Debug.Log("[QLearning] 훈련 완료!");
        PrintOptimalPolicy();
    }

    private System.Collections.IEnumerator RunEpisode()
    {
        _currentState = 0; // 시작: 왼쪽 상단
        int steps = 0;

        while (steps < maxSteps)
        {
            int action = ChooseAction(_currentState);
            int nextState = TakeAction(_currentState, action);
            float reward = GetReward(nextState);

            UpdateQ(_currentState, action, reward, nextState);

            _currentState = nextState;
            steps++;

            // 목표나 함정에 도달하면 에피소드 종료
            if (IsTerminal(nextState)) break;
        }

        yield return null;
    }

    // =========================================================================
    // Q-Learning 핵심 메서드
    // =========================================================================

    /// <summary>ε-greedy 정책으로 액션을 선택합니다.</summary>
    private int ChooseAction(int state)
    {
        if (Random.value < epsilon)
            return Random.Range(0, ActionCount); // 탐험 (무작위)

        // 활용 (최대 Q값 액션 선택)
        int    bestAction = 0;
        float  bestQ      = float.MinValue;
        for (int a = 0; a < ActionCount; a++)
        {
            if (_qTable[state, a] > bestQ)
            {
                bestQ = _qTable[state, a];
                bestAction = a;
            }
        }
        return bestAction;
    }

    /// <summary>액션 실행 후 다음 상태를 반환합니다. 경계를 벗어나면 제자리.</summary>
    private int TakeAction(int state, int action)
    {
        int row = state / gridWidth;
        int col = state % gridWidth;

        switch ((Action)action)
        {
            case Action.Up:    row = Mathf.Max(0, row - 1);             break;
            case Action.Down:  row = Mathf.Min(gridHeight - 1, row + 1); break;
            case Action.Left:  col = Mathf.Max(0, col - 1);             break;
            case Action.Right: col = Mathf.Min(gridWidth - 1, col + 1); break;
        }
        return row * gridWidth + col;
    }

    /// <summary>보상 함수: 목표=+10, 함정=-10, 일반=-0.1</summary>
    private float GetReward(int state)
    {
        foreach (int g in _goalStates) if (state == g) return 10f;
        foreach (int t in _trapStates) if (state == t) return -10f;
        return -0.1f;
    }

    /// <summary>Q값 업데이트: Q(s,a) ← Q(s,a) + α[r + γ·maxQ(s',·) - Q(s,a)]</summary>
    private void UpdateQ(int state, int action, float reward, int nextState)
    {
        float maxNextQ = float.MinValue;
        for (int a = 0; a < ActionCount; a++)
            maxNextQ = Mathf.Max(maxNextQ, _qTable[nextState, a]);

        float tdTarget = reward + discountFactor * maxNextQ;
        float tdError  = tdTarget - _qTable[state, action];

        _qTable[state, action] += learningRate * tdError;
    }

    private bool IsTerminal(int state)
    {
        foreach (int g in _goalStates) if (state == g) return true;
        foreach (int t in _trapStates) if (state == t) return true;
        return false;
    }

    // =========================================================================
    // 결과 출력
    // =========================================================================

    private void PrintOptimalPolicy()
    {
        string[] arrows = { "↑", "↓", "←", "→" };
        var sb = new StringBuilder("\n[최적 정책]\n");

        for (int row = 0; row < gridHeight; row++)
        {
            for (int col = 0; col < gridWidth; col++)
            {
                int state = row * gridWidth + col;
                bool isGoal = System.Array.IndexOf(_goalStates, state) >= 0;
                bool isTrap = System.Array.IndexOf(_trapStates, state) >= 0;

                if (isGoal)      sb.Append(" G ");
                else if (isTrap) sb.Append(" X ");
                else
                {
                    int bestA = ChooseAction(state);
                    sb.Append($" {arrows[bestA]} ");
                }
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }

    // =========================================================================
    // 디버그 UI — Q-테이블 시각화
    // =========================================================================

    private void OnGUI()
    {
        if (_qTable == null) return;

        GUI.Label(new Rect(10, 10, 300, 20),
            $"에피소드: {_episode}/{maxEpisodes}  ε: {epsilon:F3}  {(_training ? "훈련 중..." : "완료")}");

        string[] arrows = { "↑", "↓", "←", "→" };
        int cellSize = 60;

        for (int row = 0; row < gridHeight; row++)
        for (int col = 0; col < gridWidth; col++)
        {
            int state = row * gridWidth + col;
            int x = 10 + col * cellSize;
            int y = 40 + row * cellSize;

            GUI.Box(new Rect(x, y, cellSize - 2, cellSize - 2), "");

            bool isGoal = System.Array.IndexOf(_goalStates, state) >= 0;
            bool isTrap = System.Array.IndexOf(_trapStates, state) >= 0;
            string label = isGoal ? "GOAL" : isTrap ? "TRAP" : arrows[ChooseAction(state)];

            GUI.Label(new Rect(x + 5, y + 20, cellSize - 10, 20), label);
        }
    }
}
