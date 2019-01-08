using System.Collections.Generic;

public class Card {

    public Card(Enums.Card card)
    {
        Init(card);
    }

    Enums.Card type;

    public void Init(Enums.Card card)
    {
        type = card;
    }

    public Enums.Card GetCard()
    {
        return type;
    }

    public string GetDisplayName()
    {
        return ConfigHandler.cardConfigs[type].displayName;
    }

    public Enums.CardType GetCardType()
    {
        return ConfigHandler.cardConfigs[type].cardType;
    }

    public string GetDescription()
    {
        return ConfigHandler.cardConfigs[type].description;
    }

    public List<CardEffectConfig> GetEffects()
    {
        return ConfigHandler.cardConfigs[type].effects;
    }

    public bool isCombatCard()
    {
        //UnityEngine.Debug.Log(type);
        CardConfig config = ConfigHandler.cardConfigs[type];
        if (config.cardType == Enums.CardType.ClassSelect)
        {
            return false;
        }
        if (config.cardType == Enums.CardType.None)
        {
            return false;
        }
        if (config.cardType == Enums.CardType.Equipment)
        {
            return false;
        }

        return true;
    }

    public Enums.CardTargetType GetTargetType()
    {
        return ConfigHandler.cardConfigs[type].targetType;
    }
}
