using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    private Dictionary<int, Monster> MonsterDic = new Dictionary<int, Monster>();

    /*Dictionary<int, Monster> monsterDic = new Dictionary<int, Monster>();

    public void AddMonster(Monster monster)
    {
        int id = monster.GetComponent<Collider>().GetInstanceID();
        monsterDic.Add(id, monster);
    }

    public Monster GetMonster(int id)
    {
        if (monsterDic.TryGetValue(id, out Monster monster))
            return monster;
        else return null;
    }*/

    /*
    
    �����ʿ��� ȣ��
    private void OnEnable()
    {
    MonsterManager.Instance.AddMonster(this);
    }

    �÷��̾�� ���� �浹üũ�� ��
    public void OnTriggerEnter(Collider collider)
    {
        //collider.GetComponent<Monster>().OnDamage(damage);
        Monster monster = MonsterManager.Instance.GetMonster(collider.GetInstanceID());
    if(monster != null)
    monster.Ondamage();
    }
    */
}
