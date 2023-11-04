
public class Health 
{
    private readonly int maxHealth;
    private int points;
    private Health() { }
    public Health(int health)
    {
        this.maxHealth = health;
        this.points = health;
    }
    public int Points => points;
    public void TakeDamage(int damage) => points -= damage;
    public float GetHealthPercent() => (float)points / (float)maxHealth;
    public bool IsAlive() => points > 0;
    public void Reset() => points = maxHealth;
}
