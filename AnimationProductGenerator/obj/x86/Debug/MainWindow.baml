   M S B A M L   `   `   `  ����  AnimationProductGenerator*    $AnimationProductGenerator.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$!  Animation Product Generator��5   	   $	��600��6-   $	��800��6:    � WindowStartupLocation$ CenterScreen=�6F   .��5      �  � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��* �6   �� 5	      $��10 �6    � ColumnDefinitions 5   
   �� 5      $��80 �6   �� 5      $	��150 �6   �� 5      $��80 �6   �� 5      $	��150 �6   �� 5      $��80 �6   �� 5      $��80 �6   �� 5      $��80 �6   .+�5   
   �� -   5        $��lblDivision��5      $��
Divisions:��6#   $��Center=�68   $��Right=�6S   $��0��6o   �� 5   
   -   5        $��cboDivisions��5       �� DisplayMemberPath$
 Name��6X   $��Center=�6q   $��1��6�   �� 5   
   -   5        $��lblAnimations��5      $��Animations:��6%   $��Center=�6;   $��Right=�6V   $��2��6r   �� 5   
   -   5        $��cboAnimations��5      $
 Name��6Z   $��Center=�6s   $��3��6�   �� 5   
   -   5        $��lblCount��5      $��Count:��6    $��Center=�61   $��Right=�6L   $��4��6h   �� 5   
   -   5        $��txtCount��5      $��Center=�6"   $��5��6=   �� 5   
   -   5        $��btnGenerate��5      $��Generate��6$   $��Center=�67   $��Right=�6R   $��6��6n   6~   5       5!      