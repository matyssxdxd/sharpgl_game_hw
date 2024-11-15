using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace game_hw
{
    internal class Bullet
    {
        public float[] color;
        public int colorID;
        public float speed = 0.1f;
        public float directionX;
        public float directionY;
        public float positionX = 0.0f;
        public float positionY = 0.0f;

        public Bullet (float[] color,float mouseX, float mouseY, int colorID)
        {
            this.color = color;
            this.colorID = colorID;
            calculateDirection(mouseX, mouseY);
        }

        private void calculateDirection(float mouseX, float mouseY)
        {
            double length = Math.Sqrt(Math.Pow(mouseX, 2) + Math.Pow(mouseY, 2));
            directionX = mouseX / (float)length * speed;
            directionY = mouseY / (float)length * speed;
        }

        public void updatePosition()
        {
            positionX += directionX;
            positionY += directionY;
        }
    }
}
