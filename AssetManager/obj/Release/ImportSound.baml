   M S B A M L   `   `   `  ����   AssetManager    AssetManager.ImportSound   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Import Sound��$	��600��$	��350�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=�.���    ContentRoot#��[   � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto ��� $��* ��� $
��auto � � ColumnDefinitions �� $
��auto ��� $��* ��� $��* �.+��� $��Sound type: ��$
��Left=�$��Center=�$��10,0,0,0q��� $��Sound name: ��$��1��$
��Left=�$��Center=�$��10,0,0,0q��� -    ��Name$ 
SoundTypes��$��1��$��2�� �� 
IsEditable . $��5q��� -    ��Name$ 	SoundName��$��1��$��1��$��2��$��5,5,5,5q��� -    ��Name$ BrowseSound��$��Browse sound��$��2��$��0,5,5,5q��� -   	 ��Name$	 BrowsedSound��$��2��$��1��$��2��$��Center=�� $��3��$��3��$��10q� �� �� �� $
��auto ���  �� �� .+��� -   $	 CurrentPosition��$
��Left=�$��Bottom=�$��5q��� -   $	 Duration��$��1��$��Right=�$��Bottom=�$��5q��� -   
 ��Name$
 TimeLine�� �� IsMoveToPointEnabled . {�.  $��1��$��2��$��Stretch=�$��Bottom=��� -   $ 
PlayButton��{�.  $��2��$
��Play��$	��Top=�$��Right=�$��5q��� -	   $ 
StopButton��{�.  $��2��$��1��$
��Stop��$	��Top=�$
��Left=�$��5q��� -
    ��Name$ 	IsLooping��{�.  $��3��$��2��$��Auto replay��$��Center=�$	��Top=�$��5q��� -   $ Ok��$��OK��$��4��$��1��$��5q� �� 	IsDefault . �� -   $ Cancel��$��Cancel��$��4��$��2��$��5q� �� IsCancel . 