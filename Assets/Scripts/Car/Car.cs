using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public Transform[] meshs;
    public List<Transform> carMoveTrans;
    int indexMove = 0;
    int speed = 8;
    bool isMoveToParkingSlot;
    bool isOutParking;
    List<Vector3> carMovePos = new List<Vector3>();
    List<Vector3> carOutMovePos = new List<Vector3>();
    float parkingTime = 10f;
    float deltaTimeParking;
    bool isRunning;
    public void InitCar(List<Transform> move) {
        for (int i = 0; i < meshs.Length; i++) {
            meshs[i].gameObject.SetActive(false);
        }
        if (meshs.Length > 0) meshs[Random.Range(0, meshs.Length)].gameObject.SetActive(true);
        deltaTimeParking = 0;
        carMoveTrans = move;
        isMoveToParkingSlot = false;
        isOutParking = false;
        carMovePos.Clear();
        for (int i = 0; i < carMoveTrans.Count; i++) {
            carMovePos.Add(carMoveTrans[i].position);
        }
        indexMove = 0;
        transform.position = carMovePos[0];
        isRunning = true;
    }
    private void Update() {
        if (!isRunning) return;
        if (!isOutParking) {
            TransformMoveToTarget(carMovePos[indexMove]);
            AIRotateToTarget(carMovePos[indexMove]);
            if (GetDistanceToTarget(carMovePos[indexMove]) == 0) {
                if (indexMove >= carMovePos.Count - 1) {
                    if (!isMoveToParkingSlot) {
                        isRunning = false;
                        PoolManager.Pools["GameEntity"].Despawn(transform);
                    } else {
                        deltaTimeParking += Time.deltaTime;
                        if (deltaTimeParking >= parkingTime) {
                            deltaTimeParking = 0;
                            OurParking();
                        }
                    }
                } else {
                    indexMove++;
                    indexMove = Mathf.Clamp(indexMove, 0, carMovePos.Count - 1);
                }
            }
        } else {
            TransformMoveToTarget(carOutMovePos[indexMove]);
            AIRotateToTarget(carOutMovePos[indexMove]);
            if (GetDistanceToTarget(carOutMovePos[indexMove]) == 0) {
                if (indexMove >= carOutMovePos.Count - 1) {
                    isRunning = false;
                    PoolManager.Pools["GameEntity"].Despawn(transform);
                } else {
                    indexMove++;
                    indexMove = Mathf.Clamp(indexMove, 0, carOutMovePos.Count - 1);
                }
            }
        }
    }
    float GetDistanceToTarget(Vector3 target) {
        return Vector3.Distance(transform.position, target);
    }
    void TransformMoveToTarget(Vector3 target, float changeSpeed = 1) {
        float moveSpeed = this.speed;
        float speed = moveSpeed * changeSpeed;
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
    }

    void AIRotateToTarget(Vector3 target) {
        Vector3 pos = new Vector3(target.x, transform.position.y, target.z) - transform.position;
        if (pos != Vector3.zero) {
            var q = Quaternion.LookRotation(new Vector3(target.x, transform.position.y, target.z) - transform.position);
            transform.rotation = q;
        }
    }

    public void OurParking() {
        indexMove = 0;
        isOutParking = true;
    }
}