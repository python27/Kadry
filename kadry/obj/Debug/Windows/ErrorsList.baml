   M S B A M L   `   `   `  ����@  <kadry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:kadry.Windowskadry.Windows  5   	   ;"clr-namespace:kadry.Windows.Modelskadry.Windows.Models  5   	       kadry.Windows.ErrorsList   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:kadry.Windows     5   	   5models"clr-namespace:kadry.Windows.Models     5   	     � 
ResizeMode$  NoResize=�5
   	    � WindowStartupLocation$ CenterScreen=�5   	    � Title$ 
ErrorsList��5   	   $	��450��5   	   $	��800��6   .��5      �  � RowDefinitions 5   
   �� 5      $��50��6   $	��500��6I   ��6+   �� *
Row0Height+6I   �� 5      .+�5   
   �� -   5         ��Name$ Message��5      $��5q�6   $��0��6U   $	��Top=�6{   ��6b   �� *	Message+6{   �� 5   
   -   5         ��Name$ Errors��5      $��Stretch=�5       �� 
IsReadOnly . " �� VerticalScrollBarVisibility$
 Auto=�5      $��1��5      ��5      �� *Errors+5      5      