   M S B A M L   `   `   `  ����?  ;dCom, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:dComdCom  5   	   1clr-namespace:dCom.ConvertersdCom.Converters  5          dCom.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:dCom     5   	   4
convertersclr-namespace:dCom.Converters     5        � Title$  dComm��5	   	   $	��350��6   $
��1000��6$    � 	Resources 5
      5   /dCom.Converters.ConnectionStateToColorConverter   ConnectionStateToColor*  $System.Windows.Controls.DataGridCell �� 
TargetType��%b   &	        '         5      6/   �� 5      "  6
   .�5      -   5        5      5      .��5      �  � RowDefinitions 5      �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5      �� 5      $
��0.6* �6   �� 5      $
��0.4* �6   .+�5      &   System.Windows.Controls.DataGrid  -   5        $��dgPoints��5         SelectionMode$ Single=�5   ,      AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .  	   CanUserResizeRows	 .  
   SelectionUnit$
 FullRow=�5      ��5      �� *Points+   Columns 5       0  *System.Windows.Controls.DataGridTextColumn  5!      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6'   �� *Name+6@     5"      $
 Type��6    .  6'   �� *Type+6@     5#      $ Address��6    .  6*   �� *	Address+ �� StringFormat$ {0:d5}��6]     5$      $ Value��6    .  6(   �� *DisplayValue+6I     5%      $ 	Raw Value��6    .  6,   �� *
RawValue+6I     5&      $ Quality��6    .  6*   �� *	Quality+6F     5'      $ 	Timestamp��6    .  6,   �� *	Timestamp+6J   5*      �� 5,      $��2��6   $��2��6   .@�5-      �� .��5.      ��  �� Orientation$ 
Horizontal=�6   .+�5/      �� $��Current Time: ��6   �� 50      .��51      �� ��6   ��  �� Path$ CurrentTime�$ {0:dd/MM/yyyy hh:mm:ss}��52      53      54      �� 55      .��56      �� $ 
Horizontal=�6   .+�57      �� $��Elapsed Time: ��6   �� 58      .��59      �� ��6   �� $ ElapsedTime�5:      5;      5<      �� 5=      .��5>      �� ��6   �� *ConnectionState+ �� 	Converter# [  ._�5?      �� $ 
Horizontal=�6   .+�5@      �� $��Connection status: ��6   �� 5A      .��5B   	   �� ��6   �� $ ConnectionState�5C   	   5D      5E      5F      5G      �� 5I      $	��Log��6   $��2��6   .��5J      �� ��. ��6   �� *	LogText+6(   5K      5L      5M      