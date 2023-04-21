using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseStaffMoveSystem : MonoBehaviour {
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public float m_MoveSpeed = 4;
    protected float defaultSpeed;
    private bool IsMoving;
    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        defaultSpeed = m_MoveSpeed;
        SetAISpeed(m_MoveSpeed);
    }
    public void MultiplySpeedAI(float value) {
        m_MoveSpeed = defaultSpeed * value;
        navMeshAgent.speed = m_MoveSpeed;
    }
    public void SetAISpeed(float speed) {
        navMeshAgent.speed = speed;
    }
    public void OnEnableAI() {
        navMeshAgent.enabled = true;
    }
    public void OnDisableAI() {
        navMeshAgent.enabled = false;
    }
    public void SetDestination(Vector3 target) {
        navMeshAgent.SetDestination(target);
    }
    public void OnEnableMove() {
        IsMoving = true;
        if (animator) animator.SetBool("IsMove", IsMoving);
    }
    public void OnDisableMove() {
        IsMoving = false;
        if (animator) animator.SetBool("IsMove", IsMoving);
    }
    public bool IsFinishMoveOnNavemesh() {
        if (!navMeshAgent.pathPending) {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
                    return true;
                }
            }
        }
        return false;
    }
}
