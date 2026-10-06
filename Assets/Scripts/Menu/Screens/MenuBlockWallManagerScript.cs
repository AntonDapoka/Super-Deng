
using UnityEngine.UI;
using UnityEngine;

namespace Menu.Screens
{
    public class MenuBlockWallManagerScript : MonoBehaviour
    {
        [SerializeField] private Image wall;

        public void TurnOnBlockWall()
        {
            wall.gameObject.SetActive(true);
        }

        public void TurnOffBlockWall()
        {
            wall.gameObject.SetActive(false);
        }
    }
}