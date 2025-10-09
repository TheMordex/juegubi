using UnityEngine;

public class CharacterController
{
    private CharacterModel model;
    private CharacterView view;

    public CharacterController(CharacterModel model, CharacterView view)
    {
        this.model = model;
        this.view = view;
        UpdateView();
    }

    public void Attack(CharacterController target)
    {
        target.model.TakeDamage(model.AttackPower);
        target.UpdateView();
    }

    public void Heal(int amount)
    {
        model.Heal(amount);
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