/*

Copyright © 2018 NEEEU Spaces GmbH

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
﻿using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class InputHelper : MonoBehaviour {

	private static TouchCreator lastFakeTouch;

	public static List<Touch> GetTouches()
	{
		List<Touch> touches = new List<Touch>();
		touches.AddRange(Input.touches);
		#if UNITY_EDITOR
			createTouches();
			if (lastFakeTouch != null) touches.Add(lastFakeTouch.Create());
		#endif
		return touches;      
	}
	static void createTouches(){
		if(lastFakeTouch == null) lastFakeTouch = new TouchCreator();
		if(Input.GetMouseButtonDown(0)){
			lastFakeTouch.phase = TouchPhase.Began;
			lastFakeTouch.deltaPosition = new Vector2(0,0);
			lastFakeTouch.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			lastFakeTouch.fingerId = 0;
			return;
		}
		if (Input.GetMouseButtonUp(0)){
			lastFakeTouch.phase = TouchPhase.Ended;
			Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			lastFakeTouch.deltaPosition = newPosition - lastFakeTouch.position;
			lastFakeTouch.position = newPosition;
			lastFakeTouch.fingerId = 0;
			return;
		}
		if (Input.GetMouseButton(0)){
			lastFakeTouch.phase = TouchPhase.Moved;
			Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			lastFakeTouch.deltaPosition = newPosition - lastFakeTouch.position;
			lastFakeTouch.position = newPosition;
			lastFakeTouch.fingerId = 0;
			return;
		}
		lastFakeTouch = null;
		return;
	}
	public static Touch GetTouch(int fingerId){
		foreach(Touch tInst in GetTouches()){
			if(tInst.fingerId == fingerId){
				return tInst;
			}
		}
		if(lastFakeTouch == null) lastFakeTouch = new TouchCreator();
		lastFakeTouch.fingerId = -1;
		return lastFakeTouch.Create();
	}

}

public class TouchCreator{
	static BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
	static Dictionary<string, FieldInfo> fields;

	object touch;

	public float deltaTime { get { return ((Touch)touch).deltaTime; } set { fields["m_TimeDelta"].SetValue(touch, value); } }
	public int tapCount { get { return ((Touch)touch).tapCount; } set { fields["m_TapCount"].SetValue(touch, value); } }
	public TouchPhase phase { get { return ((Touch)touch).phase; } set { fields["m_Phase"].SetValue(touch, value); } }
	public Vector2 deltaPosition { get { return ((Touch)touch).deltaPosition; } set { fields["m_PositionDelta"].SetValue(touch, value); } }
	public int fingerId { get { return ((Touch)touch).fingerId; } set { fields["m_FingerId"].SetValue(touch, value); } }
	public Vector2 position { get { return ((Touch)touch).position; } set { fields["m_Position"].SetValue(touch, value); } }
	public Vector2 rawPosition { get { return ((Touch)touch).rawPosition; } set { fields["m_RawPosition"].SetValue(touch, value); } }

	public Touch Create()
	{
		return (Touch)touch;
	}
	
	public TouchCreator()
	{
		touch = new Touch();
	}

	static TouchCreator()
	{
		fields = new Dictionary<string, FieldInfo>();
		foreach(var f in typeof(Touch).GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
		{
			fields.Add(f.Name, f);
			//Debug.Log("name: " + f.Name);
		}
	}
}