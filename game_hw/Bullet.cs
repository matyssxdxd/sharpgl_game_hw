using SharpGL;
using SharpGL.SceneGraph.Assets;
using System;

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
        private float rotationAngle = 0.0f;

        public Bullet(float[] color, float mouseX, float mouseY, int colorID)
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

            rotationAngle += 20.0f;
            if (rotationAngle >= 360.0f)
            {
                rotationAngle -= 360.0f;
            }
        }

        public void draw(OpenGL gl, Texture texture)
        {
            updatePosition();

            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            texture.Bind(gl);

            gl.PushMatrix();

            gl.Translate(positionX, positionY, 0.0f);
            gl.Rotate(rotationAngle, 0.0f, 0.0f, 1.0f);

            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(color);
            gl.TexCoord(0.0f, 1.0f); gl.Vertex(-0.1f, -0.1f, 0.0f);
            gl.TexCoord(1.0f, 1.0f); gl.Vertex(0.1f, -0.1f, 0.0f);
            gl.TexCoord(1.0f, 0.0f); gl.Vertex(0.1f, 0.1f, 0.0f);
            gl.TexCoord(0.0f, 0.0f); gl.Vertex(-0.1f, 0.1f, 0.0f);
            gl.End();

            gl.PopMatrix();

            gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.Disable(OpenGL.GL_BLEND);
        }
    }
}
