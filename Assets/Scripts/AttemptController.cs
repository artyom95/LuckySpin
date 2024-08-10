

public class AttemptController
{
    public int Attempts { get; private set; }
        
    public AttemptController(GameSettings gameSettings)
    {
        Attempts = gameSettings.AmountAttempts;
    }

    public void DecreaseAttempts()
    {
        if (Attempts > 0)
        {
            Attempts--;
        }
        else
        {
            return;
        }
    }
}