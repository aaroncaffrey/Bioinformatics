'Option Explicit

Enum ProteinColourScheme
    HydrophobicityProteinColourer
    PhysicochemicalClustalW2
    PhysicochemicalUniProtKB
End Enum

Sub ColourProteinSequenceHydrophobicity()
    ColourProteinSequence HydrophobicityProteinColourer
End Sub


Sub ColourProteinSequencePhysicochemicalClustalW2()
    ColourProteinSequence PhysicochemicalClustalW2
End Sub



Sub ColourProteinSequencePhysicochemicalUniProtKB()
    ColourProteinSequence PhysicochemicalUniProtKB
End Sub


Sub ColourProteinSequence(scheme As ProteinColourScheme)

    Dim textIndex As Long
    Dim text As String
    
    Dim BlueAminoAcids As String
    Dim RedAminoAcids As String
    Dim GreenAminoAcids As String
    Dim YellowAminoAcids As String
    Dim GreyAminoAcids As String
    Dim MagentaAminoAcids As String
    Dim BlackAminoAcids As String
    Dim WhiteAminoAcids As String
    Dim LightGreyAminoAcids As String
    
    
    If (scheme = ProteinColourScheme.HydrophobicityProteinColourer) Then
        BlueAminoAcids = "AGILPV"
        RedAminoAcids = "FYW"
        GreenAminoAcids = "DENQRHSTK"
        YellowAminoAcids = "CM"
        GreyAminoAcids = "BJOUXZ"
        
        If (Len(RedAminoAcids) + Len(BlueAminoAcids) + Len(YellowAminoAcids) + Len(GreenAminoAcids) + Len(GreyAminoAcids) <> 26) Then
            Err.Raise 1, "Missing amino acid letter", "Missing amino acid letter"
        End If
    ElseIf (scheme = ProteinColourScheme.PhysicochemicalClustalW2) Then
        RedAminoAcids = "AVFPMILW"
        BlueAminoAcids = "DE"
        MagentaAminoAcids = "RK"
        GreenAminoAcids = "STYHCNGQ"
        GreyAminoAcids = "BJOUXZ"
        
        If (Len(RedAminoAcids) + Len(BlueAminoAcids) + Len(MagentaAminoAcids) + Len(GreenAminoAcids) + Len(GreyAminoAcids) <> 26) Then
            Err.Raise 1, "Missing amino acid letter", "Missing amino acid letter"
        End If
    ElseIf (scheme = ProteinColourScheme.PhysicochemicalUniProtKB) Then
        GreyAminoAcids = "LAGVIP"
        RedAminoAcids = "ED"
        GreenAminoAcids = "ST"
        BlueAminoAcids = "RKH"
        BlackAminoAcids = "FYW"
        WhiteAminoAcids = "NQ"
        YellowAminoAcids = "MC"
        LightGreyAminoAcids = "BJOUXZ"
    Else
        Err.Raise 2, "Unknown colour scheme", "Unknown colour scheme"
    End If
    
    For Each cell In Selection
        text = cell.Value
        
        For textIndex = 1 To Len(text)
            char = Mid(text, textIndex, 1)
            
            If (InStr(BlueAminoAcids, char) > 0) Then
                cell.Characters(textIndex, 1).Font.Color = rgbBlue
                
            ElseIf (InStr(RedAminoAcids, char) > 0) Then
                cell.Characters(textIndex, 1).Font.Color = rgbRed
                
            ElseIf (InStr(GreenAminoAcids, char) > 0) Then
                cell.Characters(textIndex, 1).Font.Color = rgbGreen
                
            ElseIf (InStr(YellowAminoAcids, char) > 0) Then
                cell.Characters(textIndex, 1).Font.Color = rgbYellow
                
            ElseIf (InStr(GreyAminoAcids, char) > 0) Then
                cell.Characters(textIndex, 1).Font.Color = rgbGrey
                
            ElseIf (InStr(MagentaAminoAcids, char) > 0) Then
                cell.Characters(textIndex, 1).Font.Color = rgbMagenta
                
            ElseIf (InStr(BlackAminoAcids, char) > 0) Then
                cell.Characters(textIndex, 1).Font.Color = rgbBlack
                
            ElseIf (InStr(WhiteAminoAcids, char) > 0) Then
                cell.Characters(textIndex, 1).Font.Color = rgbWhite
                
            ElseIf (InStr(LightGreyAminoAcids, char) > 0) Then
                cell.Characters(textIndex, 1).Font.Color = rgbLightGrey
                
            Else
                cell.Characters(textIndex, 1).Font.Color = rgbBlack
                
            End If
        Next
    Next cell
End Sub
