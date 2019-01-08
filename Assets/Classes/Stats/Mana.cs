
public class Mana {


    int currentMana;
    int maxMana;

    int startingMana;

    public void Init()
    {
        Init(10);
    }

    public void Init(int max)
    {
        maxMana = max;

        currentMana = 0;
        startingMana = 0;
    }

    public void Tick()
    {

    }

    public int GetMaxMana()
    {
        return maxMana;
    }

    public int GetCurrentMana()
    {
        return currentMana;
    }

    public int GetStartingMana()
    {
        return startingMana;
    }

    public void IncreaseMaxMana(int amount)
    {
        maxMana += amount;
    }
}
