using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;

public class ConfigHandler
{

    public static Dictionary<Enums.EnemyType, EnemyConfig> enemyConfigs;
    public static Dictionary<Enums.EncounterID, EncounterConfig> encounterConfigs;
    public static Dictionary<Enums.Card, CardConfig> cardConfigs; 

    public static void LoadConfigs(string ConfigFolderPath)
    {
        string[] files = Directory.GetFiles(ConfigFolderPath, "*.xml");

        foreach(var f in files)
        {
            LoadXMLConfigFile(f);
        }
    }

    static void LoadXMLConfigFile(string f)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(f);
        
        XmlNode header = doc.FirstChild;
        //UnityEngine.Debug.Log(header.Name);
        if (header.Name == "enemies")
        {
            enemyConfigs = new Dictionary<Enums.EnemyType, EnemyConfig>();

           foreach(XmlNode enemy in header.ChildNodes)
           {
                EnemyConfig config = new EnemyConfig();
                config.type = Enums.ParseEnum<Enums.EnemyType>(enemy.Attributes["type"].Value);
                //UnityEngine.Debug.Log(config.type);
                config.name = enemy.Attributes["name"].Value;

                config.health = int.Parse(enemy.ChildNodes[0].Attributes["amount"].Value);

                config.mana = int.Parse(enemy.ChildNodes[1].Attributes["amount"].Value);

                config.action = int.Parse(enemy.ChildNodes[2].Attributes["amount"].Value);

                config.deck = new List<Enums.Card>();
                foreach (XmlNode card in enemy.SelectSingleNode("deck"))
                {
                    config.deck.Add(Enums.ParseEnum<Enums.Card>(card.Attributes["type"].Value));
                }

                enemyConfigs.Add(config.type, config);
            }
        }
        else if(header.Name == "encounters")
        {
            encounterConfigs = new Dictionary<Enums.EncounterID, EncounterConfig>();

            foreach(XmlNode encounter in header.ChildNodes)
            {
                if(encounter.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }
                EncounterConfig config = new EncounterConfig();
                config.id = Enums.ParseEnum<Enums.EncounterID>(encounter.Attributes["id"].Value);
                //UnityEngine.Debug.Log(config.id);
                if (encounter.Attributes["type"] != null)
                {
                    config.type = Enums.ParseEnum<Enums.EncounterType>(encounter.Attributes["type"].Value);
                }
                if(encounter.Attributes["hideStatsUI"] != null)
                {
                    config.hideStatsUI = bool.Parse(encounter.Attributes["hideStatsUI"].Value);
                }
                
                if (encounter.Attributes["hideHandUI"] != null)
                {
                    config.hideHandUI = bool.Parse(encounter.Attributes["hideHandUI"].Value);
                }

                foreach(XmlNode childNode in encounter.ChildNodes)
                {
                    if(childNode.Name == "enemies")
                    {
                        config.enemies = new List<Enums.EnemyType>();
                        foreach(XmlNode child in childNode)
                        {
                            config.enemies.Add(Enums.ParseEnum<Enums.EnemyType>(child.Attributes["type"].Value));
                        }
                    }
                    else if(childNode.Name == "sequence")
                    {
                        config.sequence = new List<ScriptConfig>();
                        foreach(XmlNode child in childNode)
                        {
                            ScriptConfig scriptConfig = new ScriptConfig();
                            scriptConfig.type = Enums.ParseEnum<Enums.ScriptActionType>(child.Attributes["type"].Value);
                            scriptConfig.dialogue = child.Value;
                            if(scriptConfig.type == Enums.ScriptActionType.PlayTimeline)
                            {
                                scriptConfig.timelineName = child.Attributes["timelineName"].Value;
                            }
                            config.sequence.Add(scriptConfig);
                        }
                    }
                }
                encounterConfigs.Add(config.id, config);
            }
        }
        else if(header.Name == "cards")
        {
            cardConfigs = new Dictionary<Enums.Card, CardConfig>();

            foreach(XmlNode cardNode in header.ChildNodes)
            {
                CardConfig config = new CardConfig();

                config.card = Enums.ParseEnum<Enums.Card>(cardNode.Attributes["type"].Value);
                config.displayName = cardNode.Attributes["displayName"].Value;
                config.imagePath = cardNode.Attributes["imagePath"].Value;
                if (cardNode.Attributes["cardType"] != null)
                {
                    config.cardType = Enums.ParseEnum<Enums.CardType>(cardNode.Attributes["cardType"].Value);
                }
                else
                {
                    config.cardType = Enums.CardType.None;
                }
                if(cardNode.Attributes["targetType"] != null)
                {
                    config.targetType = Enums.ParseEnum<Enums.CardTargetType>(cardNode.Attributes["targetType"].Value);
                }
                else
                {
                    config.targetType = Enums.CardTargetType.Self;
                }
                if (cardNode.Attributes["rarity"] != null)
                {
                    config.rarity = Enums.ParseEnum<Enums.Rarity>(cardNode.Attributes["rarity"].Value);
                }

                config.costs = new List<KeyValuePair<Enums.CostType, int>>();
                if(cardNode.SelectSingleNode("costs") != null)
                {
                    foreach(XmlNode costNode in cardNode.SelectSingleNode("costs"))
                    {
                        config.costs.Add(new KeyValuePair<Enums.CostType, int>(Enums.ParseEnum<Enums.CostType>(costNode.Attributes["type"].Value), int.Parse(costNode.Attributes["amount"].Value)));
                    }
                }

                config.effects = new List<CardEffectConfig>();
                if(cardNode.SelectSingleNode("effects") != null)
                {
                    foreach(XmlNode effectNode in cardNode.SelectSingleNode("effects"))
                    {
                        CardEffectConfig effectConfig = new CardEffectConfig();
                        effectConfig.effect = Enums.ParseEnum<Enums.CardEffect>(effectNode.Attributes["type"].Value);
                        if(effectNode.Attributes["amount"] != null)
                        {
                            effectConfig.amount = int.Parse(effectNode.Attributes["amount"].Value);
                        }
                        if (effectNode.Attributes["stat"] != null)
                        {
                            effectConfig.stat = Enums.ParseEnum<Enums.StatType>(effectNode.Attributes["stat"].Value);
                        }
                        if (effectNode.Attributes["equipment"] != null)
                        {
                            effectConfig.equipment = Enums.ParseEnum<Enums.Equipment>(effectNode.Attributes["equipment"].Value);
                        }
                        if (effectNode.Attributes["trinket"] != null)
                        {
                            effectConfig.trinket = Enums.ParseEnum<Enums.Trinket>(effectNode.Attributes["trinket"].Value);
                        }
                        if(effectNode.Attributes["class"] != null)
                        {
                            effectConfig.selectedClass = Enums.ParseEnum<Enums.Class>(effectNode.Attributes["class"].Value);
                        }
                        if (effectNode.Attributes["card"] != null)
                        {
                            effectConfig.card = Enums.ParseEnum<Enums.Card>(effectNode.Attributes["card"].Value);
                        }
                        if(effectNode.Attributes["effectType"] != null)
                        {
                            effectConfig.appliedEffect = Enums.ParseEnum<Enums.AppliedEffect>(effectNode.Attributes["effectType"].Value);
                        }
                        config.effects.Add(effectConfig);
                    }
                }
                config.description = GetDescription(cardNode.Attributes["description"].Value, config);
                //UnityEngine.Debug.Log(config.description);
                cardConfigs.Add(config.card, config);
            }
        }
    }

    static string GetDescription(string desc, CardConfig config)
    {
        string[] splitString = desc.Split('|');
        string outDescription = "";

        for(int i = 0; i < splitString.Length; i++)
        {
            Enums.CardEffect stat = Enums.ParseEnum<Enums.CardEffect>(splitString[i]);
            //UnityEngine.Debug.Log(stat);
            if (stat != Enums.CardEffect.None)
            {
                CardEffectConfig effect = config.effects.Find((x) => x.effect == stat);
                if(effect.effect == Enums.CardEffect.PhysicalDamage || effect.effect == Enums.CardEffect.MagicDamage)
                {
                    outDescription += effect.amount + " Damage"; 
                }
                else if(effect.effect == Enums.CardEffect.ApplyEffect)
                {
                    outDescription += effect.amount + " " + effect.appliedEffect.ToString();
                }
                else if(effect.effect == Enums.CardEffect.GainMana)
                {
                    outDescription += effect.amount + " Mana";
                }
                else if(effect.effect == Enums.CardEffect.GainArmor)
                {
                    outDescription += effect.amount;
                }
            }
            else
            {
                outDescription += splitString[i];
            }
            
        }

        return outDescription;
    }

    
}

public class EnemyConfig
{
    public Enums.EnemyType type;
    public string name;

    public int health;
    public int mana;
    public int action;

    public List<Enums.Card> deck;
}

public class EncounterConfig
{
    public Enums.EncounterID id;
    public Enums.EncounterType type;
    public bool hideStatsUI;
    public bool hideHandUI;

    public List<Enums.EnemyType> enemies;
    public List<ScriptConfig> sequence;
}

public class ScriptConfig
{
    public Enums.ScriptActionType type;
    public string dialogue;
    public string timelineName;
    public Enums.EncounterID encounterTransitionID;
}

public class CardConfig
{
    public Enums.Card card;
    public Enums.CardType cardType;
    public Enums.CardTargetType targetType;
    public Enums.Rarity rarity;
    public string displayName;
    public string imagePath;
    public string description;
    public List<KeyValuePair<Enums.CostType, int>> costs;
    public List<CardEffectConfig> effects;
}

public class CardEffectConfig
{
    public Enums.CardEffect effect;
    public int amount;
    public Enums.Card card;
    public Enums.StatType stat;
    public Enums.Equipment equipment;
    public Enums.Trinket trinket;
    public Enums.Class selectedClass;
    public Enums.AppliedEffect appliedEffect;
}
