using System;
using System.Collections.Generic;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Gamengine.core {
	public class InputSystem {

		private readonly Dictionary<Keys, List<Action<Engine>>> _controls = new();
		private readonly Dictionary<Action<Engine>, int> _delays = new();
		private readonly Dictionary<Action<Engine>, int> _tickedDelays = new();

		public void RegisterInput(Keys key, Action<Engine> callback) {
			if (!_controls.ContainsKey(key)) _controls[key] = new List<Action<Engine>>();
			_controls[key].Add(callback);
		}
		
		public void RegisterInput(Keys key, Action<Engine> callback, int minDelay) {
			if (!_controls.ContainsKey(key)) _controls[key] = new List<Action<Engine>>();
			_controls[key].Add(callback);
			_delays[callback] = minDelay;
		}
		
		public void CheckInput(Engine engine, KeyboardState state) {

			foreach (Keys key in _controls.Keys) {
				foreach (Action<Engine> callback in _controls[key]) {

					if ( _tickedDelays.GetValueOrDefault(callback, 0) <= 0) {
						if (state.IsKeyDown(key)) {
							callback.Invoke(engine);
							_tickedDelays[callback] = _delays.GetValueOrDefault(callback, 0);
						}
					} else {
						_tickedDelays[callback] = _tickedDelays[callback] - 1;
					}
				}
			}
		}
		
	}
}