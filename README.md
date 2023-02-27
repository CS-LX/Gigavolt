# GigaVoltage
## 简介 Introduction
这是一个为生存战争游戏带来十亿伏特电力系统的mod，将原版的16个电压级别（0\~1.5V）扩展到2^32个（0\~2^32-1V）

This is a mod for Survivalcraft that take a new Electric system with Gigavolt to the game. The original Electric system has 16 voltage level(0\~1.5V), then Gigavolt expands it to 2^32 voltage level(0\~2^32-1V).
## 区别 Differences
|方块|原版|十亿伏特版|
|--|--|--|
|SR锁存器|S端高压才储存|S端非0即储存|
|开关、按钮、电池|默认输出1.5V|默认输出0xFFFFFFFF V，其他原本默认输出1.5V的也改为默认输出此电压，不提供经典版|
|计数器|默认溢出电压为0x10V，上限为0xF V|默认溢出电压为0V，上限0xFFFFFFFF V，可设置溢出电压|
|真值表|略|详见[说明-真值表](#真值表)|
|存储器|略|详见[说明-存储器](#存储器)|
|压力板|有压力时输出0.8\~1.5V，随意设置的压力与电压关系|输出准确的压力值，参考16进制结果：男性玩家0x46V，虎鲸0x5DC V|
|活塞|0.8V开始伸长|1V开始伸长，提高了伸长、黏住、速度上限（黏住过多时卡住是游戏其他子系统问题），不提供经典版|
|彩色LED、1面LED|0.8\~1.5V对应不同颜色|所有电压对应不同颜色（ABGR格式）。1面LED使用独立的新发光子系统，支持半透明，可无缝拼接|
|靶子|输出0.8\~1.5V|输出0\~0xFFFFFF00 V，因游戏坐标精度问题，最低8位为0|
|电子雷管|输入0.8v及以上电压产生威力为10的爆炸|输入0v以上电压产生威力等于电压的爆炸|

## 说明
### 存储器
为存储远超原版的数据量，该Mod将数据无损保存为了[PNG](https://www.w3.org/TR/png/)图片格式，因此需要先手动设置长宽，而且之后不可在编辑界面修改，长宽上限均为2^31-1，但为了避免不可预知的错误，请不要设置如此巨大的矩阵  
编辑界面用文字编辑时，每个数据用英文逗号`,`分开，每行数据用英文分号`;`分开  
当数据量较大时，不建议继续文本编辑，而建议使用图片编辑工具手动编辑保存的png文件，保存位置在存档文件夹的GVMB文件夹下，文件名可在游戏内对着此方块点击编辑按钮查看，如果您的设备不能直接编辑存档文件，可先导出存档；除了使用图片编辑工具编辑，您也可以将其他png文件重命名后直接将其覆盖  
png要求颜色模式为24位带透明通道的RGB模式，因为游戏引擎的缘故，像素颜色会按照透明通道、蓝色、绿色、红色的顺序（即ABGR）保存进电压数据中，例如ARGB顺序的AABBCCDD像素，会被转换成AADDCCBB V的电压数据
### 真值表
此套规则的计算表达式基于[NCalc2](https://github.com/sklose/NCalc2)，写法类似于Excel，请先看具体例子：  
* `1;2;3;4:5`输入1\~4分别为1、2、3、4V时，输出5V  
* `>0x1;<=0xA;i3>1&&i3<99:0xABC`输入1大于1V，输入2小于等于十六进制A V，输入3同时满足大于1V和小于99V，输出4为任意值时，输出十六进制ABC V  
* `0;0;0;0;;i2;true;>i1:10`输入变化前输入1~4均为0V；变化后输入1等于输入2，输入2为任意值（也可以写i1，但没必要，不过因为还要设定输入3的规则，所以此处需填true），输入3大于输入1，输入4为任意值，输出10V（使用两个英文引号`;;`分开输入变化前的规则和变化后的规则，最多可以获取15次输入变化前的规则）  
* `0;;1;;2;;3:4::5:6::7:8`现在输入1为3V，输入变化前输入1为2V，再上一次输入变化前输入1为1V，再上一次输入变化前输入为0V，则现在输出4V；如果前一组规则未满足，则继续计算下一组规则`5:6` ，未满足则再计算下一组规则`7:8`（使用两个英文冒号`::`分开多套输入输出规则）  
* `true;true;Min(i1,i2),Max(i1,i2):i1+i2`输入1、2为任意值，输入3是输入1、输入2中的较小值，输入4是输入1、输入2中的较大值，则输出输入1+输入2  
#### 详细
4个输入规则之间用英文分号`;`分隔，如果只给输入1、2设定规则，输入3、4的规则可以不写，但也请不要加上多余的`;`；不能只给输入1、3设定规则，至少要给第2输入设定规则`true`  
如果输入规则的开头是`=`、`!=`、`>`、`<`，此Mod会根据是第几个输入，自动在开头加上`i1`、`i2`……  
如果输入规则不是`true`，且不包含`=`、`!`、`not`、`>`、`<`、`and`、`&&`、`||`、`or`中的任意一个操作符，此Mod会自动在开头加上`i1=`、`i2=`……  
使用两个英文引号`;;`来分隔时序，越左边时序越早，最右边的时序是现在的输入，最多取得15次变化前的输入  
只有输入规则的计算结果是`true`时，才会计算下一个输入规则，然后计算下一组时序，直到所有规则计算结果均为`true`，才会计算此组规则设定的输出  
使用英文冒号`:`来分隔所有输入规则和输出规则，输出规则的计算结果必须为自然数（>=0的整数）  
使用两个英文冒号`::`来分隔多套输入输出规则，当第一套规则不输出时，则计算下一套规则，直到有输出为止，最多2^32-1套  
过程中出现任何错误均会直接停止计算并输出0，错误详见游戏日志
> **注意：规则对大小写敏感！**

如果想了解更多，请参见[操作符](https://github.com/ncalc/ncalc/wiki/Operators)、[函数](https://github.com/ncalc/ncalc/wiki/Functions)、[值](https://github.com/ncalc/ncalc/wiki/Values)