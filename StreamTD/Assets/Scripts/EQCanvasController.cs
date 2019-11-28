using System.Collections.Generic;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(RectTransform))]
    public class EQCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject _buttonPrefab;
        public const int RandomItemCost = 300;

        private RectTransform _parent;
        private Canvas _parentCanvas;
        private List<GameObject> _allButtonsObjects = new List<GameObject>();
        private List<Button> _allButtons = new List<Button>();

        private void Awake()
        {
            _parent = GetComponent<RectTransform>();
            _parentCanvas = GetComponentInParent<Canvas>();
        }

        public void UpdateCanvas()
        {
            _parent.sizeDelta = new Vector2(_parent.sizeDelta.x,120 * _allButtons.Count);

            for (var i = 0; i < _allButtonsObjects.Count; i++)
                _allButtonsObjects[i].transform.position = _parent.position + Vector3.up*100 + Vector3.down * 120 * i;
        }

        public void AddNewItem(Item item, GameController gc)
        {
            var newButtonObject = Instantiate(_buttonPrefab, _parent);
            var newButton = newButtonObject.GetComponent<Button>();
            newButton.GetComponentInChildren<Text>().text = item.Name;

            newButton.onClick.AddListener(() =>
            {
                if(gc.UIController.UpgradeManager.CurrentUnit == null) return;

                item.Apply(gc.UIController.UpgradeManager.CurrentUnit);

                _allButtonsObjects.Remove(newButtonObject);
                _allButtons.Remove(newButton);
                Destroy(newButtonObject);
                UpdateCanvas();
            });

            _allButtonsObjects.Add(newButtonObject);
            _allButtons.Add(newButton);
            UpdateCanvas();
        }
    }
}