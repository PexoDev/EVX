using System.Reflection;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Soldier;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class UpgradeUIManager
    {
        private Unit _currentUnit;
        private readonly Canvas _choiceMenuCanvas;
        private readonly Button _choiceLeft;
        private readonly Button _choiceMid;
        private readonly Button _choiceRight;

        private readonly Slider _experienceSlider;
        private readonly SpriteRenderer[] _levelPoints;
        private readonly Text _nameText;
        private readonly Text _describText;

        public UpgradeUIManager(Canvas choiceMenu,Button choiceLeft, Button choiceMid, Button choiceRight, Text nameText, Text describText, SpriteRenderer[] levelPoints, Slider expSlider)
        {
            _choiceMenuCanvas = choiceMenu;
            _nameText = nameText;
            _describText = describText;
            _levelPoints = levelPoints;
            _experienceSlider = expSlider;
        }

        public Unit CurrentUnit
        {
            get => _currentUnit;
            set
            {
                _choiceMenuCanvas.enabled = true;


                _currentUnit = value;
                Render();
            }
        }

        public void Render()
        {
            if(CurrentUnit == null) return;

            //_nameText.text = CurrentUnit.Name;

            _describText.text = "";
            foreach (PropertyInfo property in typeof(UnitParameters).GetProperties())
            {
                _describText.text += $"{property.Name}: {property.GetValue(CurrentUnit.UP)}\n";
            }
        }
    }
}