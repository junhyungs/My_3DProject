
public enum State
{
    Idle,
    Move,
    Ladder,
    Skill,
    Attack,
    Roll,
    Hit,
    Falling
}

public enum MVVM
{
    Health_Event,
    Soul_Event,
    HealthItem_Event,
    CurrentSkill_Event,
    SkillCount_Event
}

public enum Map
{
    MainStage,
    BossStage,
    GimikStage,
    EndingStage
}

public enum JsonName
{
    Monster,
    Boss,
    Player,
    PlayerSkill,
    PlayerWeapon,
    PrefabPath,
    BossProjectile,
    Dialogue,
    Item,
    Ability,
    Map
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
    MotherProjectile,

    Bat,
    Mage,
    Ghoul,
    Deku,
    Slime,

    Soul,
    UseUI,
    GetUI,
    LadderUI,
    InteractionDialogueUI,
    InteractionDialogueBoxUI,
    Ring,
    RustyKey,
    Teddy,
    Trowel,
    Surveillance,
    OpenUI,
    ThankYouUI,

    Null
}

public enum VineType
{
    Right,
    Left,
}

public enum TimeLineType
{//타임라인 이름 명확하게
    Intro,
    HallCrow,
    Move,
    Out,
    ForestMotherIntro,
    ForestMotherDeath
}
public enum GimikEnum
{
    OpenDoor = 1,
    SpawnMonster = 2,
    nextSceneDoor = 3,
}

public enum DialogueOrder
{
    Story,
    Loop,
    End
}

public enum NPC
{//클래스 이름
    TestNPC,
    BusNPC,
    TelePhoneNPC,
    HallCrow_1,
    HallCrow_2,
    HallCrow_3,
    HallCrow_4,
    HallCorw_5,
    Bager,
    Security,
    Banker,
    Agatha
}

public enum PlayerWeapon
{
    Sword,
    Hammer,
    Dagger,
    GreatSword,
    Umbrella
}

public enum PlayerWeaponID
{
    W101,
    W102,
    W103,
    W104,
    W105
}

public enum Effect
{
    Sword,
    Hammer,
    Dagger,
    GreatSword,
    Umbrella
}

public enum BaseItemType
{
    Witch,
    Swampking,
    Betty,
    CrystalHP,
    CrystalMagic,
    Sword
}

public enum TrinketItemType
{
    Ring,
    RustyKey,
    Teddy,
    Trowel,
    Surveillance
}

public enum ItemType
{
    Ring,
    RustyKey,
    Teddy,
    Trowel,
    Surveillance,
    Sword,
    Hammer,
    Dagger,
    GreatSword,
    Umbrella
}

