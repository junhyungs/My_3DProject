using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    private GameObject chain;

    [SerializeField]
    private Transform SpwanPosition;

    public float speed = 15f;

    void Start()
    {
        StartCoroutine(SpawnChainCoroutine());
    }

    IEnumerator SpawnChainCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.045f); // 0.1초 대기 후 실행

            // 현재 위치에 chain 오브젝트 생성
            GameObject chainpre = Instantiate(chain, transform.position, Quaternion.identity);
            chainpre.transform.rotation = Quaternion.Euler(90f, 0, -90f);
        }
    }
    public void Create()
    {
        for(int i = 0; i <100; i++)
        {
            GameObject chain = Instantiate(this.chain, SpwanPosition);
            chain.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
