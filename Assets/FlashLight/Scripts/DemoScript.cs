using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  

public enum FlashLight_Options 
{
  switchOnFlashLight,
  switchOffFlashLight,
  toggleFlashLight,
  checkFlashLightStatus
}

public class DemoScript : MonoBehaviour {
	public FlashLight_Options myOption;
	private string callbackGameObjectName = "UnityReceiveMessage";
  private string callbackMethodName = "statusMessage";

  private void Awake()
  {
	  FlashLightBridge.SetupPlugin();
  }

  /*void OnMouseUp() {
    	StartCoroutine(BtnAnimation());
 	}

 	private IEnumerator BtnAnimation()
    {
    	Vector3 originalScale = gameObject.transform.localScale;
        gameObject.transform.localScale = 0.9f * gameObject.transform.localScale;
        yield return new WaitForSeconds(0.2f);
        gameObject.transform.localScale = originalScale;
        ButtonAction();
    }

    private void ButtonAction() {
    	FlashLightBridge.setCallBackMethod(callbackGameObjectName, callbackMethodName);
			switch(myOption) {
		    case FlashLight_Options.switchOnFlashLight:
		      FlashLightBridge.switchOnFlashLight();
		      break;
		    case FlashLight_Options.switchOffFlashLight:
		    	FlashLightBridge.switchOffFlashLight();
		      break;
		    case FlashLight_Options.toggleFlashLight:
		    	FlashLightBridge.toggleFlashLight();
		      break;
		    case FlashLight_Options.checkFlashLightStatus:
		    	FlashLightBridge.checkFlashLightStatus();
		      break;
			}
    }*/

    public void Toggle()
    {
	    FlashLightBridge.setCallBackMethod(callbackGameObjectName, callbackMethodName);
	    FlashLightBridge.toggleFlashLight();
    }
}
