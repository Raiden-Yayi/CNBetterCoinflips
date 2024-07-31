# BetterCoinflips
  
SCP：SL插件，为游戏内硬币添加风险奖励机制。每当您抛硬币时，都会发生“随机”效应，具体取决于抛硬币的结果。

特色：
每当玩家掷硬币并落在头上时，都会发生以下情况之一：
他们将收到一张收容工程师/设施经理钥匙卡。
他们将收到一个由医疗包和止痛药组成的“医疗包”。
他们将被传送到逃生区的门。
他们将被 25 点生命值治愈。
他们的生命值将增加10%。
他们会得到一个SCP-268。
他们将在 5 秒内随机获得良好的效果。
他们将获得一个带有 1 发弹药的逻辑师。
他们将收到一个SCP-2176。
他们将收到一颗粉红色的糖果。
他们将收到一把带有最差附件的左轮手枪。
他们将得到一个空的微隐藏。
其比例将设置为 1.3/0.5/1.3。
他们将收到一个随机物品。

每当有人抛硬币并落在硬币反面时，就会发生以下情况之一：
他们的生命值将减少30%。
他们将被传送到D类牢房。
他们将在 5 秒内随机产生不良效果。
地图上的所有灯光将关闭 10 秒钟。
一枚实弹手榴弹将出现在他们的头上。
一枚实弹闪光手榴弹将在他们的头上生成。
如果有活着的人，他们将被传送到SCP上，否则他们将失去15点生命值。
他们将失去除 1 马力之外的所有生命值。
玩家将收到一个已启动的SCP-244。
他们拉屎了。
他们的库存将被重置。
他们的角色将更改为相反的角色（d 类 - 科学家、MTF - CI 等）
一枚立即爆炸的手榴弹将落在他们的头上。
他们将与其他玩家交换位置。
他们会被踢。
他们将被随机观众取代。
他们将被传送到一个随机的特斯拉上。
他们的库存将与其他玩家的库存交换。
他们将被传送到一个随机的房间。
他们将被戴上手铐并丢失他们的物品。
该插件将阻止在地图周围生成指定数量的硬币。
该插件将用SCP基座中的一枚硬币替换指定数量的选定物品（默认为SCP-500）。
该插件将为每个投掷的硬币分配随机数量的使用。这个数量可以用命令读取或设置。如果一枚硬币用完了，它就会破裂。

命令
GetSerial - 获取您或其他玩家持有的物品的序列号。
CoinUses - 获取或设置特定硬币的使用次数。用法示例：、、coinuses get player 5coin uses set player 4coinuses set serial 10

权限
bc.coinuses.set - 授予对 CoinUses Set 命令的访问权限
bc.coinuses.get - 授予对 CoinUses Get 命令的访问权限

该插件由BetterCoinflips改编而来。
