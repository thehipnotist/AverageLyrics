   M S B A M L   `   `   `  ����  AverageLyrics    AverageLyrics.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Average Lyrics��5   	   $	��700��6    $	��700��6-   .��5      � .+�5   
   �� -   5         ��Name$ FirstInstructions��5      $��45��6/   $��	30,20,0,0q�6;   $
��Left=�6O   $	��Top=�6j   $	��640��6�    �� Padding$ 5q�6�   $��WrapWithOverflow=�6�   
�5      CAThis tool gives the average number of words in an artist's songs.�� 5	      |zTo get started, enter an artist's name in the box, and if desired select a type to filter on. Then click 'Search Artists'.5      �� 5   
   -   5         ��Name$ ArtistLabel��5      $��Artist Name��6%   $
��Left=�6;   $��30��6V   $��	30,70,0,0q�6b   $	��Top=�6u   $��80��6�   �� 5   
   -   5         ��Name$ 
ArtistName��5      $
��Left=�6&   $��30��6A   $��
105,70,0,0q�6M    �� TextWrapping$
 Wrap=�6a   $�� ��6u   $	��Top=�6}   $	��180��6�      RoundedTextBox#���   5      �� 5   
   -   5        $ 	TypeLabel��5      $��Artist Type��6#   $
��Left=�69   $��30��6T   $��
320,70,0,0q�6`   $	��Top=�6t   $��70��6�   �� 5   
   $
��Left=�6   $��30��6.   $��
390,70,0,0q�6:   $	��Top=�6N   $	��110��6f     RoundedCombo#���  6r   �� 5   
   -   5         ��Name$ ArtistSearchButton��5      $��Search Artists��6-   $
��Left=�6F   $	��Top=�6a   $��90��6y   $��
550,70,0,0q�6�   $��30��6�     RoundedButton#���  5      �� 5   
   -   5        $ SecondInstructions��5      $��45��60   $��
30,110,0,0q�6<   $
��Left=�6Q   $	��Top=�6l   $	��640��6�   $ 5q�6�   $��WrapWithOverflow=�6�   
�5      }The first ten matches are displayed below, with the best match selected. If that is the correct artist, click 'Search Songs'.�� 5      pnIf not, select another record below, or if it is not listed, try changing the search string and/or type above.5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ ArtistDataGrid��5      $
��Left=�6+   $	��175��6F   $��
80,160,0,0q�6S   $	��Top=�6g   $	��385��6      
IsReadOnly . 	   AutoGenerateColumns	 .  
   Columns
 5       0  *System.Windows.Controls.DataGridTextColumn  5!      -	   5        ArtistNameColumn��,  &System.Windows.Controls.DataGridColumn   Header$ Artist Name��5!   ]      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6r      MinWidth$	 150Q�6|   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6?   ��  �� Path$
 Name�6|     5"      -
   5        ArtistTypeColumn��$ Artist Type��5"   ]    6?   �� $
 Type�6]     5#      -   5        ArtistCountryColumn��$ Country��5#   `    6B   �� $
 Type�6`     5$      -   5        ArtistScoreColumn��$ Search Score��5$   ^    6@   �� $
 Type�6^   5&      �� 5'   
   -   5        $ SongSearchButton��5'      $��Search Songs��6+   $
��Left=�6B   $	��Top=�6]   $��90��6u   $��550,160,0,0q�6�   $��30��6�   #���  5(        5*   
   -   5        $ SongDataGrid��5*      $
��Left=�6)   $	��288��6D   $��
80,350,0,0q�6Q   $	��Top=�6e   $	��385��6}    . 	 .  
 5+        5,      -   5        SongNameColumn��$ 
Song Title��5,   \   $ * 6p   $	 150Q�6z    6=   �� $ Title�6z     5/      -   5        LyricsColumn��$ Lyric Count��5/   _    6;   �� $ 
LyricCount�6_   51      54      55      