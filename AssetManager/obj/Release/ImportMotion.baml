   M S B A M L   `   `   `  ���� N  JHelixToolkit.Wpf, Version=2014.2.1.1, Culture=neutral, PublicKeyToken=nullM8clr-namespace:HelixToolkit.Wpf;assembly=HelixToolkit.WpfHelixToolkit.Wpf  G CAssetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:AssetManager.PagesAssetManager.Pages    AssetManager.ImportMotion   Bhelix8clr-namespace:HelixToolkit.Wpf;assembly=HelixToolkit.Wpf  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     2local clr-namespace:AssetManager.Pages       � Title$  Import Motion��$	��600��$	��500�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=�.���    ContentRoot#��[   � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��* ��� $
��auto � � ColumnDefinitions �� $
��auto ��� $��* ��� $��* �.+��� $��Motion type: ��$
��Left=�$��Center=�$��10,0,0,0q��� $��Motion name: ��$��1��$
��Left=�$��Center=�$��10,0,0,0q��� -    ��Name$ MotionTypes��$��1��$��2�� �� 
IsEditable . $��5q��� -    ��Name$ 
MotionName��$��1��$��1��$��2��$��5,5,5,5q��� -    ��Name$ FilePath��$��1��$��2��$��2��$��Center=�$	��5,0q��� -   	 ��Name$	 Browse��$��Browse��$��2��$��5,5,0,5q��� 
 �� Orientation$
 
Horizontal=�$��3��$��3��$��Center=�.+�#  AssetManager.Pages.PlayerView   � DataContext �� *
Animator+&    HelixToolkit.Wpf.HelixViewport3D  -   $��viewport��   ZoomExtentsWhenLoaded . $��4��$��3��$��10q�   Children. $   HelixToolkit.Wpf.DefaultLights  �� -   $	 Ok��$��Ok��$��5��$��1��$��5q� �� 	IsDefault . �� -   $	 Cancel��$��Cancel��$��5��$��2��$��5q� �� IsCancel . 