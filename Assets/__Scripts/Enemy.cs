using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected static Vector3[] directions = new Vector3[] { 
Vector3.right, Vector3.up, Vector3.left, Vector3.down };

    [Header("Set in Inspector: Enemy")] // b
    public float maxHealth = 1; // c
    public float knockbackSpeed = 10;
    public float knockbackDuration = 0.25f;
    public float invincibleDuration = 0.5f;
    public GameObject[] randomItemDrops;
    public GameObject guaranteedltemDrop = null;



    [Header("Set Dynamically: Enemy")]
    public float health; // c
    protected Animator anim; // c
    protected Rigidbody rigid; // c
    protected SpriteRenderer sRend; // c
    public bool invincible = false;
    public bool knockback = false;

    private float invincibleDone = 0;
    private float knockbackDone = 0;
    private Vector3 knockbackVel;

    protected virtual void Awake()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        sRend = GetComponent<SpriteRenderer>();
    }
    protected virtual void Update()
    { // b
      // ��������� ��������� ������������ � ������������� ��������� ������
        if (invincible && Time.time > invincibleDone) invincible = false;
        sRend.color = invincible ? Color.red : Color.white;
        if (knockback)
        {
            rigid.velocity = knockbackVel;
            if (Time.time < knockbackDone) return;
        }
        anim.speed = 1; // c
        knockback = false;
    }
    void OnTriggerEnter(Collider colld)
    { // d
        if (invincible) return; // �����, ���� ���� ���� ��������
        DamageEffect dEf = colld.gameObject.GetComponent<DamageEffect>();
        if (dEf == null) return; // ���� ��������� DamageEffect ����������� - �����
        health -= dEf.damage; // ������� �������� ������ �� ������ ��������
        if (health <= 0)
        {
                Die();
        }
        invincible = true; // ������� ���� ����������
        invincibleDone = Time.time + invincibleDuration;
        if (dEf.knockback)
        { // ��������� ������������
          // ���������� ����������� �������
            Vector3 delta = transform.position - colld.transform.root.position;
            if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
            {
                // ������������ �� �����������
                delta.x = (delta.x > 0) ? 1 : -1;
                delta.y = 0;
            }
            else
            {
                // ������������ �� ���������
                delta.x = 0;
                delta.y = (delta.y > 0) ? 1 : -1;
            }
            // ��������� �������� ������������ � ���������� Rigidbody
            knockbackVel = delta * knockbackSpeed;
            rigid.velocity = knockbackVel;
            // ���������� ����� knockback � ����� ����������� ������������
            knockback = true;
            knockbackDone = Time.time + knockbackDuration;
            anim.speed = 0;
        }
    }
    void Die()
    { // f
        GameObject go;
        if (guaranteedltemDrop != null)
        {
            go = Instantiate<GameObject>(guaranteedltemDrop);
            go.transform.position = transform.position;
        }
        else if (randomItemDrops.Length > 0)
        { // b
            int n = Random.Range(0, randomItemDrops.Length);
            GameObject prefab = randomItemDrops[n];
            if (prefab != null)
            {
                go = Instantiate<GameObject>(prefab);
                go.transform.position = transform.position;
            }
        }
        Destroy(gameObject);
    }
}
