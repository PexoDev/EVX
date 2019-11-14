using System.Reflection;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Soldier;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class UpgradeUIManager
    {
        private Soldier _currentSoldier;
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

        public Soldier CurrentSoldier
        {
            get => _currentSoldier;
            set
            {
                _choiceMenuCanvas.enabled = true;


                _currentSoldier = value;
                Render();
            }
        }

        public void Render()
        {
            if(CurrentSoldier == null) return;

            _nameText.text = CurrentSoldier.Name;

            _describText.text = "";
            foreach (PropertyInfo property in typeof(UnitParameters).GetProperties())
            {
                _describText.text += $"{property.Name}: {property.GetValue(CurrentSoldier.UnitParams)}\n";
            }
        }
    }
}