
public class Health
{

    int currentHealth;
    int maxHealth;

    public void Init()
    {

        Init(10);
    }

    public void Init(int health)
    {
        currentHealth = health;
        maxHealth = health;
    }

    public void Tick()
    {

    }

    
	public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void IncreaseMaxHealth(int amount)
    {
        currentHealth += amount;
        maxHealth += amount;
    }

    public void ModifyCurrentHealth(int amount)
    {
        currentHealth += amount;
    }
}
