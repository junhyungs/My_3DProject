using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSortingOrder : MonoBehaviour
{
    private void Start()
    {
        var renderer = GetComponent<Renderer>();

        Debug.Log(renderer.sortingOrder);
    }
}
