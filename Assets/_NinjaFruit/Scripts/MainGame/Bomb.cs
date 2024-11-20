using UnityEngine;

namespace NinjaFruit
{
    public class Bomb : MonoBehaviour
    {
        public ParticleSystem fxExplosion;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GetComponent<Collider>().enabled = false;
                fxExplosion.transform.SetParent(null);
                fxExplosion.Play();
                gameObject.SetActive(false);
            
                GameManager.Instance.Explode();
            }
        }
    }
}