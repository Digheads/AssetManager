   M S B A M L   `   `   `  ���� G  CAssetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:AssetManager.CommonAssetManager.Common      AssetManager.Pages.Loops   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     3bored!clr-namespace:AssetManager.Common     .���    ContentRoot#��[  .+��   � ColumnDefinitions  �� $	��300����  � RowDefinitions �� $��20 ��� $	��300���� $��30 �.+��� -    ��Name$ 	LoopTypes��$��0��$��0���� -    ��Name$ 	LoopsList��$��0��$��1����. $
��Auto=�$��Hidden=�$��	0,10,0,10q���  �� Orientation$ 
Horizontal=�$��2��.+��� -   $��
ImportLoop��$��Import Loop��$��0,0,5,0q��� -   $��
DeleteLoop��$��Delete Loop��{�.  � $��2��$��1��$��10q� �� �� �� $
��auto ���   �� �� .+��� -    ��Name$ CurrentPosition��$
��Left=�$��Bottom=�$��5q��� -   $ Duration��$��1��$��Right=�$��Bottom=�$��5q��� -    ��Name$ TimeLine�� �� IsMoveToPointEnabled . {�.  $��1��$��2��$��Stretch=�$��Bottom=��� -    ��Name$ 
PlayButton��{�.  $��2��$
��Play��$	��Top=�$��Right=�$��5q��� -	   $ 
StopButton��{�.  $��2��$��1��$
��Stop��$	��Top=�$
��Left=�$��5q��� -
   	 ��Name$	 	IsLooping��{�.  $��3��$��2��$��Auto replay��$��Center=�$	��Top=�$��5q��� -   $
 Tipp��$��1��$��1��$��Center=�$��Center=��� -   $ Export��{�.  $��Center=�$��Stretch=�$��1��$��2��$��Export��$��	10,0,10,0q�