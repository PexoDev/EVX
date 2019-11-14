namespace Assets.Scripts.Controllers
{
    public class ScoreController
    {
        public int Score { get; set; }
        public void Add(int value)
        {
            Score += value;
        }
    }
}