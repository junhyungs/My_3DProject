using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHitObject : MonoBehaviour, IDamged
{
    [SerializeField]
    private int HP = 9999;

    [SerializeField]
    private Material TestMaterial;

    Color savecolor;
    public void TakeDamage(float damage)
    {
        HP -= (int)damage;


        StartCoroutine(IntensityChange(2f,2f));
    }
    protected IEnumerator IntensityChange(float powValue1, float powValue2)
    {
        Color color = TestMaterial.GetColor("_Color");
        Color newColor = color * Mathf.Pow(powValue1, powValue2);
        TestMaterial.SetColor("_Color", newColor);

        yield return new WaitForSeconds(0.1f);

        TestMaterial.SetColor("_Color", savecolor);
    }

    // Start is called before the first frame update
    void Start()
    {
        savecolor = TestMaterial.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
