using AdventOfCodeChallenges.C10;

namespace AdventOfCodeChallenges.C13.Views
{
    public class SegmentDisplay : IGameView
    {
        private readonly IGameView scoreView;
        private readonly IGameView playView;
        private readonly Coordinate ScoreInput = new Coordinate(-1, 0);

        public SegmentDisplay(IGameView scoreView, IGameView playView)
        {
            this.scoreView = scoreView;
            this.playView = playView;
        }


        public void Draw(Coordinate coordinate, int drawCode)
        {
            if (coordinate == ScoreInput)
            {
                scoreView.Draw(Coordinate.Origin, drawCode);
            }
            else
            {
                playView.Draw(coordinate.Offset(0, 1), drawCode);
            }
        }
    }
}
