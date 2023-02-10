using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class FrontBoxCheck : MonoBehaviour 
{
    [Header("コンテキストメニューからセットしたコライダーにサイズを合わせられる")]
    [SerializeField] private Collider2D _checkCollider;
    [SerializeField] private Transform _checkTransform;
    [SerializeField] private Vector2 _boxSize = Vector2.one;
    [Tag] [SerializeField] private string _tagName;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _xOffSet = 0;
    [SerializeField] private float _yOffSet = 0;
    [SerializeField] private bool _reverseFront = false;
    [Header("ヒットしたオブジェクトを格納する配列の長さ(入り切らないオブジェクトは認識されない)")]
    [SerializeField] private int raycastHitLength = 25;
    [Header("DebugSettings")]
    [SerializeField] private Color _gizmoColor = new Color(Color.green.r, Color.green.g, Color.green.b, 0.2f);
    [SerializeField] private bool _drawGizmo = true;
    [SerializeField] private bool _debugMode = false;

    private RaycastHit2D[] _hit;

    private void Reset()
    {
        _checkCollider = GetComponent<Collider2D>();
        _checkTransform = transform;
    }

    [ContextMenu("セットしたコライダーにサイズを合わせる")]
    private void SetBoxSize()
    {
        if (_checkCollider != null)
        {
            float y = _checkCollider.bounds.size.y;
            _boxSize = new Vector2(_boxSize.x, y);
        }
    }

    private void Awake()
    {
        _hit = new RaycastHit2D[raycastHitLength];
    }

    private void OnDrawGizmosSelected()
    {
        if (_checkCollider != null && _drawGizmo && _checkTransform != null)
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawCube(GetFrontPos(), _boxSize);
        }
    }

    private Vector2 GetFrontPos()
    {
        if (_reverseFront)
        {
            if (Math.Sign(_checkTransform.localScale.x) < 0)
                return GerRightPos();
            else
                return GetLeftPos();
        }
        else
        {
            if (Math.Sign(_checkTransform.localScale.x) > 0)
                return GerRightPos();
            else
                return GetLeftPos();
        }
    }

    private Vector2 GerRightPos()
    {
        return _checkCollider.bounds.center
            + new Vector3(_xOffSet + _checkCollider.bounds.size.x / 2 + _boxSize.x / 2, _yOffSet, 0);
    }

    private Vector2 GetLeftPos()
    {
        return _checkCollider.bounds.center
            - new Vector3(_xOffSet + _checkCollider.bounds.size.x / 2 + _boxSize.x / 2, -_yOffSet, 0);
    }

    public bool FrontCheck()
    {
        int hitLength = Physics2D.BoxCastNonAlloc(GetFrontPos(), _boxSize, 0f, Vector2.zero, _hit, 1f, _layer);
        bool isContain = false;

        for (int i = 0; i < hitLength; i++)
        {
            if (_hit[i].collider.CompareTag(_tagName))
            {
                isContain = true;
                break;
            }
        }

        if (_debugMode)
        {
            Debug.Log($"FrontBoxCheck : {isContain}");
        }

        return isContain;
    }
}