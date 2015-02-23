using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nemiro.OAuth;

namespace AspNetWebFormsMulticlients
{

  /// <summary>
  /// Represents the request item to OAuth server.
  /// </summary>
  public class CustomOAuthRequest
  {

    private OAuthBase _Client = null;

    /// <summary>
    /// Gets instance of the OAuth client.
    /// </summary>
    public OAuthBase Client
    {
      get
      {
        return _Client;
      }
    }

    private DateTime _DateCreated = DateTime.Now;

    /// <summary>
    /// Gets date and time creation of the request.
    /// </summary>
    public DateTime DateCreated
    {
      get
      {
        return _DateCreated;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthRequest"/> class.
    /// </summary>
    /// <param name="client">The instance of the OAuth client.</param>
    public CustomOAuthRequest(OAuthBase client)
    {
      _Client = client;
    }

  }

}