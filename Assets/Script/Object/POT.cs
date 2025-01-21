using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POT : MonoBehaviour, IDamaged
{
    private List<POT_Cell> _childList;
    private HashSet<POT_Cell> _cells;

    private float _health = 1f;
    private bool _isReady;

    private void Awake()
    {
        _cells = new HashSet<POT_Cell>();

        _childList = new List<POT_Cell>();  

        foreach(Transform child in transform)
        {
            _childList.Add(child.GetComponent<POT_Cell>());
        }
    }

    public void TakeDamage(float damage)
    {
        if(_health <= 0f)
        {
            return;
        }

        _health -= damage;

        foreach (var cell in _childList)
        {
            cell.SetLayer("Cell");
            cell.SetKinematic(false);
        }

        StartCoroutine(ReturnTransform());
    }

    private IEnumerator ReturnTransform()
    {
        _isReady = false;

        yield return new WaitForSeconds(5f);

        foreach(var cell in _childList)
        {
            RegisterHashSet(cell);

            StartCoroutine(cell.MoveAndRotateTowardsCoroutine(this));
        }

        yield return new WaitUntil(() =>
        {
            return _isReady;
        });

        _health = 1f;
    }

    private void RegisterHashSet(POT_Cell cell)
    {
        if (!_cells.Contains(cell))
        {
            _cells.Add(cell);
        }
    }

    public void UnRegisterHashSet(POT_Cell cell)
    {
        if (_cells.Contains(cell))
        {
            _cells.Remove(cell);

            if( _cells.Count == 0)
            {
                _isReady = true;
            }
        }
    }
}
