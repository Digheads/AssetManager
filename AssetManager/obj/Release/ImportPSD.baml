   M S B A M L   `   `   `  ����   AssetManager    AssetManager.ImportPSD   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  
Import PSD��$	��600��$	��500�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=�.���    ContentRoot#��[   � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto ��� $��* ��� $
��auto � � ColumnDefinitions �� $
��auto ��� $��* ��� $��* �.+��� $��
PSD type: ��$
��Left=�$��Center=�$��10,0,0,0q��� $��
PSD name: ��$��1��$
��Left=�$��Center=�$��10,0,0,0q��� -    ��Name$ PSDTypes��$��1��$��2��$��5q� �� 
IsEditable . �� -    ��Name$ PSDName��$��1��$��1��$��2��$��5,5,5,5q��� -    ��Name$ 	BrowsePSD��$��
Browse PSD��$��2��$��0,5,5,5q��� -   	 ��Name$	 
BrowsedPSD��$��2��$��1��$��2��$��Center=��� -   $ BrowsePreview��$��Browse preview��$��3��$��0,5,5,5q�$	��Top=��� -   
 ��Name$
 BrowsedPreview��$��Uniform=�$��5q�$��1��$��2��$��3���� -   $ Ok��$��OK��$��4��$��1��$��5q� �� 	IsDefault . �� -   $ Cancel��$��Cancel��$��4��$��2��$��5q� �� IsCancel . 