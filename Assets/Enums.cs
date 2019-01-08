using System;

public class Enums
{
    public enum EnemyType : int
    {
        None,
        IntroClassPicker,
        Wolf,
        GoblinScout,
        Goblin,
        GoblinTaskMaster,
        Crocolisk,
        GoblinBoss,
        Eel,
        PotionGranter,
        Bandit,
        UpgradeShrine,
        TransformShrine,
        RareChest,
        PitfallTrap,
        DarkDealer,
        MagicMirror,
    }   

    public enum StatType : int
    {
        Level,
        Health,
        MaxMana,
        Action,
        Armor,

    }

    public enum CardType : int
    {
        None,
        Attack,
        Armor,
        Spell,
        Mana,
        Feat,
        Equipment,
        Trinket,
        
        ClassSelect,
    }

    public enum CardTargetType : int
    {
        Self,
        SingleEnemy,
        AllEnemies,
    }

    public enum CostType : int
    {
        None,
        Mana,
        Action,
        Health,
    }

    public enum EncounterID : int
    {
        None,
        GenericCombatStart,
        StartSequence,
        CrossRoadsEncounter,
        EnterCaveDungeon,

        CaveDungeon,
        Cave_GoblinFightEasy,
        Cave_GoblinFightMedium,
        Cave_Crocolisk,
        Cave_Eels,
        Cave_Pitfall,
        Cave_GoblinTaskMaster,
        Cave_Bandit,
        Cave_GoblinBoss,

        GainHealingPotions,
        Rest,
        Shrine_Upgrade,
        Shrine_Transform,
        DarkDealer,
        MagicMirror,

        Chest_Rare,
    }

    public enum EncounterType : int
    {
        None,
        Combat,
        ScriptedSequence,
        TimelineSequence,
        RandomizedDungeon,
    }

    public enum ScriptActionType : int
    {
        None,
        PlayTimeline,
        ReadDialogueWaitForInput,
        DrawEnemyDeck,
        WaitForChoice,
        ChoiceResponse,
        TransitionToEncounter,
        ReadDialogue,
        PauseTimeline,
        EndCombat,
    }

    public enum CardEffect : int
    {
        None,
        //Battle Effect
        PhysicalDamage,
        MagicDamage,
        GainHealth,
        GainMana,
        GainAction,
        DrawCard,
        ActivateTrinket,
        ApplyEffect,
        GainArmor,

        //Non-Combat Gains
        GainStat,
        GainEquipment,
        GainTrinket,
        GainCard,

        //Class Selector
        SelectClass,
        StarterLevel,

    }

    public enum EquipmentSlot : int
    {
        Head,
        Body,
        RightHand,
        LeftHand,
        TwoHand,
        DualWield,
        Acessory,
        Trinket
    }

    public enum EquipmentType : int
    {
        Sword,
        Shield,
        Dagger,
        Bow,
        Helm,
        Hat,
        Robe,
        Ring,
        Necklace,
    }

    public enum Card : int
    {
        None,
        ClassSelect_None,
        ClassSelect_Warrior,
        ClassSelect_Mage,
        ClassSelect_Ranger,
        BaseAttack,
        Fireball,
        Focus,
        Block,
        Rend,
        MinorPoison,
        Scorch,

    }

    public enum Equipment : int
    {
        WarriorsAxe,
        RangerBow,
        MageStaff,
        MysteriousRing,
    }

    public enum Trinket : int
    {
        HardStone,
        ShiftingAmulet,
        ManaFocus,
        HandyBauble,
    }

    public enum Class : int
    {
        None,
        Warrior,
        Mage,
        Ranger,
    }

    public enum AppliedEffect : int
    {
        Bleed,
        Poison,
        Vulnerable,
        Burn,
    }

    public enum Rarity : int
    {
        None,
        Common,
        Rare,
        Epic,
        Legendary,
    }
    

    public static T ParseEnum<T>(string value)
    {
        try
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        catch(ArgumentException e)
        {
            return default(T);
        }
        
    }
}
