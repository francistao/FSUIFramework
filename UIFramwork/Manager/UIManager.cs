using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager{
	//单例模式的核心
	//1.定义一个静态的对象，在外界访问 在内部构造
	//2.构造方法私有化
	private static UIManager _instance;

	public static UIManager Instance{
		get{ 
			if (_instance == null) {
				_instance = new UIManager ();
			}
			return _instance;
		}
	}

	private Transform canvasTransform;
	private Transform CanvasTransform{
		get{ 
			if (canvasTransform == null) {
				canvasTransform = GameObject.Find ("Canvas").transform;
			}
			return canvasTransform;
		}
		set{ 
		}
	}

	private Dictionary<UIPanelType, string> panelPathDict;  //存储所有的面板Prefeb的路径
	private Dictionary<UIPanelType, BasePanel> panelDict;   //保存所有面板的游戏物体身上的BasePanel组件
	private Stack<BasePanel> panelStack;

	private UIManager(){
		ParseUIPanelTypeJson ();
	}

	//把某个页面入栈，把某个页面显示在界面上
	public void PushPanel(UIPanelType panelType){
		if (panelStack == null) {
			panelStack = new Stack<BasePanel> ();
		}
		//判断一下栈里面是否有页面
		if(panelStack.Count > 0){
			BasePanel topPanel = panelStack.Peek ();
			topPanel.OnPause ();
		}

		//关闭栈顶页面的显示
		BasePanel panel = GetPanel (panelType);
		panel.OnEnter ();
		panelStack.Push (panel);
	}
	//根据面板类型，得到实例化的面板
	public void PopPanel(){
		if (panelStack == null) {
			panelStack = new Stack<BasePanel> ();
		}

		if (panelStack.Count <= 0) {
			return;
		}

		BasePanel topPanel = panelStack.Pop ();
		topPanel.OnExit ();

		if (panelStack.Count <= 0) {
			return;
		}

		BasePanel topPanel2 = panelStack.Peek ();
		topPanel2.OnResume ();
	}

	private BasePanel GetPanel(UIPanelType panelType){
		if (panelDict == null) {
			panelDict = new Dictionary<UIPanelType, BasePanel> ();
		}
//		BasePanel panel;
//		panelDict.TryGetValue (panelType, out panel);

		BasePanel panel = panelDict.TryGet (panelType);
		if (panel == null) {
			//如果找不到，那么就找这个面板的prefeb 的路径，然后根据 prefeb 的路径去实例化面板
//			string path;
//			panelPathDict.TryGetValue (panelType, out path);
			string path = panelPathDict.TryGet(panelType);
			GameObject instPanel = GameObject.Instantiate (Resources.Load (path)) as GameObject;
			instPanel.transform.SetParent (CanvasTransform, false);
			panelDict.Add (panelType, instPanel.GetComponent<BasePanel> ());
			return instPanel.GetComponent<BasePanel> ();
		} else {
			return panel;
		}

	}
	[Serializable] 
	class UIPanelTypeJson{
		public List<UIPanelInfo> infoList;

	}

	private void ParseUIPanelTypeJson(){
		panelPathDict = new Dictionary<UIPanelType, string> ();
		TextAsset ta = Resources.Load<TextAsset> ("UIPanelType");
		UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson> (ta.text);

		foreach (UIPanelInfo info in jsonObject.infoList) {
			panelPathDict.Add (info.panelType, info.path);
		}
	}

	public void Test(){
		string path;
		panelPathDict.TryGetValue (UIPanelType.Knapsack, out path);
		Debug.Log (path);
	}
}
