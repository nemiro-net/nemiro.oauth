// ----------------------------------------------------------------------------
// Copyright © Aleksey Nemiro, 2016. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Nemiro.OAuth;

namespace TwitterExample
{

  /// <summary>
  /// Implements Twitter API.
  /// </summary>
  /// <remarks>
  /// <see href="https://dev.twitter.com/docs"/>.
  /// </remarks>
  static class TwitterApi
  {

    private static readonly int MediaUploadChunkSize = (3 * 1024 * 1024); // 3 MB

    /// <summary>
    /// Checks access token.
    /// </summary>
    public static async Task<bool> CheckAccessToken()
    {
      var parameters = new HttpParameterCollection
      {
        new HttpUrlParameter("user_id", Properties.Settings.Default.UserId),
        new HttpUrlParameter("include_entities", "false")
      };

      var t = Task.Run<bool>(() =>
      {
        var result = OAuthUtility.Get
        (
          "https://api.twitter.com/1.1/users/show.json",
          parameters: parameters,
          authorization: TwitterApi.GetAuth()
        );

        return result.IsSuccessfully;
      });

      return await t;
    }

    /// <summary>
    /// Gets tweets.
    /// </summary>
    /// <param name="lastTweetId"></param>
    /// <remarks>
    /// <see href="https://dev.twitter.com/rest/reference/get/statuses/user_timeline"/>
    /// </remarks>
    public static async Task<RequestResult> GetTweets(string lastTweetId)
    {
      HttpParameterCollection parameters = null;

      if (!String.IsNullOrEmpty(lastTweetId))
      {
        parameters = new HttpParameterCollection { new HttpUrlParameter("max_id", lastTweetId) };
      }

      var t = Task.Run<RequestResult>(() =>
      {
        return OAuthUtility.Get
        (
          "https://api.twitter.com/1.1/statuses/user_timeline.json",
          parameters: parameters,
          authorization: TwitterApi.GetAuth()
        );
      });

      return await t;
    }

    public static async Task<RequestResult> SendTweet(string text, List<string> media_ids)
    {
      var parameters = new HttpParameterCollection();
      parameters.AddFormParameter("status", text);

      // images
      if (media_ids != null && media_ids.Count > 0)
      {
        parameters.AddFormParameter("media_ids", String.Join(",", media_ids));
      }

      var t = Task.Run<RequestResult>(() =>
      {
        return OAuthUtility.Post
        (
          "https://api.twitter.com/1.1/statuses/update.json",
          parameters: parameters,
          authorization: TwitterApi.GetAuth(),
          contentType: "application/x-www-form-urlencoded"
        );
      });

      return await t;
    }

    /// <summary>
    /// Uploads media (only photos).
    /// </summary>
    /// <remarks>
    /// <see href="https://dev.twitter.com/rest/reference/post/media/upload-init"/>
    /// </remarks>
    /// <param name="path">File path.</param>
    public static async Task<RequestResult> UploadMedia(string path, MeadiaUploadEventHandler uploadingCallback)
    {
      var file = new FileInfo(path);

      string media_type = "image/jpeg";

      switch (file.Extension.ToLower())
      {
        case ".png":
          media_type = "image/png";
          break;
        case ".gif":
          media_type = "image/gif";
          break;
        case ".bmp":
          media_type = "image/bmp";
          break;
      }

      var parameters = new HttpParameterCollection();
      parameters.AddFormParameter("command", "INIT");
      parameters.AddFormParameter("total_bytes", file.Length.ToString());
      parameters.AddFormParameter("media_type", media_type);
      parameters.AddFormParameter("media_category", "tweet_image");

      var t = Task.Run<RequestResult>(() =>
      {
        return OAuthUtility.Post
        (
          "https://upload.twitter.com/1.1/media/upload.json",
          parameters: parameters, 
          authorization: TwitterApi.GetAuth(),
          contentType: "multipart/form-data"
        );
      });

      var result = await t;

      if (result.IsSuccessfully)
      {
        return await TwitterApi.MeadiaUploadAppend(path, result["media_id"].ToString(), 0, uploadingCallback);
      }
      else
      {
        return result;
      }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/media/upload-append
    /// </summary>
    /// <param name="path">File path.</param>
    /// <param name="media_id">Media id.</param>
    /// <param name="chunk">Chunk. Default: 0.</param>
    /// <param name="uploadingCallback">Uploading callback.</param>
    private static async Task<RequestResult> MeadiaUploadAppend(string path, string media_id, int chunk, MeadiaUploadEventHandler uploadingCallback)
    {
      var file = new FileInfo(path);
      bool isUploded = false;
      byte[] media = null;

      if (chunk > 0)
      {
        // multiple chunks
        using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Inheritable))
        using (var reader = new BinaryReader(stream))
        {
          stream.Position = (TwitterApi.MediaUploadChunkSize * chunk);
          media = reader.ReadBytes(TwitterApi.MediaUploadChunkSize);
          isUploded = (stream.Position == stream.Length);
        }
      }
      else
      {
        if (file.Length <= TwitterApi.MediaUploadChunkSize)
        {
          // one chunk
          using (var reader = new BinaryReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.Inheritable)))
          {
            media = reader.ReadBytes(Convert.ToInt32(file.Length));
            isUploded = true;
          }
        }
        else
        {
          // multiple chunks
          using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Inheritable))
          using (var reader = new BinaryReader(stream))
          {
            media = reader.ReadBytes(TwitterApi.MediaUploadChunkSize);
            isUploded = (stream.Position == stream.Length);
          }
        }
      }

      var parameters = new HttpParameterCollection();
      parameters.AddFormParameter("command", "APPEND");
      parameters.AddFormParameter("media_id", media_id);
      parameters.AddFormParameter("segment_index", chunk.ToString());
      parameters.Add("media", Path.GetFileName(path), media);

      var t = Task.Run<RequestResult>(() =>
      {
        return OAuthUtility.Post
        (
          "https://upload.twitter.com/1.1/media/upload.json",
          parameters: parameters, 
          authorization: TwitterApi.GetAuth(),
          contentType: "multipart/form-data",
          streamWriteCallback: (object s, StreamWriteEventArgs e) => 
          {
            int progressPercentage = 0;
            long totalUploaded = 0;

            if (chunk > 0)
            {
              totalUploaded = TwitterApi.MediaUploadChunkSize * chunk;
            }

            totalUploaded += e.TotalBytesWritten;

            progressPercentage = Convert.ToInt32((totalUploaded * 100) / file.Length);

            uploadingCallback?.Invoke(s, new ProgressChangedEventArgs(progressPercentage, null));
          }
        );
      });

      var result = await t;

      if (!result.IsSuccessfully)
      {
        // error
        return result;
      }

      if (file.Length > TwitterApi.MediaUploadChunkSize && !isUploded)
      {
        // next chunk
        return await TwitterApi.MeadiaUploadAppend(path, media_id, chunk + 1, uploadingCallback);
      }
      else
      {
        // finalize
        return await TwitterApi.MeadiaUploadFinalize(path, media_id);
      }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/media/upload-finalize
    /// </summary>
    /// <param name="media_id">Media id.</param>
    private static async Task<RequestResult> MeadiaUploadFinalize(string path, string media_id)
    {
      var parameters = new HttpParameterCollection();
      parameters.AddFormParameter("command", "FINALIZE");
      parameters.AddFormParameter("media_id", media_id);

      var t = Task.Run<RequestResult>(() =>
      {
        return OAuthUtility.Post
        (
          "https://upload.twitter.com/1.1/media/upload.json",
          parameters: parameters,
          authorization: TwitterApi.GetAuth(),
          contentType: "multipart/form-data"
        );
      });

      return await t;
    }

    /// <summary>
    /// Returns Authorization.
    /// </summary>
    /// <returns></returns>
    public static OAuthAuthorization GetAuth()
    {
      var auth = new OAuthAuthorization();
      auth.ConsumerKey = Properties.Settings.Default.ConsumerKey;
      auth.ConsumerSecret = Properties.Settings.Default.ConsumerSecret;
      auth.SignatureMethod = SignatureMethods.HMACSHA1;
      auth.Token = Properties.Settings.Default.AccessToken;
      auth.TokenSecret = Properties.Settings.Default.TokenSecret;

      return auth;
    }

  }

}