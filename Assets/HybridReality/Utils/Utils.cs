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
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NEEEU.HybridReality{
	public class Utils{
		public static Matrix4x4 GetTRMatrix(Matrix4x4 matrix){
			Vector3 position = matrix.GetColumn(3);
			Quaternion rotation = GetRotation(matrix);
			return Matrix4x4.TRS(position,rotation,Vector3.one);
		}
		public static Vector3 GetPosition(Matrix4x4 matrix)
		{
			Vector3 position = matrix.GetColumn(3);
			return position;
		}
		public static Quaternion GetRotation(Matrix4x4 m) {
			// Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
			Quaternion q = new Quaternion();
			q.w = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] + m[1,1] + m[2,2] ) ) / 2; 
			q.x = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] - m[1,1] - m[2,2] ) ) / 2; 
			q.y = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] + m[1,1] - m[2,2] ) ) / 2; 
			q.z = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] - m[1,1] + m[2,2] ) ) / 2; 
			q.x *= Mathf.Sign( q.x * ( m[2,1] - m[1,2] ) );
			q.y *= Mathf.Sign( q.y * ( m[0,2] - m[2,0] ) );
			q.z *= Mathf.Sign( q.z * ( m[1,0] - m[0,1] ) );
			return q;
		}
		//Get an average (mean) from more then two quaternions (with two, slerp would be used).
		//Note: this only works if all the quaternions are relatively close together.
		//Usage: 
		//-Cumulative is an external Vector4 which holds all the added x y z and w components.
		//-newRotation is the next rotation to be added to the average pool
		//-firstRotation is the first quaternion of the array to be averaged
		//-addAmount holds the total amount of quaternions which are currently added
		//This function returns the current average quaternion
		public static Quaternion AverageQuaternion(ref Vector4 cumulative, Quaternion newRotation, Quaternion firstRotation, int addAmount){
		
			float w = 0.0f;
			float x = 0.0f;
			float y = 0.0f;
			float z = 0.0f;
		
			//Before we add the new rotation to the average (mean), we have to check whether the quaternion has to be inverted. Because
			//q and -q are the same rotation, but cannot be averaged, we have to make sure they are all the same.
			if(!AreQuaternionsClose(newRotation, firstRotation)){
		
				newRotation = InverseSignQuaternion(newRotation);	
			}
		
			//Average the values
			float addDet = 1f/(float)addAmount;
			cumulative.w += newRotation.w;
			w = cumulative.w * addDet;
			cumulative.x += newRotation.x;
			x = cumulative.x * addDet;
			cumulative.y += newRotation.y;
			y = cumulative.y * addDet;
			cumulative.z += newRotation.z;
			z = cumulative.z * addDet;		
		
			//note: if speed is an issue, you can skip the normalization step
			return NormalizeQuaternion(x, y, z, w);
		}
		
		public static Quaternion NormalizeQuaternion(float x, float y, float z, float w){
		
			float lengthD = 1.0f / (w*w + x*x + y*y + z*z);
			w *= lengthD;
			x *= lengthD;
			y *= lengthD;
			z *= lengthD;
		
			return new Quaternion(x, y, z, w);
		}
		
		//Changes the sign of the quaternion components. This is not the same as the inverse.
		public static Quaternion InverseSignQuaternion(Quaternion q){
		
			return new Quaternion(-q.x, -q.y, -q.z, -q.w);
		}
		
		//Returns true if the two input quaternions are close to each other. This can
		//be used to check whether or not one of two quaternions which are supposed to
		//be very similar but has its component signs reversed (q has the same rotation as
		//-q)
		public static bool AreQuaternionsClose(Quaternion q1, Quaternion q2){
		
			float dot = Quaternion.Dot(q1, q2);
		
			if(dot < 0.0f){
		
				return false;					
			}
		
			else{
		
				return true;
			}
		}
	}
}