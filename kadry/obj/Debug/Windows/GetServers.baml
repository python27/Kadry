   M S B A M L   `   `   `  ����@  <kadry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:kadry.Windowskadry.Windows  5   	       kadry.Windows.GetServers   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:kadry.Windows     5   	     � 
ResizeMode$  CanMinimize=�5   	      BackgroundBrush#��[  5	   	    � WindowStartupLocation$ CenterScreen=�5
   	    � Title$ 
GetServers��5   	   $	��450��6   $	��350��6)   .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      �� 5      $��60 �6   .+�5   
   �� -   5         ��Name$ 
ServerList��5   N   #��[  6   $��15(�6@    �� SelectionMode$ Single=�6�   $��Stretch=�6�   $��2��6�   $��Stretch=�6�   5      �� 5   
    �� IsCancel .   MainButtonStyle#��[ 6!   $��2��6J   $��2��6X   $��50��6h   $��Right=�6s   $��5,5,60,5q�6�   .��5      F�   
CancelPath#��[ 6   5      �� 5   
   -   5         ��Name$ SelectServer��5      #��[ 6'   {�.  $��Right=�6b   $��50��6~   $��50��6�   $��2��6�   $��1��6�   $��5q�6�   .��5      F�   	ApplyPath#��[ 6   5      �� 5   
   -   5        	 ��Name$	 LoadingText��5      $��2��6)   $��17(�6=   $��Wczytywanie serwerów��6K   $��Center=�6g   $��Center=�6�   5!      5"      