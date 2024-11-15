using SharpGL;
using SharpGL.SceneGraph.Assets;
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
        static float[] redColor = { 1.0f, 0.0f, 0.0f };
        static float[] greenColor = { 0.0f, 1.0f, 0.0f };
        static float[] blueColor = { 0.0f, 0.0f, 1.0f };
        static float[] yellowColor = { 0.0f, 1.0f, 1.0f };
        float[][] colors = { redColor, greenColor, blueColor, yellowColor };

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

        public void draw(OpenGL gl, Texture texture)
        {
            calculatePosition();

            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            texture.Bind(gl);

            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.TexCoord(0.0f, 1.0f); gl.Vertex(-0.25f + positionX, -0.25f + positionY, 0.0f);
            gl.TexCoord(1.0f, 1.0f); gl.Vertex(0.25f + positionX, -0.25f + positionY, 0.0f);
            gl.TexCoord(1.0f, 0.0f); gl.Vertex(0.25f + positionX, 0.25f + positionY, 0.0f);
            gl.TexCoord(0.0f, 0.0f); gl.Vertex(-0.25f + positionX, 0.25f + positionY, 0.0f);

            gl.End();

            gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.Disable(OpenGL.GL_BLEND);

            for (int j = 0; j < shields.Count; ++j)
            {
                float shieldOffsetY = 0.5f;
                float shieldGap = 0.25f;
                float shieldOffsetX = (j - (shields.Count - 1) / 2.0f) * shieldGap;

                gl.Color(colors[shields[j]]);

                gl.Begin(OpenGL.GL_QUADS);
                gl.Vertex(-0.1f + positionX + shieldOffsetX, -0.1f + positionY + shieldOffsetY, 0.0f);
                gl.Vertex(0.1f + positionX + shieldOffsetX, -0.1f + positionY + shieldOffsetY, 0.0f);
                gl.Vertex(0.1f + positionX + shieldOffsetX, 0.1f + positionY + shieldOffsetY, 0.0f);
                gl.Vertex(-0.1f + positionX + shieldOffsetX, 0.1f + positionY + shieldOffsetY, 0.0f);
                gl.End();
            }
        }
    }
}
