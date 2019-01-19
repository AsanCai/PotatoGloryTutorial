using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
public class PlayerAttack : MonoBehaviour {
    [Tooltip("导弹Prefab")]
    public Missile MissilePrefab;
    [Tooltip("导弹发射点")]
    public Transform ShootingPoint;
    [Tooltip("发射导弹的音效")]
    public AudioClip ShootEffect;
    [Tooltip("炸弹Prefab")]
    public Rigidbody2D BombPrefab;
    [Tooltip("炸弹的初始数量")]
    public int InitBombNumber = 4;
    [Tooltip("使用火箭筒抛射炸弹的力")]
    public float ProjectileBombForce = 1000f;


    private int m_CurrentBombNumber;

    // private Animator m_Animator;
    private PlayerController m_PlayerCtrl;

    private void Awake() {
        // 获取引用
        // m_Animator = GetComponent<Animator>();
        m_PlayerCtrl = GetComponent<PlayerController>();

        // 检查关键属性是否赋值
        if(MissilePrefab == null) {
            Debug.LogError("请设置MissilePrefab");
        }

        if(ShootingPoint == null) {
            Debug.LogError("请设置ShootingPoint");
        }

        if(BombPrefab == null) {
            Debug.LogError("请设置BombPrefab");
        }
    }

    private void Start() {
        m_CurrentBombNumber = InitBombNumber;
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1")) {
            // 发射导弹
            Fire();
        }

        if (Input.GetButtonDown("Fire2")) {
            // 放置炸弹
            LayBomb();
        }

        if (Input.GetButtonDown("Fire3")) {
            // 抛射炸弹
            ProjectileBomb();
        }
    }

    // 发射导弹
    private void Fire() {
        // // 播放射击动画
        // m_Animator.SetTrigger("Shoot");

        // 播放射击音效
        AudioSource.PlayClipAtPoint(ShootEffect, ShootingPoint.position);

        // 创建导弹
        Missile instance = Instantiate(MissilePrefab, ShootingPoint.position, Quaternion.identity) as Missile;

        // 如果角色跟导弹的朝向不一致，就翻转导弹
        if(m_PlayerCtrl.FacingRight ^ instance.FacingRight) {
            instance.Flip();
        }
    }

    // 放置炸弹
    private void LayBomb() {
        if(m_CurrentBombNumber <= 0) {
            return;
        }

        // 放置炸弹
        Instantiate(BombPrefab, this.transform.position, Quaternion.identity);

        // 减少炸弹数量
        m_CurrentBombNumber --;
    }

    // 抛射炸弹
    private void ProjectileBomb() {
        if(m_CurrentBombNumber <= 0) {
            return;
        }

        // 抛射炸弹
        Rigidbody2D body = Instantiate(BombPrefab, ShootingPoint.position, Quaternion.identity) as Rigidbody2D;
        if(m_PlayerCtrl.FacingRight) {
            body.AddForce(Vector2.right * ProjectileBombForce);
        } else {
            body.AddForce(Vector2.left * ProjectileBombForce);
        }

        // 减少炸弹数量
        m_CurrentBombNumber --;
    }
}