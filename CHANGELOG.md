# Change Log

All notable changes to **Nemiro.OAuth** will be documented in this file.

## [v1.13] - 2017-08-25

This release contains minor improvements and updates API versions used by clients.

### Added

* Added `ResponseType` to `OAuth2Client`;
* Added default return URL to `OdnoklassnikiClient`;
* Added method `SetAccessToken` to `OAuthBase` (only for OAuth v2.0 and token requests);
* Added method `Get` to `OAuthBase` for strict type of clients.

### Changed

* Changed type of the `Version` property. Now is `Version` type;
* Facebook Graph API updated from v2.7 to v2.9;
* VK API updated from v5.53 to v5.65.

### Fixed

* Added `UrlDecore` to the `ParseParameters`;
* `YahooClient`: Receiving a user GUID, if there is no such identifier in the headers.

### Thanks

* [Visio70](https://github.com/Visio70)
* [santoshpasi](https://github.com/santoshpasi)

## [v1.12] - 2016-09-12

This release includes upgrades API, used by clients.
And also general improvements.

### Added

* Added custom state into authorization requests;
* Added the ability to create custom providers for manage authorization requests;
* Added automatic serialization to JSON and XML for `RequestBody`;
* Added the ability to override the HTTP headers: `Accept`, `Connection`, `Expect`, `Transfer-Encoding` and `User-Agent`.

### Changed

* Updated `DropboxClient` to API v2;
* Updated `FacebookClient` to Graph API v2.7;
* Updated `GoogleClient` to API v3 and v4 for token;
* Updated `VkontakteClient` to API v5.53;
* Updated endpoints for `LinkedinClient` and `OdnoklassnikiClient`;
* Changed `HttpParameterType`. Added item `None` into the first position, the remaining elements are shifted one step;
* `HttpParameterCollection.ToStringParameters` only for Url and Unformed parameters;
* `HttpParameterCollection.ToRequestBody` is discarded.

### Thanks

* [filmico](https://github.com/filmico)
* [rrivani](https://github.com/rrivani)
* [Mike Norgate](https://github.com/mikenorgate)

## [v1.11.2477] - 2016-08-08

This release includes minor fixes.

### Added

* Added ability to disable the encoding of the names of query parameters.

### Changed

* Methods that takes `HttpPostedFile` is deprecated.

### Fixed

* Fixed a problem with the specified `Content-Type`, which could lead to an incorrect calculation of the signature OAuth 1.0 in some cases.

### Thanks

* [Shakeel Ahmad](http://www.codeproject.com/script/Membership/View.aspx?mid=6184216)

## [v1.11] - 2016-07-11

This release includes minor fixes and enhancements.

### Added

* Added parameter names encoding;
* Added the ability to upload large files;
* Strong name.

### Thanks

* [Nicola Bizzoca](https://github.com/nico159)
* [Michael Collins](https://github.com/mfcollins3)

## [v1.10] - 2015-06-21

This version includes fixes for .NET 3.5 and common enhancements.

### Added

* Added the ability to use any multipart requests (not only `multipart/form-data`);

### Removed

* Removed the obsolete overload of the `GetUserInfo`.

### Fixed

* Fixed bug in the `WriteToRequestStream` for .NET Framework 3.5;
* Fixed error: "Inheritance security rules violated while overriding member: UniValue.GetObjectData...". An error was detected in projects .NET Framework 3.5;
* Fixed client for LinkedIn (updated default scope);

### Thanks

* [Ramil Khazhiev](https://github.com/RamilKhazhiev)
* [Asi Nehrim](http://www.youtube.com/channel/UC6dAAoRUxMGBR3FwP_P9vAA)
* [CodeProject Member 11728803](http://www.codeproject.com/script/Membership/View.aspx?mid=11728803)

## [v1.9] - 2015-03-19

This version includes fixes and enhancements.

### Added

* In the method GetUserInfo added ability to specify an access token;
* In the web methods of the `OAuthUtility` class added ability to specify an access token;
* Implemented refreshing and revoking an access token for providers that support it.

### Changed

* Updated URLs in the `GoogleClient`;
* Reworked the `AccessToken` class;

### Fixed

* Fixed JSON: 
  * single quotes replaced by double; 
  * names are placed in quotation marks; 
  * added encoding special characters, and unicode characters; 
  * fixed the decimal separator for numbers.
* Fixed bug with overwriting the query parameters in obtaining authorization address;
* Fixed `UserInfo` mapper for LinkedIn;
* Fixed typo (internal): `Requet` -> `Requests`;

### Thanks

* [Mike Norgate](https://github.com/mikenorgate)
* [Steve Barron](https://github.com/sdbarron)

## [v1.8] - 2015-03-08

This version includes enhancements for customization.

### Added

* Added the ability to register multiple client with the same name;
* Added decoding html-entities in the processing of a callback address, if provider, for some reason, perform encoding 	(potential problem is detected in Foursquare);
* Added OAuth client for Assembla;

### Changed

* Allowed to specify the `GrantType` after an instance of a client;
* Opened access (public modifier) to basic properties of the OAuth protocol;

### Removed

* Deleted file of the obsolete `Helper` class (obsolete since v1.4; use `OAuthUtility`).

### Fixed

* Fixed bug with `DefaultScope` and `Scope`;

### Thanks

* [Nacer](https://github.com/Nacer-)
* [codexboise](https://github.com/codexboise)

## [v1.7] - 2015-02-11

The version improved for Windows Forms projects.

### Added

* Added a data binding for API responses;
* Added ability to specify `grant_type`: `authorization_code` (default), `password` and `client_credentials`;
* Added OAuth clients for: CodeProject and SourceForge.

## [v1.6] - 2015-01-04

The version includes minor improvements.

### Added

* Added `OAuthManager.GetClientTypeByName` method to obtain the type of client;
* Added default scope.

## [v1.5] - 2014-12-27

In this version were made significant changes and improvements, which are mainly aimed at simplifying integration with a variety of API.

### Added

* Added support for requests: `PUT` and `DELETE`;
* Added methods to perform asynchronous requests;
* Added support for Unicode to URL encoding method (RFC-3986);  
* Added OAuth clients for: Instagram and Tumblr;
* Unified mechanism for handling responses in various data formats (XML, JSON, PLAIN). Created universal type - `UniValue`.

### Changed

* Improved transmission parameters in the web request, added support for file transfer;
* Simplified mechanism for generating and usage of the authorization header.

## [v1.4] - 2014-11-02

### Added

* Added the ability to register the clients class by provider name;
* Added OAuth clients for: Dropbox, Foursquare, LinkedIn, SoundCloud and Yahoo!

### Changed

* The `Helpers` class marked as `[Obsolete]`. Created new class - `OAuthUtility`;
* Methods for signature moved to the `OAuthUtility` class;
* Improved class `OAuthAuthorization`, added SetSignature method;

### Fixed

* Fixed SSL3 problem, completely;
* Fixed problems with `Content-Type`;
* Fixed minor bugs in `OAuthClient` (for OAuth 1.0);
* Username-getting for Yandex;

## [v1.3] - 2014-10-23

### Fixed

* Fixed SSL3 problem.

## [v1.2] - 2014-10-08

### Changed

* Updated protocol for Odnoklassniki.ru
* Improved client for VKontakte: added the ability to receive an email address.

### Thanks

* [Aleksander (KamAz) Kryatov](http://vk.com/acid_rock)

## [v1.1] - 2014-07-20

* First public release.