namespace SnowmanHanoiTower;

public class ScoreManager
{
    public int Score { get; private set; } = 0;

    public void IncreaseScore()
    {
        Score++;
    }
}