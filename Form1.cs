using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RayTracing
{
    public partial class Form1 : Form
    {

        private int BasicProgramID;
        private int BasicVertexShader;
        private int BasicFragmentShader;
        OpenTK.Vector3 CubeColor;
        OpenTK.Vector3 CameraPosition;
        OpenTK.Vector3 CubeCoord2;

        public Form1()
        {
            InitializeComponent();
        }

        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        void InitShaders()
        {
            BasicProgramID = GL.CreateProgram();
            loadShader("..\\..\\Shaders\\raytracing.vert", ShaderType.VertexShader, BasicProgramID, out BasicVertexShader);
            loadShader("..\\..\\Shaders\\raytracing.frag", ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);
            GL.LinkProgram(BasicProgramID);
            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));
        }

        private static bool Init()
        {
            GL.Enable(EnableCap.ColorMaterial);
            GL.ShadeModel(ShadingModel.Smooth);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            return true;
        }

        void SetUniformVec3(string name, OpenTK.Vector3 value)
        {
            GL.Uniform3(GL.GetUniformLocation(BasicProgramID, name), value);
        }


        private void Draw()
        {
            GL.ClearColor(Color.AliceBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(BasicProgramID);
            SetUniformVec3("cube_color", CubeColor);
            SetUniformVec3("camera_position", CameraPosition);
            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-1, -1);

            GL.TexCoord2(1, 1);
            GL.Vertex2(1, -1);

            GL.TexCoord2(1, 0);
            GL.Vertex2(1, 1);

            GL.TexCoord2(0, 0);
            GL.Vertex2(-1, 1);

            GL.End();
            glControl1.SwapBuffers();
            GL.UseProgram(0);
        }

       


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            CubeColor.X = trackBar1.Value / 255.0f;
            glControl1.Invalidate();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            CubeColor.Y = trackBar2.Value / 255.0f;
            glControl1.Invalidate();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            CubeColor.Z = trackBar3.Value / 255.0f;
            glControl1.Invalidate();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            CameraPosition.X = trackBar4.Value;
            glControl1.Invalidate();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            CameraPosition.Y = trackBar5.Value;
            glControl1.Invalidate();
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            CameraPosition.Z = trackBar6.Value;
            glControl1.Invalidate();
        
        }

        private void glControl1_Load_1(object sender, EventArgs e)
        {
                CubeColor.X = 255;
                CubeColor.Y = 255;
                CubeColor.Z = 255;
                CameraPosition.X = 1;
                CameraPosition.Y = 1;
                CameraPosition.Z = 1;
                Init();
                InitShaders();
            }

        private void glControl1_Paint_1(object sender, PaintEventArgs e)
        {
            Draw();
        }
    }
}
