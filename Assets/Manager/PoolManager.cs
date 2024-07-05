using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.Rendering;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [Header("Projectile")]
    [Header("ArrowPrefab")]
    [SerializeField] private GameObject m_arrowPrefab;
    [SerializeField] private int m_arrowCount;
    [SerializeField] private Transform m_arrowPoolPosition;
    private Queue<GameObject> ArrowPool = new Queue<GameObject>();

    [Header("FireBall")]
    [SerializeField] private GameObject m_fireBallPrefab;
    [SerializeField] private int m_fireBallCount;
    [SerializeField] private Transform m_fireBallPoolPosition;
    private Queue<GameObject> FireBallPool = new Queue<GameObject>();

    [Header("Hook")]
    [SerializeField] private GameObject m_hookPrefab;
    [SerializeField] private int m_hookCount;
    [SerializeField] private Transform m_hookPoolPosition;
    private Queue<GameObject> HookPool = new Queue<GameObject>();

    [Header("MageMonsterBullet")]
    [SerializeField] private GameObject m_MagicBullet;
    [SerializeField] private int m_MagicBulletCount;
    [SerializeField] private Transform m_MagicBulletPoolPosition;
    private Queue<GameObject> MagicBulletPool = new Queue<GameObject>();

    [Header("HitEffect")]
    [SerializeField] private GameObject m_HitParticle;
    [SerializeField] private int m_HitParticleCount;
    [SerializeField] private Transform m_HitParticlePoolPosition;
    private Queue<GameObject> HitParticlePool = new Queue<GameObject>();

    [Header("playerChainSegment")]
    [SerializeField] private GameObject m_playerSegment;
    [SerializeField] private int m_playerSegmentCount;
    [SerializeField] private Transform m_playerSegmentPoolPosition;
    private Queue<GameObject> PlayerSegmentPool = new Queue<GameObject>();

    [Header("OldCrowSegment")]
    [SerializeField] private GameObject m_oldCrowSegment;
    [SerializeField] private int m_oldCrowSegmentCount;
    [SerializeField] private Transform m_oldCrowPoolPosition;
    private Queue<GameObject> OldCrowSegmentPool = new Queue<GameObject>();

    [Header("Bomb")]
    [SerializeField] private GameObject m_BombPrefab;
    [SerializeField] private int m_BombPrefabCount;
    [SerializeField] private Transform m_BombPoolPosition;
    private Queue<GameObject>BombPool = new Queue<GameObject>();

    [Header("Deku")]
    [SerializeField] private GameObject m_DekuProjectileObject;
    [SerializeField] private int m_DekuProjectileObjectCount;
    [SerializeField] private Transform m_DekuProjectileObjectPoolPosition;
    private Queue<GameObject> DekuPool = new Queue<GameObject>();

    [Header("MonsterArrow")]
    [SerializeField] private GameObject m_MonsterArrowPrefab;
    [SerializeField] private int m_MonsterArrowPrefabCount;
    [SerializeField] private Transform m_MonsterArrowPoolPosition;
    private Queue<GameObject> MonsterArrowPool = new Queue<GameObject>();

    [Header("Monster")]
    [Header("Bat")]
    [SerializeField] private GameObject m_BatPrefab;
    [SerializeField] private int m_BatPrefabCount;
    [SerializeField] private Transform m_BatPoolPosition;
    private Queue<GameObject>BatPool = new Queue<GameObject>();

    [Header("Mage")]
    [SerializeField] private GameObject m_MagePrefab;
    [SerializeField] private int m_MagePrefabCount;
    [SerializeField] private Transform m_MagePoolPosition;
    private Queue<GameObject> MagePool = new Queue<GameObject>();

    [Header("Pot")]
    [SerializeField] private GameObject m_PotPrefab;
    [SerializeField] private int m_PotPrefabCount;
    [SerializeField] private Transform m_PotPoolPosition;
    private Queue<GameObject> PotPool = new Queue<GameObject>();

    [Header("Ghoul")]
    [SerializeField] private GameObject m_GhoulPrefab;
    [SerializeField] private int m_GhoulPrefabCount;
    [SerializeField] private Transform m_GhoulPoolPosition;
    private Queue<GameObject> GhoulPool = new Queue<GameObject>();

    [Header("DEKU_SCRUB")]
    [SerializeField] private GameObject m_DekuPrefab;
    [SerializeField] private int m_DekuPrefabCount;
    [SerializeField] private Transform m_DekuPoolPosition;
    private Queue<GameObject> DekuMonsterPool = new Queue<GameObject>();
    

    private void Awake()
    {
        CreateObject();
    }
   
    private void CreateObject()
    {
        for(int i = 0; i < m_arrowCount; i++)
        {
            GameObject arrow = Instantiate(m_arrowPrefab, m_arrowPoolPosition);
            arrow.SetActive(false);
            ArrowPool.Enqueue(arrow);
        }

        for(int i = 0; i < m_fireBallCount; i++)
        {
            GameObject fire = Instantiate(m_fireBallPrefab, m_fireBallPoolPosition);
            fire.SetActive(false);
            FireBallPool.Enqueue(fire);
        }

        for(int i = 0; i < m_MagicBulletCount; i++)
        {
            GameObject magicBullet = Instantiate(m_MagicBullet, m_MagicBulletPoolPosition);
            magicBullet.SetActive(false);
            MagicBulletPool.Enqueue(magicBullet);
        }

        for(int i = 0; i < m_HitParticleCount; i++)
        {
            GameObject hitParticle = Instantiate(m_HitParticle, m_HitParticlePoolPosition);
            hitParticle.SetActive(false);
            HitParticlePool.Enqueue(hitParticle);
        }

        for(int i = 0; i < m_playerSegmentCount; i++)
        {
            GameObject playerSegment = Instantiate(m_playerSegment, m_playerSegmentPoolPosition);
            playerSegment.SetActive(false);
            PlayerSegmentPool.Enqueue(playerSegment);
        }

        for(int i = 0; i < m_oldCrowSegmentCount; i++)
        {
            GameObject oldCrowSegment = Instantiate(m_oldCrowSegment, m_oldCrowPoolPosition);
            oldCrowSegment.SetActive(false);
            OldCrowSegmentPool.Enqueue(oldCrowSegment);
        }

        for(int i = 0; i < m_hookCount; i++)
        {
            GameObject hookObject = Instantiate(m_hookPrefab, m_hookPoolPosition);
            hookObject.SetActive(false);
            HookPool.Enqueue(hookObject);
        }

        for(int i = 0; i < m_BombPrefabCount; i++)
        {
            GameObject bomb = Instantiate(m_BombPrefab, m_BombPoolPosition);
            bomb.SetActive(false);
            BombPool.Enqueue(bomb);
        }

        for(int i = 0; i < m_DekuProjectileObjectCount; i++)
        {
            GameObject dekuProjectile = Instantiate(m_DekuProjectileObject, m_DekuProjectileObjectPoolPosition);
            dekuProjectile.SetActive(false);
            DekuPool.Enqueue(dekuProjectile);
        }

        for(int i = 0; i < m_MonsterArrowPrefabCount; i++)
        {
            GameObject monsterArrow = Instantiate(m_MonsterArrowPrefab, m_MonsterArrowPoolPosition);
            monsterArrow.SetActive(false);
            MonsterArrowPool.Enqueue(monsterArrow);
        }

        for(int i = 0; i < m_BatPrefabCount; i++)
        {
            GameObject bat = Instantiate(m_BatPrefab, m_BatPoolPosition);
            bat.SetActive(false);
            BatPool.Enqueue(bat);
        }

        for(int i = 0; i < m_PotPrefabCount; i++)
        {
            GameObject pot = Instantiate(m_PotPrefab, m_PotPoolPosition);
            pot.SetActive(false);
            PotPool.Enqueue(pot);
        }

        for(int i = 0; i < m_MagePrefabCount; i++)
        {
            GameObject mage = Instantiate(m_MagePrefab, m_MagePoolPosition);
            mage.SetActive(false);
            MagePool.Enqueue(mage);
        }

        for(int i = 0; i < m_GhoulPrefabCount; i++)
        {
            GameObject ghoul = Instantiate(m_GhoulPrefab, m_GhoulPoolPosition);
            ghoul.SetActive(false);
            GhoulPool.Enqueue(ghoul);
        }

        for(int i = 0; i < m_DekuPrefabCount; i++)
        {
            GameObject deku = Instantiate(m_DekuPrefab, m_DekuPoolPosition);
            deku.SetActive(false);
            DekuMonsterPool.Enqueue(deku);
        }
    }

    #region GetObject
    public GameObject GetArrow()
    {
        GameObject arrow = ArrowPool.Dequeue();
        Rigidbody arrowRigid = arrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = Vector3.zero; 
        ArrowPool.Enqueue(arrow);
        arrow.SetActive(true);
        return arrow;   
    }

    public GameObject GetFireBall()
    {
        GameObject fire = FireBallPool.Dequeue();
        Rigidbody fireRigid = fire.GetComponent<Rigidbody>();
        fireRigid.velocity = Vector3.zero;
        FireBallPool.Enqueue(fire);
        fire.SetActive(true);
        return fire;
    }

    public GameObject GetMagicBullet()
    {
        GameObject magicBullet = MagicBulletPool.Dequeue();
        MagicBulletPool.Enqueue(magicBullet);
        magicBullet.SetActive(true);
        return magicBullet;
    }

    public GameObject GetHitParticle()
    {
        GameObject hitParticle = HitParticlePool.Dequeue();
        HitParticlePool.Enqueue(hitParticle);
        return hitParticle;
    }

    public GameObject GetPlayerSegment(Vector3 position, Quaternion rotation)
    {
        if(PlayerSegmentPool.Count > 0)
        {
            GameObject playerSegment = PlayerSegmentPool.Dequeue();
            PlayerSegmentPool.Enqueue(playerSegment);
            playerSegment.transform.position = position;
            playerSegment.transform.rotation = rotation;
            playerSegment.SetActive(true);
            return playerSegment;
        }
        else
        {
            GameObject newSegment = Instantiate(m_playerSegment, m_playerSegmentPoolPosition);
            newSegment.AddComponent<BoxCollider>().isTrigger = true;
            newSegment.transform.position = position;
            return newSegment;
        }
    }

    public GameObject GetOldCrowSegment(Vector3 position)
    {
        if(OldCrowSegmentPool.Count > 0)
        {
            GameObject oldCrowSegment = OldCrowSegmentPool.Dequeue();
            OldCrowSegmentPool.Enqueue(oldCrowSegment);
            oldCrowSegment.transform.position = position;
            oldCrowSegment.SetActive(true);
            return oldCrowSegment;
        }
        else
        {
            GameObject newSegment = Instantiate(m_oldCrowSegment, m_oldCrowPoolPosition);
            newSegment.AddComponent<BoxCollider>().isTrigger = true;
            newSegment.transform.position = position;
            return newSegment;
        }
    }

    public GameObject GetHook()
    {
        GameObject hookObj = HookPool.Dequeue();
        HookPool.Enqueue(hookObj);
        hookObj.SetActive(true);
        return hookObj; 
    }

    public GameObject GetBomb()
    {
        GameObject bomb = BombPool.Dequeue();
        BombPool.Enqueue(bomb);
        Rigidbody bombRigid = bomb.GetComponent<Rigidbody>();
        bombRigid.velocity = Vector3.zero;
        bomb.SetActive(true);
        return bomb;
    }

    public GameObject GetDekuProjectile()
    {
        GameObject dekuProjectile = DekuPool.Dequeue();
        DekuPool.Enqueue(dekuProjectile);
        Rigidbody dekuRigid = dekuProjectile.GetComponent<Rigidbody>();
        dekuRigid.velocity = Vector3.zero;
        dekuProjectile.SetActive(true);
        return dekuProjectile;
    }

    public GameObject GetMonsterArrow()
    {
        GameObject monsterArrow = MonsterArrowPool.Dequeue();
        MonsterArrowPool.Enqueue(monsterArrow);
        monsterArrow.SetActive(true);
        return monsterArrow;
    }

    public GameObject GetBatMonster()
    {
        GameObject bat = BatPool.Dequeue();
        BatPool.Enqueue(bat);
        bat.SetActive(true);
        return bat;
    }

    public GameObject GetMageMonster()
    {
        GameObject mage = MagePool.Dequeue();
        MagePool.Enqueue(mage);
        mage.SetActive(true);
        return mage;
    }

    public GameObject GetPotMonster()
    {
        GameObject pot = PotPool.Dequeue();
        PotPool.Enqueue(pot);
        pot.SetActive(true);
        return pot;
    }

    public GameObject GetGhoulMonster()
    {
        GameObject ghoul = GhoulPool.Dequeue();
        GhoulPool.Enqueue(ghoul);
        ghoul.SetActive(true);
        return ghoul;
    }

    public GameObject GetDekuMonster()
    {
        GameObject deku = DekuMonsterPool.Dequeue();
        DekuMonsterPool.Enqueue(deku);
        deku.SetActive(true);
        return deku;
    }

    #endregion

    #region ReturnObject
    //Return Object
    public void ReturnArrow(GameObject arrow)
    {
        arrow.transform.SetParent(m_arrowPoolPosition);
        arrow.SetActive(false);
    }

    public void ReturnFireBall(GameObject fireBall)
    {
        fireBall.transform.SetParent(m_fireBallPoolPosition);
        fireBall.SetActive(false);
    }

    public void ReturnMagicBullet(GameObject magicBullet)
    {
        magicBullet.transform.SetParent(m_MagicBulletPoolPosition);
        magicBullet.SetActive(false);
    }

    public void ReturnHitParticle(GameObject hitParticle)
    {
        hitParticle.transform.SetParent(m_HitParticlePoolPosition);
        hitParticle.SetActive(false);
    }

    public void ReturnPlayerSegment(GameObject playerSegment)
    {
        playerSegment.transform.SetParent(m_playerSegmentPoolPosition);
        playerSegment.SetActive(false);
    }

    public void ReturnOldCrowSegment(GameObject oldCrowSegment)
    {
        oldCrowSegment.transform.SetParent(m_oldCrowPoolPosition);
        oldCrowSegment.SetActive(false);
    }

    public void ReturnHookObject(GameObject hookObj)
    {
        hookObj.transform.SetParent(m_hookPoolPosition);
        hookObj.SetActive(false);
    }

    public void ReturnBombObject(GameObject bombObj)
    {
        GameObject explosionParticleObject = bombObj.transform.GetChild(1).gameObject;
        explosionParticleObject.SetActive(false);
        bombObj.transform.SetParent(m_BombPoolPosition);
        bombObj.SetActive(false);
    }

    public void ReturnDekuProjectile(GameObject dekuProjectile)
    {
        dekuProjectile.transform.SetParent(m_DekuProjectileObjectPoolPosition);
        dekuProjectile.SetActive(false);
    }

    public void ReturnMonsterArrow(GameObject monsterArrow)
    {
        monsterArrow.transform.SetParent(m_MonsterArrowPoolPosition);
        monsterArrow.SetActive(false);
    }

    public void ReturnBatMonster(GameObject bat)
    {
        bat.transform.SetParent(m_BatPoolPosition);
        bat.SetActive(false);
    }

    public void ReturnMagerMonster(GameObject mage)
    {
        mage.transform.SetParent(m_MagePoolPosition);
        mage.SetActive(false);
    }

    public void ReturnPotMonster(GameObject pot)
    {
        pot.transform.SetParent(m_PotPoolPosition);
        pot.SetActive(false);
    }

    public void ReturnGhoulMonster(GameObject ghoul)
    {
        ghoul.transform.SetParent(m_GhoulPoolPosition); 
        ghoul.SetActive(false);
    }

    public void ReturnDekuMonster(GameObject deku)
    {
        deku.transform.SetParent(m_DekuPoolPosition);
        deku.SetActive(false);
    }
    #endregion
}
