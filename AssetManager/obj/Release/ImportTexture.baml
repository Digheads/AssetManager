   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem   AssetManager    AssetManager.ImportTexture   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     9System&clr-namespace:System;assembly=mscorlib       � Title$  Import Texture��$	��600��$	��500�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=�.���    ContentRoot#��[   � RowDefinitions �� $
��auto ��� $
��auto ��� $��* ��� $��* ��� $
��auto � � ColumnDefinitions �� $
��auto ��� $��* ��� $��* �.+��� $��Texture type: ��$
��Left=�$��Center=�$��10,0,0,0q��� $��Texture name: ��$��1��$
��Left=�$��Center=�$��10,0,0,0q��� -    ��Name$ TextureTypes��$��1��$��2��$��5q� �� 
IsEditable . �� -    ��Name$ TextureName��$��1��$��1��$��2��$��5,5,5,5q��� -    ��Name$ FileList��$��1��$��2��$��2��$��2����.  $
��Auto=�	 �� SelectionMode$	 Multiple=�$	��5,0q����� .R��  �� $��* ��� $
��auto �.+��� $ 	ImageType��$��1��$��30��$	��100��
 �� SelectedValue
 �� *Type+.@���Diffuse����Specular����Metallic����Bump����
Normal����
Height����	Occlusion����Emission����Opacity��9�  9�Name$ FullFilePath������ *
FullName+.���� $��Uniform=�$	��300��$��5q����� *Header+ �� ElementName$ FullFilePath���� -    ��Name$ AddFiles��$��	Add files��$��2��$��5,5,0,5q��� -   $ RemoveFiles��$��Remove files��$��3��$��5,5,0,5q��� -   $ Ok��$��Ok��$��4��$��1��$��5q� �� 	IsDefault . �� -   $ Cancel��$��Cancel��$��4��$��2��$��5q� �� IsCancel . 