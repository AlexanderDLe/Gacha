namespace RPG.Core
{
    public interface IHasCooldown
    {
        int Id { get; }
        float CooldownDuration { get; }
    }
}