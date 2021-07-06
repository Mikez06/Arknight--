using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
	static JsonSerializerSettings setting;
	static JsonSerializerSettings typeSerializerSetting;
	static JsonHelper()
	{
		typeSerializerSetting = new JsonSerializerSettings()
		{
			TypeNameHandling = TypeNameHandling.Auto,
		};
		//JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
		//JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
		//{
		//	//日期类型默认格式化处理
		//	//setting.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
		//	//setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

		//	//空值处理
		//	setting.NullValueHandling = NullValueHandling.Ignore;
		//	setting.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
		//	setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		//	//高级用法九中的Bool类型转换 设置
		//	//setting.Converters.Add(new BoolConvert("是,否"));

		//	return setting;
		//});
	}
	public static void Init()
	{
		// 调用这个是为了调用静态方法
	}

	public static string ToJson(object obj)
	{
		return JsonConvert.SerializeObject(obj);
	}
	public static T FromJson<T>(string str)
	{
		return JsonConvert.DeserializeObject<T>(str);
	}

	public static string ToJsonWithType(object obj)
	{
		return JsonConvert.SerializeObject(obj,typeSerializerSetting);
	}
	public static T FromJsonWithType<T>(string str)
	{
		return JsonConvert.DeserializeObject<T>(str, typeSerializerSetting);
	}

	public static object FromJson(Type type, string str)
	{
		return JsonConvert.DeserializeObject(str, type);
	}

	public static T Clone<T>(T t)
	{
		return FromJson<T>(ToJson(t));
	}
}