   M S B A M L   `   `   `  ����@  <kadry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:kadry.Windowskadry.Windows  5   	       kadry.Windows.DbPass   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:kadry.Windows     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ CanMinimize=�5	   	      BackgroundBrush#��[  5
   	    � Title$* $Zaloguj się do bazy konfiguracyjnej��5   	   $	��300��65   $	��300��6B   .��5      �  � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      $
��1.2* �6    � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��Użytkownik SQL��6   $��10q�6*   $��18(�66   �� 5   
   -   5         ��Name$
 User��5   I     TextBoxBaseControlTemplate#��[ 6   $��1��6W   $��2��6d   $��5q�6x   �� 5   
   $��Hasło��6   $��2��6!   $��10q�6.   $��18(�6:   G� 5   
   -   5         G�Name$ Password��5   Q    " PasswordBoxBaseControlTemplate#��[ 6   $��3��6c   $��2��6p   $��5q�6�   �� 5   
     MainButtonStyle#��[ 6    �� IsCancel . $��4��6J   $��1��6W   $��50��6g   $��50��6r   $��Right=�6~   $��0,0,60,5q�6�   .��5      F�   
CancelPath#��[ 6   5      �� 5    
   -   5         ��Name$ Confirm��5    :   #��[ 6   {�.  $��4��6r   $��1��6   $��50��6�   $��50��6�   $��Right=�6�   $��0,0,5,5q�6�   .��5!      F�   	ApplyPath#��[ 6   5"      5$      5%      