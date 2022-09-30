using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth { get; set; } = 100;
    private float _currentHealth;
    private EnemySpawaner _spawaner;
    private Material _material;

    private Color originalColor = new Color(1f, 1f, 1f);
    private Color purpleColor = new Color(0.5f, 0f, 1f);

    [Header("FireDamage")]
    [SerializeField] private float _fireDamage = 5;
    [SerializeField] private float _fireDamageDeltaTime = 5f;
    [SerializeField] private int _fireDamageCount = 4;
    private int _fireStack = 0;
    private int _fireDamageToTake = 0;

    [Header("PoisionDamage")]
    [SerializeField] private float _poisionDamage = 2;
    [SerializeField] private float _poisionDamageDeltaTime = 3f;
    [SerializeField] private int _poisionDamageCount = 2;

    [Header("Curse")]
    [SerializeField] private float _curseDieTime = 0.5f;

    private void Awake()
    {
        _spawaner = transform.parent.GetComponent<EnemySpawaner>();
        _material = GetComponent<Renderer>().material;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        _fireStack = 0;
        _fireDamageToTake = 0;
        _currentHealth = MaxHealth;
        transform.localScale = new Vector3(1f, 1f, 1f);
        _material.color = originalColor;
    }

    public void TakeDamage(BuffState buff, float damage)
    {
        switch(buff)
        {
            case BuffState.Fire:
                GetFireDamage();
                break;
            case BuffState.Poision:
                GetPoisionDamage();
                break;
            case BuffState.Curse:
                GetCurseDamage();
                break;
            case BuffState.None:
                GetDamage(damage);
                break;
            default:
                Debug.LogError("이상한 값이 들어옴");
                break;
        }
    }

    private void GetDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log($"{name} : {_currentHealth}");
        if (_currentHealth <= 0)
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }

    private void GetCurseDamage()
    {
        if (_currentHealth / MaxHealth <= 0.1f)
        {
            StopAllCoroutines();
            StartCoroutine(DieByCurse());
        }
    }

    private void GetFireDamage()
    {
        ++_fireStack;
        Debug.Log($"{name} : {_fireStack}");
        if(_fireStack == 1)
        {
            _fireDamageToTake += _fireDamageCount;
            StartCoroutine(FireDamageCoroutine());
        }
        else if(_fireStack <= 10)
        {
            ++_fireDamageToTake;
        }
        else
        {
            --_fireStack;
        }
    }

    private void GetPoisionDamage()
    {
        StartCoroutine(PoisionDamageCoroutine());
    }

    private IEnumerator PoisionDamageCoroutine()
    {
        for (int i = 0; i < _poisionDamageCount; ++i)
        {
            StartCoroutine(ChangeColor(purpleColor));
            GetDamage(_poisionDamage);
            yield return new WaitForSeconds(_poisionDamageDeltaTime);
        }
    }

    private IEnumerator FireDamageCoroutine()
    {

        while (_fireDamageToTake > 0)
        {
            StartCoroutine(ChangeColor(Color.red));
            GetDamage(_fireDamage * _fireStack);
            if(_fireDamageToTake > _fireDamageCount)
            {
                --_fireStack;
            }
            --_fireDamageToTake;
            yield return new WaitForSeconds(_fireDamageDeltaTime);
        }
    }

    private IEnumerator DieByCurse()
    {
        float elapsedTime = 0f;
        while(true)
        {
            elapsedTime += Time.deltaTime;
            float currentScale = Mathf.Lerp(1f, 0f, elapsedTime / _curseDieTime);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);

            if(elapsedTime >= _curseDieTime)
            {
                gameObject.SetActive(false);
                break;
            }

            yield return null;
        }
    }

    private IEnumerator ChangeColor(Color color)
    {
        _material.color = color;
        yield return new WaitForSeconds(0.1f);
        _material.color = originalColor;
    }

    private void OnDisable()
    {
        _spawaner.ReturnToPool(gameObject);
    }
}
