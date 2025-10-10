using UnityEngine;

public class CharacterController
{
    public CharacterModel model { get; private set; }
    private CharacterView view;

    public CharacterController(CharacterModel model, CharacterView view)
    {
        this.model = model;
        this.view = view;
        UpdateView();
    }

    public void Attack(CharacterController target)
    {
        if (model.IsStunned)
        {
            Debug.Log($"{model.Name} está aturdido y no puede atacar este turno.");
            model.IsStunned = false;
            return;
        }

        int totalDamage = Mathf.Max(model.AttackPower - target.model.DefenseBonus, 0);
        target.model.TakeDamage(totalDamage);
        Debug.Log($"{model.Name} ataca a {target.model.Name} causando {totalDamage} de daño.");
        target.UpdateView();
    }

    public void Heal(int amount)
    {
        model.Heal(amount);
        Debug.Log($"{model.Name} se cura {amount} puntos de vida.");
        UpdateView();
    }

    public void ApplyStatus(StatusEffect effect)
    {
        model.AddEffect(effect);
        UpdateView();
    }

    public void UpdateStatusEffects()
    {
        model.UpdateStatusEffects();
        UpdateView();
    }

    public void UpdateView()
    {
        if (view != null)
            view.UpdateHealth(model.CurrentHealth, model.MaxHealth);
    }

    public bool IsDead() => model.IsDead();
    public string GetName() => model.Name;
}