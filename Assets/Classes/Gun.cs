using Unity.VisualScripting;
using UnityEngine;

namespace Classes
{
    public class Gun
    {
        public Vector3Int Position;
        public int Cooldown = 5;
        public int TimeToShoot = 2;

        public Gun(Vector3Int position)
        {
            Position = position;
        }
        // public Vector3Int Position;
        // public int Cooldown = 3;
        // public int TimeToShoot = 1;

        public int MakeMove()
        {
            TimeToShoot -= 1;
            if (TimeToShoot == 0)
            {
                TimeToShoot = Cooldown;
                return 1;
            }

            return 0;
        }
    }
}