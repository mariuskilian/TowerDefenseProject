using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    [Header("Unity Stuff")]
    public Image healthBar;

    public float _health = 500;
    public float _maxHealth = 500;

    public Material material;

    private void Start()
    {
        material.SetColor("_Color", new Color(0, 1, 1, 0.25f));
    }

    void OnEnable()
    {
        GameStateManager.OnStateChange += StateChangeHandler;
    }

    void OnDisable()
    {
        GameStateManager.OnStateChange -= StateChangeHandler;
    }

    void StateChangeHandler(GameState newState)
    {
        switch (newState)
        {
            case GameState.POST_ROUND:
                material.SetColor("_Color", new Color(0, 1, 0, 0.25f));
                StartCoroutine(RegenerateShield());
                break;
            default:
                break;
        }
    }

    IEnumerator RegenerateShield()
    {
        while (_health != _maxHealth)
        {
            _health += _maxHealth / 10;
            TakeDamage(0);
            yield return new WaitForSeconds(0.1f);
        }
        material.SetColor("_Color", new Color(0, 1, 1, 0.25f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy Bullet")
        {
            Destroy(other.gameObject);
            Debug.Log("Shield Hit!");
            material.SetColor("_Color", new Color(1, 0, 0, 0.25f));
            StartCoroutine(colorChange());
            float damage = Difficulty.bullet_dmg;
            Debug.Log(damage);
            TakeDamage(damage);
        }
    }

    IEnumerator colorChange()
    {
        yield return new WaitForSeconds(0.3f);
        material.SetColor("_Color", new Color(0, 1, 1, 0.25f));
    }

    internal void TakeDamage(float amountOfDamage)
    {
        _health -= amountOfDamage;

        healthBar.fillAmount = _health / _maxHealth;

        if (_health <= 0)
        {//could place death animation here
            Die();
        }
    }
    void Die()
    {
        Destroy(transform.parent.gameObject);
    }
}
