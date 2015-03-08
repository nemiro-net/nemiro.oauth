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
    Dim accessToken As RequestResult = client.AccessToken
    If accessToken.IsSuccessfully Then
      Console.WriteLine("Access token:")
      Console.WriteLine(CType(accessToken, AccessToken).Value)
      Console.WriteLine("")
      Console.WriteLine("Press any key to exit")
      Console.ReadKey()
    Else
      Console.WriteLine(accessToken.ToString())
      GoTo again
    End If
  End Sub

End Module
