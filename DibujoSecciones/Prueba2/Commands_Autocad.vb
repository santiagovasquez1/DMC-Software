Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Runtime

Public Class Commands_Autocad
    <CommandMethod("Prueba1")>
    Sub add_scale()

        Dim Doc = Application.DocumentManager.MdiActiveDocument
        Dim db As Database = Doc.Database
        Dim ed As Editor = Doc.Editor

        Try
            Dim cm As ObjectContextManager = db.ObjectContextManager
            If cm <> Nothing Then
                Dim occ As ObjectContextCollection = cm.GetContextCollection("ACDB_ANNOTATIONSCALES")
                If occ <> Nothing Then

                    Dim asc As AnnotationScale = New AnnotationScale
                    With asc
                        .Name = "1:25"
                        .PaperUnits = 1
                        .DrawingUnits = 25
                    End With

                    occ.AddContext(asc)
                End If
            End If
        Catch ex As Exception
            ed.WriteMessage(ex.ToString())
        End Try

    End Sub

End Class
