Imports Nemiro.OAuth
Imports Nemiro.OAuth.Clients
Imports System.Reflection

Module Module1

  Sub Main()
    Console.WriteLine("Nemiro.OAuth v{0}", Assembly.GetAssembly(GetType(OAuth2Client)).GetName().Version)
    Console.WriteLine(StrDup(Console.WindowWidth - 1, "-"))

    Dim client As New FacebookClient("1435890426686808", "c6057dfae399beee9e8dc46a4182e8fd")

    client.GrantType = GrantType.ClientCredentials

again:

    Console.WriteLine("Enter your facebook login:")
    client.Username = Console.ReadLine()

    Console.WriteLine("Password:")
    client.Password = Console.ReadLine()

    Console.WriteLine(StrDup(Console.WindowWidth - 1, "-"))

    Console.WriteLine("Getting access token...")

    If Not String.IsNullOrEmpty(client.AccessTokenValue) Then
      Console.WriteLine(StrDup(Console.WindowWidth - 1, "-"))
      Console.WriteLine("Access token:")
      Console.WriteLine(client.AccessTokenValue)
      Console.WriteLine(StrDup(Console.WindowWidth - 1, "-"))
      Console.WriteLine("Press any key to exit")
      Console.ReadKey()
    Else
      Console.WriteLine(client.AccessToken.ToString())
      GoTo again
    End If
  End Sub

End Module
