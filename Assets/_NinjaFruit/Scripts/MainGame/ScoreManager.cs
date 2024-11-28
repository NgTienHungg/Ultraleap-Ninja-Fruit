using BaseSource;

namespace NinjaFruit
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        public int Score { get; private set; }

        protected override void OnAwake() { }

        public void ResetScore()
        {
            Score = 0;
        }

        public void IncreaseScore(int pts)
        {
            Score += pts;
            this.PostEvent(Constant.Event.OnScoreChanged, 1);
        }
    }
}