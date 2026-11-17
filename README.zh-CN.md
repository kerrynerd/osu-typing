# osu-typing 简体中文版本

本分支基于 [powwu/osu-typing](https://github.com/powwu/osu-typing) 的 `c6a6ea3`，将客户端界面改为简体中文。该代码基于 2019 年的 osu!lazer。

这是固定简体中文分支，暂不提供界面语言切换。设置中的“优先显示歌曲信息的原始语言”仅控制曲目元数据的显示。

## 汉化范围

- 主菜单、选曲、谱面信息、模组选项、暂停、失败、回放和结算。
- 设置、快捷键及各规则集的按键说明。
- 账号、聊天、个人资料、谱面下载、通知和多人房间。
- 编辑器、比赛控制界面及动态数量、日期、状态文字。

曲名、玩家名、聊天内容、服务器返回的正文、皮肤图片内的文字保持原样。osu!、BPM、PP 和模组缩写等名称保留原文。接口字段、资源路径、配置枚举标识和数值保持兼容。

## 在 Windows 上构建

本分支已使用 .NET SDK 10.0.302 构建。项目目标仍为原有的 `netcoreapp2.2` / `netstandard2.0`，没有升级依赖或迁移游戏框架。

```powershell
git clone --branch localization/zh-CN https://github.com/kerrynerd/osu-typing.git
cd osu-typing
dotnet build osu.Desktop/osu.Desktop.csproj -c Release
dotnet publish osu.Desktop/osu.Desktop.csproj -c Release -r win-x64 --self-contained true -p:RuntimeFrameworkVersion=2.2.8 -o ../osu-typing-zh-CN-win-x64
```

运行发布目录中的 `osu!.exe`。必须保留整个目录，不能只复制可执行文件；该目录包含所需运行时和资源。

原项目使用的 .NET Core 2.2 已停止支持，构建仍会报告旧运行时及部分依赖的已知漏洞警告。汉化分支未处理这些原有依赖问题。

## 验证与试玩

新增的 `ChineseDisplayTest` 检查下拉菜单显示、日期本地化、接口枚举值与进程区域设置。使用新版本运行时执行这些无窗口测试时，可在 PowerShell 中运行：

```powershell
$env:DOTNET_ROLL_FORWARD = 'Major'
dotnet test osu.Game.Tests/osu.Game.Tests.csproj -c Release --filter FullyQualifiedName~ChineseDisplayTest
```

编译和自动检查不等于实际游玩验证。请在提交上游 PR 前检查：

1. 主菜单、设置、选曲页是否有缺字、文字截断或重叠；切换窗口模式和界面缩放后再看一次。
2. 导入熟悉的谱面，试玩打字功能，检查按键、暂停、重试、结算和回放。
3. 检查数字、时间和谱面原文显示；按需试用编辑器及在线界面。

目前未验证账号登录、下载或多人服务与当前服务器的兼容性，也未完成实际游玩及界面布局检查。

原项目的开发与许可证说明见 [README.md](README.md) 和 [LICENCE](LICENCE)。
