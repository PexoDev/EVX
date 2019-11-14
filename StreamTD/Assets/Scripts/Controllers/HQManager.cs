using Assets.Scripts.Units.Soldier;

namespace Assets.Scripts.Controllers
{
    public class HQManager
    {
        private SoldiersController _soldiersController;

        private Soldier _selectedSoldier;
        public Soldier SelectedSoldier
        {
            get => _selectedSoldier;
            set => SetSelectedSoldier(value);
        }

        public HQManager(SoldiersController sc)
        {
            _soldiersController = sc;
        }

        private void SetSelectedSoldier(Soldier soldier)
        {
            if (soldier == null)
                GameController.Mode = GameMode.Play;

            _selectedSoldier = soldier;
        }

        public bool PlaceSoldier(MapField field)
        {
            var res = _soldiersController.SpawnSoldier(_selectedSoldier, field);
            SelectedSoldier = null;
            return res;
        }
    }
}


