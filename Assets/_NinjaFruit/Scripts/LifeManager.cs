using BaseSource;
using UnityEngine;
using UnityEngine.UI;

namespace NinjaFruit
{
    public class LifeManager : MonoSingleton<LifeManager>
    {
        public Image[] heartImages;
        public int maxLife = 3;

        private int _life;
        public int Life
        {
            get => _life;
            set
            {
                _life = value;
                for (var i = 0; i < maxLife; i++)
                {
                    heartImages[i].color = _life > i ? Color.white : Color.black;
                }
            }
        }
        public bool IsOutOfLife => Life <= 0;

        protected override void OnAwake() { }

        private void OnValidate()
        {
            maxLife = heartImages.Length;
        }

        public void ResetLife()
        {
            Life = maxLife;
        }
    }
}