   M S B A M L   `   `   `  ����@  <kadry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:kadry.Windowskadry.Windows  5   	   !    kadry.Windows.WyplataDialog   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:kadry.Windows     5   	      BackgroundBrush#��[  5   	     � 
ResizeMode$  CanMinimize=�5	   	    � WindowStartupLocation$ CenterScreen=�5
   	    � Title$ Wybierz��5   	   $	��450��6   $	��350��6&   .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      $��33 �6   �� 5      �� 5      $��60 �6   .+�5   
   �� -   5         ��Name$ 	ComboList��5   N   $��15(�6   #��[  6    $��1��6a    �� SelectionMode$ Single=�6�   $��Stretch=�6�   $��2��6�   $��Stretch=�6�   5      �� 5   
     MainButtonStyle#��[ 6    �� IsCancel . $��2��6J   $��1��6W   $��50��6g   $��50��6r   $��Right=�6~   $��0,0,60,5q�6�   .��5      F�   
CancelPath#��[ 6   5      �� 5   
   -   5         ��Name$ Select��5   :   #��[ 6   {�.  $��2��6q   $��1��6~   $��50��6�   $��50��6�   $��Right=�6�   $��0,0,5,5q�6�   .��5      F�   	ApplyPath#��[ 6   5       F� 5!   
   $
��Left=�6   $	��Top=�6*   $��5,5,0,0q�6B     
SearchPath#��[ 6S   $��23��6w   	 �� Stretch$	 Uniform=�6�   $��23��6�   �� 5"   
   -   5        
 ��Name$
 	SearchBox��5"        TextBoxBaseControlTemplate#��[ 6I   $��Stretch=�6�   $��Center=�6�   $��23��6�   $��38,5,5,0q�6�    �� TextWrapping$
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $��2��6  �� 5#   
   -   5         ��Name$ 	YearCombo��5#   A   $
��Left=�6T   $��Center=�6o   $	��152��6�   $��
10,10,5,10q�6�   $��2��6�   5$      5%      