using System.Drawing;
using BulletSharp.Math;
using Gamengine.core.render;
using log4net;
using log4net.Config;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace Gamengine.core  {
	public class Engine : GameWindow {

		public float FrameTime;
		private readonly InputSystem _inputSystem = new();
		private int fps;

		// START MOVETO Camera
		float speed = 1.5f;
		private Matrix4 view;
		Vector3 position = new Vector3(0.0f, 0.0f,  3.0f);
		Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
		Vector3 up = new Vector3(0.0f, 1.0f,  0.0f);
		// END MOVETO

		public Engine(string[] args) : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
			BasicConfigurator.Configure();
		}

		protected override void OnUpdateFrame(FrameEventArgs evt) {
			// update how much ticks we lost since last update
			FrameTime = (float) evt.Time;

			_inputSystem.CheckInput(this, KeyboardState);
			// if ( _world != null ) _world.Update( FrameTime );
			
			// START MOVETO camera/InputSystem
			if (!IsFocused) { // check to see if the window is focused
				return;
			}

			if (KeyboardState.IsKeyDown(Keys.W)) {
				position += front * speed * FrameTime; //Forward 
			}

			if (KeyboardState.IsKeyDown(Keys.S)) {
				position -= front * speed * FrameTime; //Backwards
			}

			if (KeyboardState.IsKeyDown(Keys.A)) {
				position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * FrameTime; //Left
			}

			if (KeyboardState.IsKeyDown(Keys.D)) {
				position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * FrameTime; //Right
			}

			if (KeyboardState.IsKeyDown(Keys.Space)) {
				position += up * speed * FrameTime; //Up 
			}

			if (KeyboardState.IsKeyDown(Keys.LeftShift)) {
				position -= up * speed * FrameTime; //Down
			} 
			
			view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f), 
				new Vector3(0.0f, 0.0f, 0.0f),
				new Vector3(0.0f, 1.0f, 0.0f));
			// END MOVETO

			base.OnUpdateFrame(evt);
		}

		public void Destroy() {
			DestroyWindow();
		}
		
		public InputSystem GetInputSystem() {
			return _inputSystem;
		}
		
		// TODO: ABSTRACT
		// protected override void OnRenderFrame(FrameEventArgs evt) {
		// 	FrameTime += (float) evt.Time;
		// 	fps++;
		// 	if (FrameTime >= 1) {
		// 		FrameTime = 0;
		// 		Title = "Gamegine demo " + fps + " fps";
		// 		fps = 0;
		// 	}
		// }

	}
}