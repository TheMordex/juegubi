public class CharacterModel
{
    public string Name;
    public int MaxHealth;
    public int CurrentHealth;
    public int AttackPower;

    public CharacterModel(string name, int maxHealth, int attackPower)
    {
        Name = name;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        AttackPower = attackPower;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth < 0)
            CurrentHealth = 0;
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }
}