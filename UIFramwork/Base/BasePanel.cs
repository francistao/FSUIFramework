using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour {

	//界面被显示出来
	public virtual void OnEnter(){
		
	}

	//界面暂停
	public virtual void OnPause(){
		
	}

	//界面继续
	public virtual void OnResume(){
		
	}

	//界面不显示，退出这个界面，界面被关闭
	public virtual void OnExit(){
		
	}
}
