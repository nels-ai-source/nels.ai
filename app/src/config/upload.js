import API from "@/api";

//上传配置

export default {
	apiObj: API.system.file.upload,			//上传请求API对象
	filename: "file",					//form请求时文件的key
	successCode: 200,					//请求完成代码
	maxSize: 100,						//最大文件大小 默认10MB
	parseData: function (res) {
		
		return {
			code: 200,				//分析状态字段结构
			fileName: res.name,//分析文件名称
			src:'/api/system/file/preview?id='+res.id,			//分析图片远程地址结构
			msg: ''			//分析描述字段结构
		}
	},
	apiObjFile: API.system.file.upload,	//附件上传请求API对象
	maxSizeFile: 100						//最大文件大小 默认10MB
}
