# ARGameTool

a tool for creating AR game

## 项目配置：

名称：AR游戏创作工具

项目描述：一款用来协助用户创造属于自己AR游戏的工具。

version：“1.0.0”

作者：董志昂、殷天逸、徐润石、张弛

日期：2024.6.1



## 部署需求：

本项目是一个Unity项目，需要开发者在本地克隆此库后在Unity Hub应用中打开本项目。首先开发者需要在Unity Hub中下载相应的Unity版本，本项目开发时所有开发成员统一的开发版本为Unity2022.3.20，具体部署时只需要版本号对应到Unity2022.3即可，后缀版本号（如2022.3.17f1c1,2022.3.20f1）都是可以的。

在下载相应的Unity版本后即可在Unity Hub中选择该项目进入Unity中进行编辑。注意第一次进入项目时，Unity会自动加载相关依赖，请耐心等待约5-15分钟，后续打开项目时只需要再等待30秒左右即可进入。

进入Unity后，右上角的Layout建议选择2 by 3，选择过后编辑器如图所示：

![README Picture](.\README Picture\Unity Picture.png)

下面简要介绍一下Assets中一些主要的文件夹及其内容：

MobileARTemplateAssets：该文件夹是项目创建时依赖的原始项目，该项目中包含了一个移动AR项目的模板。

Models：该文件夹包含了AR游戏物体的模型素材。

Pictures:该文件夹包含了项目中的图片素材。

Scenes：该文件夹包含了所有在开发过程中我们所建立的场景文件，最终项目的场景入口为PlayScene。

Scripts：该文件夹包含了所有脚本文件。

## 如何开发：

双击Scenes文件夹，进入其中PlayScene。左上角的Scene标签页即为场景，可以在其中对Unity中游戏体进行编辑，选中其中任意游戏体，最右侧的Inspector标签页则显示了该游戏体的信息和所挂载的组件。左下角的Game标签页则显示了在实际运行时的游戏界面。中间的Hierarchy标签页则显示了场景中所有游戏体。Hierarchy标签页右侧的Project标签页显示了项目文件结构。

我们的小组论文已经基本阐述了各自所承担的工作，可以依照我们的论文对游戏逻辑进行进一步地优化。



## 已知问题：

1.语音识别界面中语音识别算法在进行字符串比较时不能返回正确的结果，相关脚本地址：Assets/Scripts/SpeechRecognition.cs

2.手势识别功能在游玩阶段不能触发，但编辑逻辑和功能已经独立的完成。相关脚本地址：Assets/Scripts/GestureRecognition.cs

3.在编辑动画事件结束后，相关函数会报错，但报错并不影响整个游戏运行。

4.由于我们新创建的AR物体模型是自己制作的，会导致其架构不能百分百契合原始项目中自带的Ar物体模型，有时会返回报错，但报错不影响游戏运行。

5.存储模块的功能未能集成到主程序中。相关实现在Git分支store分支中。

6.对对话框游戏体进行编辑时，我们所采用的方法不够好，导致编辑对话框结束后相关UI不能及时隐藏，后续可以对该算法进行优化。相关脚本地址：Assets/Scripts/MobileARTemplateAssets/Scripts/ARTemplateMenuManager.cs中318行附近。



