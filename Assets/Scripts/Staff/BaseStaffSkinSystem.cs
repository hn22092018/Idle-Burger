using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseStaffSkinSystem : MonoBehaviour {
    public Animator m_Animator;
    public StaffID charaterType;
    public SkinnedMeshRenderer _WorkMeshRender;
    public Mesh _DefaultWorkMesh;
    public int _StaffIndex;
    BaseStaff baseStaff;
    private void Awake() {
        baseStaff = GetComponent<BaseStaff>();
        EventManager.AddListener(EventName.UpdateStaffSuit.ToString(), (x) => {
            if ((StaffID)x == charaterType) {
                Debug.Log("Update Skin " + charaterType);
                UpdateStaffSkin();
            }
        });
    }

    public void UpdateStaffSkin() {
        baseStaff._SkinUsing = ProfileManager.PlayerData.skinManager.GetSkinConfig(_StaffIndex, charaterType);
        if (_WorkMeshRender == null || _DefaultWorkMesh == null) return;
        Mesh toChangeMesh = ProfileManager.PlayerData.skinManager.GetMesh(charaterType, _StaffIndex);
        if (toChangeMesh == null) {
            _WorkMeshRender.sharedMesh = _DefaultWorkMesh;
        } else {
            _WorkMeshRender.sharedMesh = toChangeMesh;
        }
    }

    //[Button]
    //void LoadRef() {
    //    BaseStaff baseStaff = GetComponent<BaseStaff>();
    //    m_Animator = baseStaff.m_Animator;
    //    charaterType = baseStaff.charaterType;
    //    baseAvatar = baseStaff.baseAvatar;
    //    _WorkObj = baseStaff._WorkObj;
    //    _WorkMeshRender = baseStaff._WorkMeshRender;
    //    _DefaultWorkMesh = baseStaff._DefaultWorkMesh;
    //    navMeshAgent = baseStaff.navMeshAgent;
    //}
}
