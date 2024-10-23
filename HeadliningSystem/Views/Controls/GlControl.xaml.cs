using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenTK;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Wpf;

namespace HeadliningSystem.Views.Controls
{
    /// <summary>
    /// Interaction logic for GlControl.xaml
    /// </summary>
    public partial class GlControl : UserControl
    {
        private GLWpfControl glControl;
        private GLWpfControlSettings settings;

        public GlControl()
        {
            InitializeComponent();
            Loaded += OnLoaded;

            glControl = new GLWpfControl();
            settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 3,
                RenderContinuously = true,
                TransparentBackground = true
            };
        }

        private async void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                glControl.Start(settings);
                glControl.Render += OnRender;
            });

            Console.WriteLine("Done");
        }

        private void OnRender(TimeSpan delta)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Viewport(0, 0, (int)ActualWidth, (int)ActualHeight);

            Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, (float)ActualWidth / (float)ActualHeight, 0.1f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);

            Matrix4 modelViewMatrix = Matrix4.LookAt(
                new Vector3(0.0f, 0.0f, 5.0f),
                Vector3.Zero,               
                Vector3.UnitY               
            );
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);

            DrawScene();
        }

        private void DrawScene()
        {
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(-1.0f, -1.0f, 0.0f);

            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(1.0f, -1.0f, 0.0f);

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);
            GL.End();
        }
    }
}
