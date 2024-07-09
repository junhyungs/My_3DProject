using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class SpawnMonster : MonoBehaviour
{
    [Header("Key")]
    [SerializeField] private int Key;

    [Header("SecondKey")]
    [SerializeField] private int SecondKey;

    [Header("SpwanPosition")]
    [SerializeField] private List<GameObject> SpawnPositionObjects;

    [Header("DoorObject")]
    [SerializeField] private GameObject[] DoorObject;

    [Header("SecondGimikObject")]
    [SerializeField] private GameObject SecondGimikObject;

    private SphereCollider m_triggerCollider;
    private Material[] m_doorMaterials;

    private void Start()
    {
        m_triggerCollider = GetComponent<SphereCollider>();

        m_doorMaterials = new Material[DoorObject.Length];

        for(int i = 0; i < m_doorMaterials.Length; i++)
        {
            MeshRenderer renderer = DoorObject[i].GetComponent<MeshRenderer>();

            if(renderer != null)
            {
                m_doorMaterials[i] = renderer.material;
            }
        }
    }


    private IEnumerator StartGimik()
    {
        m_triggerCollider.enabled = false;

        StartCoroutine(OpenDoor());

        var gimik = GimikManager.Instance.Gimik;

        if(gimik.TryGetValue(Key, out Action<GameObject> gimikEvent))
        {
            foreach (var obj in SpawnPositionObjects)
            {
                gimikEvent.Invoke(obj);
            }
        }

        yield return new WaitForSeconds(2.0f);

        StartCoroutine(CloseDoor());

        while (true)
        {
            if (!GimikManager.Instance.OnSpawnGimik)
            {
                if (SecondGimikObject == null)
                    yield break;

                if(gimik.TryGetValue(SecondKey, out Action<GameObject> secondGimikEvent))
                {
                    secondGimikEvent.Invoke(SecondGimikObject);
                }
                
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator CloseDoor()
    {
        float timer = 2.5f;

        float colorAmount = 0.006f;

        float colorMaxValue = 0.5f;

        while(timer > 0f)
        {
            colorMaxValue -= colorAmount;

            for(int i = 0; i < m_doorMaterials.Length; i++)
            {
                m_doorMaterials[i].SetFloat("_Float", colorMaxValue);
            }

            yield return null;

            timer -= Time.deltaTime;
        }
    }

    private IEnumerator OpenDoor()
    {
        float timer = 2.5f;

        float colorAmount = 0.006f;

        float colorMaxValue = -0.3f;

        while (timer > 0f)
        {
            colorMaxValue += colorAmount;

            for (int i = 0; i < m_doorMaterials.Length; i++)
            {
                m_doorMaterials[i].SetFloat("_Float", colorMaxValue);
            }

            yield return null;

            timer -= Time.deltaTime;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(StartGimik());
        }
    }
}
