using UnityEngine;

namespace Assets.Unitility.Tests.Scripts
{
    public class Destructible : MonoBehaviour {

        public void DestroyThis()
        {
            Destroy(gameObject);
        }
    }
}
