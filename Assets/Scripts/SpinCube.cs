using UnityEngine;

namespace MRCube
{
    public class SpinCube : MonoBehaviour
    {
        public float speedX = 90f; // degrees per second
        public float speedY = 90f; // degrees per second

        void Update()
        {
            transform.Rotate(speedX * Time.deltaTime, speedY * Time.deltaTime, 0f);
        }
    }
}
