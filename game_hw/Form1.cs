using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Media;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using SharpGL.WPF;

namespace game_hw
{
    public partial class Form1 : Form
    {
        OpenGL gl;

        Texture characterTexture = new Texture();
        Texture enemyTexture = new Texture();

        List<Bullet> bullets = new List<Bullet>();
        List<Enemy> enemies = new List<Enemy>();

        Random rand = new Random();

        Timer enemyTimer = new Timer();

        int health = 3;
        int level = 1;
        int enemySpawnInterval = 3000;
        int kills = 0;

        float mouseX, mouseY;
        int currentBulletColor = 0;

        static float[] redColor = {1.0f, 0.0f,  0.0f};
        static float[] greenColor = { 0.0f, 1.0f, 0.0f };
        static float[] blueColor = { 0.0f, 0.0f, 1.0f };
        static float[] yellowColor = { 0.0f, 1.0f, 1.0f };
        float[][] colors = {redColor,  greenColor, blueColor, yellowColor};

        bool isAlive = true;

        SoundPlayer Player = new SoundPlayer();

        public Form1()
        {
            InitializeComponent();

            this.label3.Text = $"Health: {health}";
            this.label2.Text = $"Level: {level}";

            this.Player.SoundLocation = "./Files/muzon.wav";
            this.Player.PlayLooping();

            enemyTimer.Interval = enemySpawnInterval;
            enemyTimer.Tick += new EventHandler(EnemyTimer_Tick);
            enemyTimer.Start();
        }

        private void EnemyTimer_Tick(object sender, EventArgs e)
        {
            generateEnemies();
        }

        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            gl = openGLControl1.OpenGL;

            characterTexture.Create(gl, "./Files/ruds.png");
            enemyTexture.Create(gl, "./Files/test.png");
        }

        private void openGLControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {
            if (!isAlive) return;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();
            gl.Translate(0.0f, 0.0f, -6.0f);

            renderPlayer();
            renderEnemies();
            renderBullets();
        }

        private void openGLControl1_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = (float)((float)e.X - openGLControl1.Width / 2) / 90;
            mouseY = (float)((float)e.Y - openGLControl1.Height / 2) / -90;
        }

        private void openGLControl1_MouseClick(object sender, MouseEventArgs e)
        {
            Bullet bullet = new Bullet(colors[currentBulletColor], mouseX, mouseY, currentBulletColor);
            bullets.Add(bullet);
        }

        private void generateEnemies()
        {
            float x = GenerateRandomBetweenRanges(-4.0f, -1.5f, 1.5f, 4.0f);
            float y = GenerateRandomBetweenRanges(-2.5f, -1.5f, 1.5f, 2.5f);

            List<int> shields = new List<int>();

            for (int i = 0; i < 3; ++i)
            {
                shields.Add(rand.Next(0, 4));
            }

            Enemy enemy = new Enemy(x, y, shields);
       
            enemies.Add(enemy);
        }

        private void openGLControl1_KeyDown(object sender, KeyEventArgs e)
        {
            string pressedKey = e.KeyData.ToString();
            
            switch (pressedKey)
            {
                case "D1":
                    //label2.Text = "Red disc";
                    currentBulletColor = 0;
                    break;
                case "D2":
                    //label2.Text = "Green disc";
                    currentBulletColor = 1;
                    break;
                case "D3":
                    //label2.Text = "Blue disc";
                    currentBulletColor = 2;
                    break;
                case "D4":
                    //label2.Text = "Yellow disc";
                    currentBulletColor = 3;
                    break;
                default:
                    break;
            }
        }

        private float GenerateRandomBetweenRanges(float minRange1, float maxRange1, float minRange2, float maxRange2)
        {
            if (rand.Next(2) == 0)
            {
                return (float)(rand.NextDouble() * (maxRange1 - minRange1) + minRange1);
            }
            else
            {
                return (float)(rand.NextDouble() * (maxRange2 - minRange2) + minRange2);
            }
        }

        private void renderPlayer()
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            characterTexture.Bind(gl);

            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(1.0f, 1.0f, 1.0f);

            gl.TexCoord(0.0f, 1.0f); gl.Vertex(-0.25f, -0.25f, 0.0f);
            gl.TexCoord(1.0f, 1.0f); gl.Vertex(0.25f, -0.25f, 0.0f);
            gl.TexCoord(1.0f, 0.0f); gl.Vertex(0.25f, 0.25f, 0.0f);
            gl.TexCoord(0.0f, 0.0f); gl.Vertex(-0.25f, 0.25f, 0.0f);


            gl.End();

            gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        private void renderEnemies()
        {
            for (int i = 0; i < enemies.Count; ++i)
            {
                var enemy = enemies[i];

                if (enemy.isAlive)
                {
                    enemy.calculatePosition();

                    gl.Enable(OpenGL.GL_TEXTURE_2D);
                    enemyTexture.Bind(gl);

                    gl.Begin(OpenGL.GL_QUADS);
                    gl.Color(1.0f, 1.0f, 1.0f);
                    gl.TexCoord(0.0f, 1.0f); gl.Vertex(-0.25f + enemy.positionX, -0.25f + enemy.positionY, 0.0f);
                    gl.TexCoord(1.0f, 1.0f); gl.Vertex(0.25f + enemy.positionX, -0.25f + enemy.positionY, 0.0f);
                    gl.TexCoord(1.0f, 0.0f); gl.Vertex(0.25f + enemy.positionX, 0.25f + enemy.positionY, 0.0f);
                    gl.TexCoord(0.0f, 0.0f); gl.Vertex(-0.25f + enemy.positionX, 0.25f + enemy.positionY, 0.0f);

                    gl.End();

                    gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
                    gl.Disable(OpenGL.GL_TEXTURE_2D);

                    for (int j = 0; j < enemy.shields.Count; ++j)
                    {
                        float shieldOffsetY = 0.5f;
                        float shieldGap = 0.25f;
                        float shieldOffsetX = (j - (enemy.shields.Count - 1) / 2.0f) * shieldGap;

                        gl.Color(colors[enemy.shields[j]]);

                        gl.Begin(OpenGL.GL_QUADS);
                        gl.Vertex(-0.1f + enemy.positionX + shieldOffsetX, -0.1f + enemy.positionY + shieldOffsetY, 0.0f);
                        gl.Vertex(0.1f + enemy.positionX + shieldOffsetX, -0.1f + enemy.positionY + shieldOffsetY, 0.0f);
                        gl.Vertex(0.1f + enemy.positionX + shieldOffsetX, 0.1f + enemy.positionY + shieldOffsetY, 0.0f);
                        gl.Vertex(-0.1f + enemy.positionX + shieldOffsetX, 0.1f + enemy.positionY + shieldOffsetY, 0.0f);
                        gl.End();
                    }

                    if (enemy.positionX >= -0.25f && enemy.positionX <= 0.25f &&
                        enemy.positionY >= -0.25f && enemy.positionY <= 0.25f)
                    {
                        health--;
                        enemy.isAlive = false;
                        label3.Text = $"Health: {health}";
                        if (health <= 0)
                        {
                            enemyTimer.Stop();
                            isAlive = false;
                            gameOverDialog();
                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void renderBullets()
        {
            for (int i = 0; i < bullets.Count; ++i)
            {
                var bullet = bullets[i];

                bullet.updatePosition();

                gl.PointSize(2);
                gl.Begin(SharpGL.Enumerations.BeginMode.Points);
                gl.Color(bullet.color);
                gl.Vertex(bullet.positionX, bullet.positionY, 0.0f);
                gl.End();

                int originalX = (int)Math.Round((bullets[i].positionX * 90) + openGLControl1.Width / 2);
                int originalY = (int)Math.Round((bullets[i].positionY * -90) + openGLControl1.Height / 2);
                if (originalY < 0 || originalY > openGLControl1.Height || originalX < 0 || originalX > openGLControl1.Width)
                {
                    bullets.RemoveAt(i);
                    i--;
                    break;
                }

                for (int j = 0; j < enemies.Count; ++j)
                {
                    var enemy = enemies[j];

                    if (enemy.isAlive)
                    {
                        float enemyLeft = enemy.positionX - 0.25f;
                        float enemyRight = enemy.positionX + 0.25f;
                        float enemyBottom = enemy.positionY - 0.25f;
                        float enemyTop = enemy.positionY + 0.25f;

                        // Bullet hits enemy
                        if (bullets[i].positionX >= enemyLeft && bullets[i].positionX <= enemyRight &&
                            bullets[i].positionY >= enemyBottom && bullets[i].positionY <= enemyTop && enemy.shields[0] == bullet.colorID)
                        {
                            bullets.RemoveAt(i);
                            i--;
                            enemy.shields.RemoveAt(0);
                            if (enemy.shields.Count == 0)
                            {
                                enemy.isAlive = false;
                                kills += 1;
                                if (kills >= 2 * level)
                                {
                                    kills = 0;
                                    level += 1;
                                    enemySpawnInterval -= 250 * level;
                                    label2.Text = $"Current level: {level}";
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void gameOverDialog()
        {
            DialogResult result = MessageBox.Show("Game Over! Do you want to try again?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                restartGame();
            }
            else
            {
                this.Close();
            }
        }

        private void restartGame()
        {
            health = 3;
            level = 1;
            kills = 0;
            enemySpawnInterval = 3000;
            label3.Text = $"Health: {health}";
            label2.Text = $"Level: {level}";

            enemyTimer.Interval = enemySpawnInterval;
            enemyTimer.Start();

            bullets.Clear();
            enemies.Clear();

            isAlive = true;
        }

    }
}
