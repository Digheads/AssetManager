   M S B A M L   `   `   `  ���� [  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms   AssetManager   AssetManager.ImportFont   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms       � Title$  Import Font��$	��600��$	��500�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=�.���    ContentRoot#��[   � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto ��� $��* ��� $
��auto � � ColumnDefinitions �� $
��auto ��� $��* ��� $��* �.+��� $��Font type: ��$
��Left=�$��Center=�$��10,0,0,0q��� $��Font name: ��$��1��$
��Left=�$��Center=�$��10,0,0,0q��� -    ��Name$ 	FontTypes��$��1��$��2�� �� 
IsEditable . $��5q��� -    ��Name$ FontName��$��1��$��1��$��2��$��5,5,5,5q��� -    ��Name$ 
BrowseFont��$��Browse Font��$��2��$��0,5,5,5q��� -   	 ��Name$	 BrowsedFont��$��2��$��1��$��2��$��Center=�7  1System.Windows.Forms.Integration.WindowsFormsHost  $��3��$��3��$��10q�
   Child.
 "   System.Windows.Forms.TextBox  -   BrowsedPreview��&    System.Windows.Forms.TextBoxBase   WordWrap .    	Multiline . �� -   $ Ok��$��OK��$��4��$��1��$��5q� �� 	IsDefault . �� -   $ Cancel��$��Cancel��$��4��$��2��$��5q� �� IsCancel . 