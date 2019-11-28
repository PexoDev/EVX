using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Soldier;
using TMPro;
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

        private readonly Text _nameText;
        private readonly Text _describText;

        public UpgradeUIManager(Canvas choiceMenu,Button choiceLeft, Button choiceMid, Button choiceRight, Text nameText, Text describText)
        {
            _choiceMenuCanvas = choiceMenu;
            _nameText = nameText;
            _describText = describText;
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
            if (CurrentUnit == null)
            {
                _nameText.text = "";
                _describText.text = "";
                return;
            }

            _nameText.text = CurrentUnit.Soldiers.Select(sold => sold.Name + " [" + sold.HP+"]").Aggregate((s1, s2) => $"{s1}\n{s2}");

            _describText.text = "";
            foreach (PropertyInfo property in typeof(UnitParameters).GetProperties())
            {
                _describText.text += $"{property.Name}: {property.GetValue(CurrentUnit.UP)}\n";
            }
        }
    }
}