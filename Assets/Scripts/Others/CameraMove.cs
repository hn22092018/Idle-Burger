using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMove : MonoBehaviour {
    public static CameraMove instance;
    public Camera camera;
    public float BorderX_Left, BorderX_Right, BorderY_Top, BorderY_Bottom, BorderZ_Bottom, BorderZ_Top;
    public float DragSpeed = 2;
    private Vector3 DragOrigin;
    Vector3 move;
    Transform FollowTargetTrans;
    bool isFollowTarget;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.2F;
    bool _IsChangeCameraPos;
    Vector3 _TargetChangeCameraPos;
    UnityAction _CallbackChangeCameraPos;
    bool _IsForceInZoom, _IsForceOutZoom;
    float _ForceZoomTarget;
    private void Awake() {
        instance = this;
    }
    void Update() {
        ZoomCamera();
        if (isFollowTarget) {
            transform.position = Vector3.SmoothDamp(transform.position, FollowTargetTrans.position, ref velocity, smoothTime);
            return;
        }
        if (_IsChangeCameraPos) {
            transform.position = Vector3.SmoothDamp(transform.position, _TargetChangeCameraPos, ref velocity, smoothTime);
            if (Vector3.Distance(transform.position, _TargetChangeCameraPos) < 0.01f) {
                _IsChangeCameraPos = false;
                if (_CallbackChangeCameraPos != null) _CallbackChangeCameraPos();
            }
            return;
        }
        if (UIManager.instance.isHasPopupOnScene || Tutorials.instance.IsShow) return;
        DragCamera();
    }
    void ZoomCamera() {
        if (_IsForceInZoom) {
            if (camera.orthographicSize <= _ForceZoomTarget) {
                camera.orthographicSize += Time.deltaTime * 10;
            } else {
                camera.orthographicSize = _ForceZoomTarget;
                _IsForceInZoom = false;
            }
        }
        if (_IsForceOutZoom) {
            if (camera.orthographicSize >= _ForceZoomTarget) {
                camera.orthographicSize -= Time.deltaTime * 10;
            } else {
                camera.orthographicSize = _ForceZoomTarget;
                _IsForceOutZoom = false;
            }
        }
        if (Input.touchCount == 2 && !Tutorials.instance.IsShow) {
            Touch touch_0 = Input.GetTouch(0);
            Touch touch_1 = Input.GetTouch(1);
            Vector2 touch_0_PrevPos = touch_0.position - touch_0.deltaPosition;
            Vector2 touch_1_PrevPos = touch_1.position - touch_1.deltaPosition;
            float prevMagnitude = (touch_0_PrevPos - touch_1_PrevPos).magnitude;
            float currentMagnitude = (touch_0.position - touch_1.position).magnitude;
            float diffenrence = currentMagnitude - prevMagnitude;
            Zoom(diffenrence * 0.01f);
        }
        if (Input.GetKey(KeyCode.KeypadPlus)) {
            Zoom(0.1f);
        }
        if (Input.GetKey(KeyCode.KeypadMinus)) {
            Zoom(-0.1f);
        }
    }
    void Zoom(float increment) {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, 10, 30);
    }
    void DragCamera() {
        if (Input.GetMouseButtonDown(0)) {
            DragOrigin = Input.mousePosition;
        }
        if (Input.GetMouseButton(0)) {
            move = Camera.main.ScreenToViewportPoint(Input.mousePosition - DragOrigin) * DragSpeed;
            if (Vector3.Distance(Camera.main.ScreenToWorldPoint(DragOrigin), Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.1f) return;
            DragOrigin = Input.mousePosition;
            move.z = 0;
            transform.Translate(-move);
            if (transform.position.x <= BorderX_Left) transform.position = new Vector3(BorderX_Left, transform.position.y, transform.position.z);
            if (transform.position.x >= BorderX_Right) transform.position = new Vector3(BorderX_Right, transform.position.y, transform.position.z);
            if (transform.position.y <= BorderY_Bottom) transform.position = new Vector3(transform.position.x, BorderY_Bottom, transform.position.z);
            if (transform.position.y >= BorderY_Top) transform.position = new Vector3(transform.position.x, BorderY_Top, transform.position.z);
            if (transform.position.z >= BorderZ_Top) transform.position = new Vector3(transform.position.x, transform.position.y, BorderZ_Top);
            if (transform.position.z <= BorderZ_Bottom) transform.position = new Vector3(transform.position.x, transform.position.y, BorderZ_Bottom);
        }
    }
    public void ChangePosition(Vector3 pos, UnityAction callback = null, int size=20) {
        Camera.main.orthographicSize = size;
        _TargetChangeCameraPos = pos;
        _TargetChangeCameraPos.x = Mathf.Clamp(pos.x, BorderX_Left, BorderX_Right);
        _TargetChangeCameraPos.y = Mathf.Clamp(pos.y, BorderY_Bottom, BorderY_Top);
        _TargetChangeCameraPos.z = Mathf.Clamp(pos.z, BorderZ_Bottom, BorderZ_Top);
        _IsChangeCameraPos = true;
        _CallbackChangeCameraPos = callback;
    }
    public void ZoomOutCamera(float zoom = 16f) {
        _IsForceOutZoom = true;
        _ForceZoomTarget = zoom;
    }
    public void ZoomInCamera(float zoom = 25f) {
        _IsForceInZoom = true;
        _ForceZoomTarget = zoom;
    }

    public void SetTargetFollow(Transform t) {
        ZoomOutCamera();
        isFollowTarget = true;
        FollowTargetTrans = t;
    }
    public void StopFollow() {
        ZoomInCamera();
        isFollowTarget = false;
    }
    public void Story1() {
        ZoomOutCamera(40);
        _TargetChangeCameraPos = new Vector3(-40, 30, -35);
        _IsChangeCameraPos = true;
    }
    public void StopFollowStory() {
        ZoomInCamera(20);
        isFollowTarget = false;
    }
}
