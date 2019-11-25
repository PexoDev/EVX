using System.CodeDom;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class EconomyController
    {
        public const char QuantChar = 'ꝙ';

        private readonly Text _quantText;

        private int _quants = 600;
        public int Quants
        {
            get => _quants;
            set
            {
                _quants = value;
                _quantText.text = $"{_quants}{QuantChar}";
            }
        }

        public bool TryBuy(int price)
        {
            if (_quants < price) return false;
            Quants -= price;
            return true;
        }

        public EconomyController(Text quantText)
        {
            _quantText = quantText;
        }
    }
}