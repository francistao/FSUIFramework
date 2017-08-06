using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtension{   //对字典类的扩展

	//尝试根据key得到value，得到了的话直接返回value，没有直接得到返回null
	//this Dictionary<Tkey, Tvalue> dict 这个字典表示我们要获取值的字典
	public static Tvalue TryGet<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key){
		Tvalue value;
		dict.TryGetValue (key, out value);
		return value;
	}
}
