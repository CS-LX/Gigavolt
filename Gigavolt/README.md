# GigaVoltage
## ��� Introduction
����һ��Ϊ����ս����Ϸ����ʮ�ڷ��ص���ϵͳ��mod����ԭ���16����ѹ����0\~1.5V����չ��2^32����0\~2^32-1V��

This is a mod for Survivalcraft that take a new Electric system with Gigavolt to the game. The original Electric system has 16 voltage level(0\~1.5V), then Gigavolt expands it to 2^32 voltage level(0\~2^32-1V).
## ���� Differences
|����|ԭ��|ʮ�ڷ��ذ�|
|--|--|--|
|SR������|S�˸�ѹ�Ŵ���|S�˷�0������|
|���ء���ť�����|Ĭ�����1.5V|Ĭ�����0xFFFFFFFF V|
|������|Ĭ�������ѹΪ0x10V������Ϊ0xF V|Ĭ�������ѹΪ0V������0xFFFFFFFF V�������������ѹ|
|�洢��|��|�����ֶ����ô洢���󳤿�ſ�ʹ�ã���������Ϊ2^31-1��ÿ��������Ӣ�Ķ���,�ֿ���ÿ��������Ӣ�ľ��.�ֿ�|
|ѹ����|��ѹ��ʱ���0.8\~1.5V���������õ�ѹ�����ѹ��ϵ|���׼ȷ��ѹ��ֵ���ο�16���ƽ�����������0x46V������0x5DC V|
|��ɫLED��1��LED|0.8\~1.5V��Ӧ��ͬ��ɫ|���е�ѹ��Ӧ��ͬ��ɫ��ABGR��ʽ��|
|����|���0.8\~1.5V|���0\~0xFFFFFF00 V������Ϸ���꾫�����⣬���2λΪ0|