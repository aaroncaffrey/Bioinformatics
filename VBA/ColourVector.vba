Sub ColourVector()

    Dim textIndex As Long
    Dim text As String
    
    Dim Group1 As String
    Dim Group2 As String
        
    Group1 = "1TY+"
    Group2 = "0FN-"
    
    For Each cell In Selection
        text = cell.Value
        
        For textIndex = 1 To Len(text)
            char = Mid(text, textIndex, 1)
            
            If (InStr(Group1, char) > 0) Then
                cell.Characters(textIndex, 1).Font.Color = rgbAquamarine
                
            ElseIf (InStr(Group2, char) > 0) Then
                cell.Characters(textIndex, 1).Font.Color = rgbYellow
                
            Else
                cell.Characters(textIndex, 1).Font.Color = vbBlack
                
            End If
        Next
    Next cell
End Sub
