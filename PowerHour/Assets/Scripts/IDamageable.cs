public interface IDamageable
{
    float maxHealth { get; set; }
    float currentHealth { get; set; }
    void TakeDamage(int damage);
}