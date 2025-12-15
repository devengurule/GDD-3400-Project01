using UnityEngine;

namespace GDD3400.Project1
{
    public class SpeedController : MonoBehaviour
    {
        private int counter = 1;

        private void Update()
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                counter++;
                Debug.Log(counter);
            }
            else if(Input.GetButtonDown("Vert"))

            Time.timeScale = counter;
        }
    }
}
