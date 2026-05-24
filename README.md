# EW

EW 是一个基于 BaseLib 的 Slay the Spire 2 角色模组。它加入了一名新的可玩角色、一套完整卡池、自定义能力、角色视觉资源、本地化对话，以及用于卡图、能力图标、动画和 UI 的资源包。

## 最新整合包

当前已经打包好的版本位于：

- `dist/EW-latest.zip`

安装方式：将压缩包中的 `EW` 文件夹解压到 Slay the Spire 2 的模组目录：

```text
Slay the Spire 2/mods/EW/
```

安装后的文件夹中应包含：

- `EW.dll`
- `EW.pck`
- `EW.json`

EW 依赖 BaseLib，因此也需要安装并启用 BaseLib。

## 模组简介

EW 加入了一名围绕爆炸物、魂灵之影召唤物、临时防护和卡兹戴尔牌协同构筑的新角色。角色拥有自定义红色主题色、自定义战斗与营火视觉、自定义能量计数器、角色选择语音，以及与先古之民的专属对话。

这套卡池不是简单换皮，而是包含新的攻击牌、技能牌、能力牌、生成牌、衍生牌，以及一张多人模式限定的支援牌。主要机制包括：

- 源石炸弹：给敌人附着延迟爆炸。炸弹伤害设计为独立结算，不吃普通攻击修正。
- 魂灵之影：召唤影子协助攻击、保护角色，并为部分卡牌提供成长。
- 迷彩：临时降低受到的伤害，常见于防御和召唤体系。
- 卡兹戴尔牌：一组可被特定能力和卡牌识别的协同牌，包括随机生成、阈值奖励和战斗属性收益。
- 残影：给敌人施加标记，使 EW 对被标记目标造成额外伤害。

## 卡牌与能力

当前模组包含较完整的 EW 卡池，包括基础牌、常见攻击与技能、稀有构筑核心、生成弹药、可选择消耗效果、炸弹工具、魂灵之影工具，以及卡兹戴尔牌。这里不逐张列出全部卡牌，只做概要说明：

- EW 起始牌组使用的基础攻击与防御牌
- D6/D12 等延迟爆炸牌，以及强化炸弹体系的能力牌
- 召唤、解散或根据魂灵之影攻击次数成长的牌
- 卡兹戴尔议长、卡兹戴尔的希望、佣兵团 Alpha、佣兵团 Beta 等卡兹戴尔协同牌
- 消耗手牌、生成免费能力牌、临时调整力量、直接削减生命，以及多人支援等功能牌

自定义能力用于支撑这些体系，包括炸弹倒计时与炸弹护甲、卡兹戴尔回合开始和阈值能力、魂灵之影支援能力、残影标记、下回合能量、当回合费用变化，以及临时力量回复等效果。

## 仓库结构

```text
EW.slnx                  解决方案文件
EW/                      Godot 与 C# 模组项目
EW/EW.csproj             主项目文件
EW/EW.json               模组 manifest
EW/EWCode/               C# 角色、卡牌、能力、遗物、补丁和辅助逻辑
EW/EW/                   会被打包进 EW.pck 的 Godot 资源
EW/EW/localization/      本地化文本
dist/                    已打包的发布 zip
```

## 从源码构建

需要准备：

- Slay the Spire 2
- BaseLib
- 与项目目标兼容的 .NET SDK
- 用于导出 PCK 的 MegaDot/Godot 4.5.1 mono

构建 DLL 和 manifest：

```powershell
dotnet build .\EW\EW.csproj
```

从 Godot 项目目录导出资源包：

```powershell
cd .\EW
& "D:\Tools\megadot\MegaDot_v4.5.1-stable_mono_win64.exe" --headless --export-pack "BasicExport" "E:\Steam\steamapps\common\Slay the Spire 2\mods\EW\EW.pck"
```

如果你的游戏路径或工具路径不同，请对应修改 `EW/Directory.Build.props` 和导出目标路径。

## 备注

本 README 只概括模组内容，不逐张记录所有卡牌和能力。具体效果请查看 `EW/EWCode/` 下的 C# 模型，以及 `EW/EW/localization/` 下的本地化文件。
