using BaseSource;

namespace NinjaFruit
{
    public class FxSpawner : MonoSingleton<FxSpawner>
    {
        public FxTextPerfect[] listFxTextPrefab;

        protected override void OnAwake() { }

        public FxTextPerfect SpawnFxTextPerfect()
        {
            var fxClone = Instantiate(listFxTextPrefab.Rand());
            return fxClone;
        }
    }
}