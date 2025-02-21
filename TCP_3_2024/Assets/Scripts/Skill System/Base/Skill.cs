using Fusion;
public abstract class Skill : NetworkBehaviour
{
    public bool HasUsed { get; private set; }

    public void EnableUse()
    {
        HasUsed = false;
    }

    public void DisableUse()
    {
        HasUsed = true;
    }
}
