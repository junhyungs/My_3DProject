using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Bat = 1,
    Slime,
    Mage,
    Pot,
    Deku,
    Ghoul
}

public enum JsonName
{
    Monster,
    Boss,
    Player,
    PlayerSkill,
    PlayerWeapon,
    PrefabPath
}

public enum PlayerWeapon
{
    Sword,
    Hammer,
    Dagger,
    GreatSword,
    Umbrella
}

public enum MapType
{
    Lobby,
    Stage_1,
    Stage_2,
    Boss
}

public enum ObjectName
{
    Player,
    PlayerArrow,
    PlayerFireBall,
    PlayerHook,
    PlayerSegment,
    PlayerBomb,
    HitEffect,

    MageBullet,
    OldCrowSegment,
    GhoulArrow,
    DekuProjectile,

    Bat,
    Mage,
    Ghoul,
    Deku,
    Slime,

    Soul,
    UseUI,
    GetUI,
    LadderUI,

    Null
}