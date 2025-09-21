using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using System;

public class FlashLightBridge {


	#if UNITY_IOS
		[DllImport("__Internal")]
		private static extern void _switchOnFlashLight();
		public static void switchOnFlashLight(){
			_switchOnFlashLight();
		}

		[DllImport("__Internal")]
		private static extern void _switchOffFlashLight();
		public static void switchOffFlashLight(){
			_switchOffFlashLight();
		}

		[DllImport("__Internal")]
		private static extern void _toggleFlashLight();
		public static void toggleFlashLight(){
			_toggleFlashLight();
		}

		[DllImport("__Internal")]
		private static extern void _checkFlashLightStatus();
		public static void checkFlashLightStatus(){
			_checkFlashLightStatus();
		}

		[DllImport("__Internal")]
		private static extern void _setCallBackMethod(string msgReceivingGameObjectName,string msgReceivingMethodName);
		public static void setCallBackMethod(string msgReceivingGameObjectName,string msgReceivingMethodName)
		{
			if (Application.isEditor) {
				return;
			}
			#if UNITY_IPHONE
				_setCallBackMethod( msgReceivingGameObjectName, msgReceivingMethodName);
			#endif
		}
	#endif

	#if UNITY_ANDROID
		static AndroidJavaClass _class;
		static AndroidJavaObject instance { get { return _class.GetStatic<AndroidJavaObject>("instance"); } }

		public static void SetupPlugin () {
			if (_class == null) {
				_class = new AndroidJavaClass ("mayankgupta.com.audioPlugin.FlashLightPlugin");
				_class.CallStatic ("_initiateFragment");
			}
		}

		public static void switchOnFlashLight(){
			SetupPlugin ();
			instance.Call("switchOnFlashLight");
		}

		public static void switchOffFlashLight(){
			SetupPlugin ();
			instance.Call("switchOffFlashLight");
		}

		public static void toggleFlashLight(){
			SetupPlugin ();
			instance.Call("toggleFlashlight");
		}

		public static void checkFlashLightStatus(){
			SetupPlugin ();
			instance.Call("checkFlashlightStatus");
		}

		public static void setCallBackMethod(string msgReceivingGameObjectName,string msgReceivingMethodName)
		{
			if (Application.isEditor) {
				return;
			}
			SetupPlugin ();
			instance.Call("_setUnityGameObjectNameAndMethodName", msgReceivingGameObjectName, msgReceivingMethodName);
		}
	#endif
}
