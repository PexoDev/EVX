using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class UIController
    {
        private readonly Button _choiceLeft;
        private readonly Text _choiceLeftText;
        private readonly Canvas _choiceMenu;
        private readonly Button _choiceMid;
        private readonly Text _choiceMidText;
        private readonly Button _choiceRight;
        private readonly Text _choiceRightText;

        private readonly Text _choiceText;
        private readonly GameController _gc;

        public HQUIManager HQUIManager { get; private set; }
        private GameObject _panelPrefab;
        public UpgradeUIManager UpgradeManager { get; set; }
        public EQCanvasController EQCanvasController { get; set; }

        public UIController(GameController gc, EQCanvasController eqCanvas, Canvas choiceMenu, Text choiceText, Button choiceLeft,
            Button choiceMid, Button choiceRight, Button recruitNewUnitButton, Button sellUnitButton, 
            Button buyRandomItemButton, Text nameText, Text describText)
        {
            _gc = gc;
            _choiceMenu = choiceMenu;
            _choiceText = choiceText;

            _choiceLeft = choiceLeft;
            _choiceLeftText = choiceLeft.GetComponentInChildren<Text>();

            _choiceMid = choiceMid;
            _choiceMidText = choiceMid.GetComponentInChildren<Text>();

            _choiceRight = choiceRight;
            _choiceRightText = choiceRight.GetComponentInChildren<Text>();

            UpgradeManager = new UpgradeUIManager(choiceMenu, choiceLeft, choiceMid, choiceRight, nameText, describText);
            HQUIManager = new HQUIManager(recruitNewUnitButton, sellUnitButton, gc.EconomyController, UpgradeManager);
            EQCanvasController = eqCanvas;

            buyRandomItemButton.onClick.AddListener(() =>
            {
                if (!gc.EconomyController.TryBuy(EQCanvasController.RandomItemCost)) return;

                eqCanvas.AddNewItem(ConsumableItemsList.AllConsumableItems[GameController.RandomGenerator.Next(0,ConsumableItemsList.AllConsumableItems.Length)], gc);
            });
        }

        public void Instantiate()
        {
            HQUIManager.Instantiate(SetButtonsBehaviour, _gc.EnemiesController,
                _gc.SoldiersController);
        }

        public async Task SetButtonsBehaviour(string title, string leftButtonName, string midButtonName,
            string rightButtonName, Action leftButtonBehaviour, Action middleButtonBehaviour,
            Action rightButtonBehaviour)
        {
            var chosen = false;

            _choiceText.text = title;
            _choiceLeftText.text = leftButtonName;
            _choiceMidText.text = midButtonName;
            _choiceRightText.text = rightButtonName;

            _choiceMenu.gameObject.SetActive(true);
            _choiceLeft.gameObject.SetActive(true);
            _choiceMid.gameObject.SetActive(true);
            _choiceRight.gameObject.SetActive(true);

            if (leftButtonBehaviour == null) _choiceLeft.gameObject.SetActive(false);
            if (middleButtonBehaviour == null) _choiceMid.gameObject.SetActive(false);
            if (rightButtonBehaviour == null) _choiceRight.gameObject.SetActive(false);

            _choiceLeft.onClick.RemoveAllListeners();
            _choiceMid.onClick.RemoveAllListeners();
            _choiceRight.onClick.RemoveAllListeners();

            _choiceLeft.onClick.AddListener(() =>
            {
                leftButtonBehaviour?.Invoke();
                chosen = true;
            });

            _choiceMid.onClick.AddListener(() =>
            {
                middleButtonBehaviour?.Invoke();
                chosen = true;
            });

            _choiceRight.onClick.AddListener(() =>
            {
                rightButtonBehaviour?.Invoke();
                chosen = true;
            });

            await Task.Run(() =>
            {
                while (!chosen)
                {
                }
            });

            _choiceMenu.gameObject.SetActive(false);
        }
    }
}