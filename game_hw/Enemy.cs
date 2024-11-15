using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace game_hw
{
    internal class Enemy
    {
        public bool isAlive = true;
        public float speed = 0.01f;
        public float direcitonX;
        public float direcitonY;
        public float positionX;
        public float positionY;
        public List<int> shields = new List<int>();

        public Enemy(float posX, float posY, List<int> shields)
        {
            positionX = posX;
            positionY = posY;
            calculateDirection();
            this.shields = shields;
        }

        public void die()
        {
            isAlive = false;
        }

        private void calculateDirection()
        {
            double length = Math.Sqrt(Math.Pow(positionX, 2) + Math.Pow(positionY, 2));
            direcitonX = -(positionX / (float)length) * speed;
            direcitonY = -(positionY / (float)length) * speed;
        }

        public void calculatePosition()
        {
            positionX += direcitonX;
            positionY += direcitonY;
        }

        public void draw()
        {
            
        }
    }
}
