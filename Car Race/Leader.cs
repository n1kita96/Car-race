/// ===============================
///  AUTHOR 
///  Mykyta Shvets
/// ===============================
namespace Car_Race
{
    public class Leader
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public Leader (string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
