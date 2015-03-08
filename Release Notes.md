### Nemiro.OAuth Release Notes

This document summarizes the changes in recent releases.

#### v1.8 (March 8, 2015)

This version includes enhancements for customization.

* Fixed bug with DefaultScope and Scope;
* Added the ability to register multiple client with the same name;
* Added decoding html-entities in the processing of a callback address, if provider, for some reason, perform encoding 	(potential problem is detected in Foursquare);
* Allowed to specify the GrantType after an instance of a client;
* Opened access (public modifier) to basic properties of the OAuth protocol;
* Added OAuth client for Assembla;
* Deleted file of the obsolete Helper class.

##### Thanks:

* [Nacer](https://github.com/Nacer-)
* [codexboise](https://github.com/codexboise)

#### v1.7 (February 11, 2015)

The version improved for Windows Forms projects.

* Added a data binding for API responses;
* Added ability to specify grant_type: authorization_code (default), password and client_credentials;
* Added OAuth clients for: CodeProject and SourceForge.

#### v1.6 (January 04, 2015)

The version includes minor improvements.

* Added OAuthManager.GetClientTypeByName method to obtain the type of client;
* Added default scope.

v1.5 (December 27, 2014)

In this version were made significant changes and improvements, which are mainly aimed at simplifying integration with a variety of API.

* Improved transmission parameters in the web request, added support for file transfer;
* Added support for requests: PUT and DELETE;
* Added methods to perform asynchronous requests;
* Simplified mechanism for generating and usage of the authorization header;
* Added support for Unicode to URL encoding method (RFC-3986);  
* Unified mechanism for handling responses in various data formats (XML, JSON, PLAIN). Created universal type - UniValue.
* Added OAuth clients for: Instagram and Tumblr.

#### v1.4 (November 2, 2014)

* Fixed SSL3 problem, completely;
* Fixed problems with Content-Type;
* Fixed minor bugs in OAuthClient (for OAuth 1.0);
* Username-getting for Yandex;
* The Helpers class marked as [Obsolete]. Created new class - OAuthUtility;
* Methods for signature moved to the OAuthUtility class;
* Improved class OAuthAuthorization, added SetSignature method;
* Added the ability to register the clients class by provider name;
* Added OAuth clients for: Dropbox, Foursquare, LinkedIn, SoundCloud and Yahoo!

#### v1.3 (October 23, 2014)

* Fixed SSL3 problem.

#### v1.2 (October 8, 2014)

* Updated protocol for Odnoklassniki.ru
* Improved client for VKontakte: added the ability to receive an email address.
	
##### Thanks:

* [Aleksander (KamAz) Kryatov](http://vk.com/acid_rock)
	
#### v1.1 (July 20, 2014)

* First version released.