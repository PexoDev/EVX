using UnityEngine;

namespace Assets.Scripts
{
    public class InteractiveMapField
    {
        public MapField Field { get; set; }
        public SpriteRenderer SpriteRenderer { get; set; }
        public GameObject GameObject { get; set; }
        public ClickableObject ClickableObject { get; set; }

        public InteractiveMapField()
        {
            Field = new MapField();
        }
    }
}