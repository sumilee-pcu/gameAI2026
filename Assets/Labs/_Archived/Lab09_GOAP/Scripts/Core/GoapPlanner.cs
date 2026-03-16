using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lab09: GOAP 플래너 — 목표를 달성하기 위한 액션 시퀀스를 탐색합니다.
/// 재귀 DFS 트리 탐색으로 최소 비용 경로를 반환합니다.
/// </summary>
public static class GoapPlanner
{
    // =========================================================================
    // 내부 노드 클래스
    // =========================================================================

    private class PlanNode
    {
        public PlanNode parent;
        public float    runningCost;
        public Dictionary<string, bool> state;
        public GoapAction action;

        public PlanNode(PlanNode parent, float cost, Dictionary<string, bool> state, GoapAction action)
        {
            this.parent      = parent;
            this.runningCost = cost;
            this.state       = new Dictionary<string, bool>(state);
            this.action      = action;
        }
    }

    // =========================================================================
    // 공개 Plan 메서드
    // =========================================================================

    /// <summary>
    /// 주어진 세계 상태와 목표에 대해 실행 가능한 액션 큐를 반환합니다.
    /// 계획을 세울 수 없으면 null을 반환합니다.
    /// </summary>
    public static Queue<GoapAction> Plan(
        GameObject         agent,
        HashSet<GoapAction> availableActions,
        Dictionary<string, bool> worldState,
        Dictionary<string, bool> goal)
    {
        // 절차적 전제조건 검사로 가용 액션 필터링
        var usableActions = new HashSet<GoapAction>();
        foreach (GoapAction action in availableActions)
        {
            if (action.CheckProceduralPrecondition(agent))
                usableActions.Add(action);
        }

        // DFS 탐색
        var leaves = new List<PlanNode>();
        var start  = new PlanNode(null, 0f, worldState, null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("[GoapPlanner] 계획을 세울 수 없습니다.");
            return null;
        }

        // 가장 저렴한 계획 선택
        PlanNode cheapest = null;
        foreach (PlanNode leaf in leaves)
        {
            if (cheapest == null || leaf.runningCost < cheapest.runningCost)
                cheapest = leaf;
        }

        return FlattenPlan(cheapest);
    }

    // =========================================================================
    // 내부 탐색
    // =========================================================================

    private static bool BuildGraph(
        PlanNode            parent,
        List<PlanNode>      leaves,
        HashSet<GoapAction> usableActions,
        Dictionary<string, bool> goal)
    {
        bool foundPath = false;

        // 목표 달성 여부 확인
        if (IsGoalAchieved(goal, parent.state))
        {
            leaves.Add(parent);
            return true;
        }

        foreach (GoapAction action in usableActions)
        {
            // 전제조건 확인
            if (!action.ArePreconditionsMet(parent.state)) continue;

            // 이 액션을 적용한 새 상태 계산
            var newState = new Dictionary<string, bool>(parent.state);
            action.ApplyEffects(newState);

            var node = new PlanNode(parent, parent.runningCost + action.Cost, newState, action);

            // 재귀 탐색 (사용한 액션 제거하여 무한루프 방지)
            var subset = new HashSet<GoapAction>(usableActions);
            subset.Remove(action);

            if (BuildGraph(node, leaves, subset, goal))
                foundPath = true;
        }

        return foundPath;
    }

    private static bool IsGoalAchieved(
        Dictionary<string, bool> goal,
        Dictionary<string, bool> state)
    {
        foreach (var kv in goal)
        {
            if (!state.TryGetValue(kv.Key, out bool val) || val != kv.Value)
                return false;
        }
        return true;
    }

    private static Queue<GoapAction> FlattenPlan(PlanNode node)
    {
        var actions = new List<GoapAction>();
        PlanNode current = node;

        while (current != null)
        {
            if (current.action != null)
                actions.Add(current.action);
            current = current.parent;
        }

        actions.Reverse();
        return new Queue<GoapAction>(actions);
    }
}
