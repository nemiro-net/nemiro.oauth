Imports Nemiro.OAuth
Imports Nemiro.OAuth.Clients

Module Module1

  Sub Main()
    Dim client As New FacebookClient("1435890426686808", "c6057dfae399beee9e8dc46a4182e8fd")
    client.GrantType = GrantType.ClientCredentials

again:
    Console.WriteLine("Enter your login:")
    client.Username = Console.ReadLine()
    Console.WriteLine("Password:")
    client.Password = Console.ReadLine()
    Console.WriteLine("Getting access token...")
    If Not String.IsNullOrEmpty(client.AccessTokenValue) Then
      Console.WriteLine("Access token:")
      Console.WriteLine(client.AccessTokenValue)
      Console.WriteLine("")
      Console.WriteLine("Press any key to exit")
      Console.ReadKey()
    Else
      Console.WriteLine(client.AccessToken.ToString())
      GoTo again
    End If
  End Sub

End Module
