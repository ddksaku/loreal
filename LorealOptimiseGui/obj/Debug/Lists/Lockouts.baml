   M S B A M L   `   `   `  ����L  HLorealOptimiseGui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:LorealOptimiseGuiLorealOptimiseGui  5      a ]DevExpress.Xpf.Grid.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a\Dclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1DevExpress.Xpf.Grid 5      a ]DevExpress.Xpf.Core.v11.1, Version=11.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49atPclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors.Settings 5      bGclr-namespace:DevExpress.Xpf.Editors;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors 5	      tPclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1DevExpress.Xpf.Editors.Settings 5
      &     LorealOptimiseGui.Lists.Lockouts   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =LorealOptimiseGuiclr-namespace:LorealOptimiseGui     5      TdxgDclr-namespace:DevExpress.Xpf.Grid;assembly=DevExpress.Xpf.Grid.v11.1     5      `dxePclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1     5      XdxedGclr-namespace:DevExpress.Xpf.Editors;assembly=DevExpress.Xpf.Core.v11.1     5	      eSettingsPclr-namespace:DevExpress.Xpf.Editors.Settings;assembly=DevExpress.Xpf.Core.v11.1     5
      ;dx2http://schemas.devexpress.com/winfx/2008/xaml/core  5      .��5      �   � RowDefinitions  5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��90 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��* �6   �� 5      $��30 �6   �� 5      $��15 �6    � ColumnDefinitions 5   
   �� 5      $	��150 �6   �� 5      $	��140 �6   �� 5      $	��140 �6   �� 5       $��* �6   .+�5#   
   �� $��10��6   $��10��6)   ��5$      �� 5%       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5&      �  � Color$ 	#FFBDCCDA��6#    � Offset$ 0Q�65   � 5'      $ White��6#   $ 1Q�61   5(      5*      �� 5,   
   -   5        $��lblTitle��5,      $$��Insert a new lock out schedule��6    $��0��6I   $��2��6V   �� 5-   
   -   5        $��lblFutureLockouts��5-      $��Upcoming lockouts��6)   $��2��6E   �� 5.   
   -   5        $��lblStartTime��5.      $��
Start Time��6$   $��3��69   �� 5/   
   -   5        $��
lblEndTime��5/      $��End Time��6"   $��4��65   $��0��6B   (  "DevExpress.Xpf.Editors.ListBoxEdit  51   
   -   5        $��lstFutureLockouts��51         DisplayMember$ ScheduleRange��64      SelectionMode$ Single=�6R   $��2��6i   $��1��6v   $��2��6�   $��2q�6�   %  DevExpress.Xpf.Editors.DateEdit  52   
   -   5        $��dateStartDate��52      $��3��6-   $��1��6:   $��2q�6J   %  DevExpress.Xpf.Editors.TextEdit  53   
   -   5        $��txtStartTime��53         Mask$ HH:mm:ss��6,   	   MaskType$	 DateTime=�6<   
   MaskUseAsDisplayFormat
 . $��3��6n   $��2��6{   $��2q�6�     54   
   -   5        $��dateEndDate��54      $��4��6+   $��1��68   $��2q�6H     55   
   -	   5        $��
txtEndTime��55      $ HH:mm:ss��6*   $	 DateTime=�6:   
 . $��4��6l   $��2��6y   $��2q�6�   �� 57   
   -
   5        $��btnSave��57      $
��Save��6    $��80��6/   $��6��6P   $��1��6]   $��2q�6m   �� 58   
   -   5        $��	btnCancel��58      $��Cancel��6"   $��80��63   $��6��6V   $��2��6c   $��2q�6s   5:      5;      