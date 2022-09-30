using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Buff")]
    [SerializeField] private TextMeshProUGUI _buffText;
    [SerializeField]
    private string[] _buffTextString =
    {
        "화상마법",
        "중독마법",
        "저주마법",
        "맨주먹"
    };
    private BuffState _buff;
    public BuffState Buff {
        get => _buff; 
        private set 
        { 
            _buff = value;
            _buffText.text = _buffTextString[(int)value];
        } 
    }

    [Header("Attack")]
    [SerializeField] private float _damage = 10f;
    private LayerMask targetLayer;

    private PlayerInput _input;


    private void Awake()
    {
        _input = GetComponent<PlayerInput>();

        Buff = BuffState.None;

        targetLayer = LayerMask.NameToLayer("Enemey");
    }

    private void Update()
    {
        if(_input.Q)
        {
            Buff = BuffState.Fire;
        }
        else if(_input.W)
        {
            Buff = BuffState.Poision;
        }
        else if(_input.E)
        {
            Buff = BuffState.Curse;
        }
        else if(_input.R)
        {
            Buff = BuffState.None;
        }

        if(_input.Click)
        {
            Attack();
        }
    }

    private void Attack()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray.origin, ray.direction, out hit, 20000f, targetLayer))
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            Debug.Assert(enemy, "Enemy에 Enemy가 없음");
            enemy.TakeDamage(Buff, _damage);
        }
    }
}
