// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.UnityWebRequest
// Assembly: UnityEngine.UnityWebRequestModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DC20A38-F4BB-4294-8979-EF4355A4D9FF
// Assembly location: E:\Programs\Gamedev\Editor\2021.3.17f1\Editor\Data\Managed\UnityEngine\UnityEngine.UnityWebRequestModule.dll
// XML documentation location: E:\Programs\Gamedev\Editor\2021.3.17f1\Editor\Data\Managed\UnityEngine\UnityEngine.UnityWebRequestModule.xml

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Bindings;
using UnityEngineInternal;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Provides methods to communicate with web servers.</para>
  /// </summary>
  [NativeHeader("Modules/UnityWebRequest/Public/UnityWebRequest.h")]
  [StructLayout(LayoutKind.Sequential)]
  public class UnityWebRequest : IDisposable
  {
    [NonSerialized]
    internal IntPtr m_Ptr;
    [NonSerialized]
    internal DownloadHandler m_DownloadHandler;
    [NonSerialized]
    internal UploadHandler m_UploadHandler;
    [NonSerialized]
    internal CertificateHandler m_CertificateHandler;
    [NonSerialized]
    internal Uri m_Uri;
    /// <summary>
    ///   <para>The string "GET", commonly used as the verb for an HTTP GET request.</para>
    /// </summary>
    public const string kHttpVerbGET = "GET";
    /// <summary>
    ///   <para>The string "HEAD", commonly used as the verb for an HTTP HEAD request.</para>
    /// </summary>
    public const string kHttpVerbHEAD = "HEAD";
    /// <summary>
    ///   <para>The string "POST", commonly used as the verb for an HTTP POST request.</para>
    /// </summary>
    public const string kHttpVerbPOST = "POST";
    /// <summary>
    ///   <para>The string "PUT", commonly used as the verb for an HTTP PUT request.</para>
    /// </summary>
    public const string kHttpVerbPUT = "PUT";
    /// <summary>
    ///   <para>The string "CREATE", commonly used as the verb for an HTTP CREATE request.</para>
    /// </summary>
    public const string kHttpVerbCREATE = "CREATE";
    /// <summary>
    ///   <para>The string "DELETE", commonly used as the verb for an HTTP DELETE request.</para>
    /// </summary>
    public const string kHttpVerbDELETE = "DELETE";

    [NativeMethod(IsThreadSafe = true)]
    [NativeConditional("ENABLE_UNITYWEBREQUEST")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetWebErrorString(UnityWebRequest.UnityWebRequestError err);

    [VisibleToOtherModules]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetHTTPStatusString(long responseCode);

    /// <summary>
    ///   <para>If true, any CertificateHandler attached to this UnityWebRequest will have CertificateHandler.Dispose called automatically when UnityWebRequest.Dispose is called.</para>
    /// </summary>
    public bool disposeCertificateHandlerOnDispose { get; set; }

    /// <summary>
    ///   <para>If true, any DownloadHandler attached to this UnityWebRequest will have DownloadHandler.Dispose called automatically when UnityWebRequest.Dispose is called.</para>
    /// </summary>
    public bool disposeDownloadHandlerOnDispose { get; set; }

    /// <summary>
    ///   <para>If true, any UploadHandler attached to this UnityWebRequest will have UploadHandler.Dispose called automatically when UnityWebRequest.Dispose is called.</para>
    /// </summary>
    public bool disposeUploadHandlerOnDispose { get; set; }

    /// <summary>
    ///   <para>Clears stored cookies from the cache.</para>
    /// </summary>
    /// <param name="domain">An optional URL to define which cookies are removed. Only cookies that apply to this URL will be removed from the cache.</param>
    public static void ClearCookieCache() => UnityWebRequest.ClearCookieCache((string) null, (string) null);

    public static void ClearCookieCache(Uri uri)
    {
      if (uri == (Uri) null)
      {
        UnityWebRequest.ClearCookieCache((string) null, (string) null);
      }
      else
      {
        string host = uri.Host;
        string path = uri.AbsolutePath;
        if (path == "/")
          path = (string) null;
        UnityWebRequest.ClearCookieCache(host, path);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ClearCookieCache(string domain, string path);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern IntPtr Create();

    [NativeMethod(IsThreadSafe = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Release();

    internal void InternalDestroy()
    {
      if (!(this.m_Ptr != IntPtr.Zero))
        return;
      this.Abort();
      this.Release();
      this.m_Ptr = IntPtr.Zero;
    }

    private void InternalSetDefaults()
    {
      this.disposeDownloadHandlerOnDispose = true;
      this.disposeUploadHandlerOnDispose = true;
      this.disposeCertificateHandlerOnDispose = true;
    }

    /// <summary>
    ///   <para>Creates a UnityWebRequest with the default options and no attached DownloadHandler or UploadHandler. Default method is GET.</para>
    /// </summary>
    /// <param name="url">The target URL with which this UnityWebRequest will communicate. Also accessible via the url property.</param>
    /// <param name="uri">The target URI to which form data will be transmitted.</param>
    /// <param name="method">HTTP GET, POST, etc. methods.</param>
    /// <param name="downloadHandler">Replies from the server.</param>
    /// <param name="uploadHandler">Upload data to the server.</param>
    public UnityWebRequest()
    {
      this.m_Ptr = UnityWebRequest.Create();
      this.InternalSetDefaults();
    }

    /// <summary>
    ///   <para>Creates a UnityWebRequest with the default options and no attached DownloadHandler or UploadHandler. Default method is GET.</para>
    /// </summary>
    /// <param name="url">The target URL with which this UnityWebRequest will communicate. Also accessible via the url property.</param>
    /// <param name="uri">The target URI to which form data will be transmitted.</param>
    /// <param name="method">HTTP GET, POST, etc. methods.</param>
    /// <param name="downloadHandler">Replies from the server.</param>
    /// <param name="uploadHandler">Upload data to the server.</param>
    public UnityWebRequest(string url)
    {
      this.m_Ptr = UnityWebRequest.Create();
      this.InternalSetDefaults();
      this.url = url;
    }

    /// <summary>
    ///   <para>Creates a UnityWebRequest with the default options and no attached DownloadHandler or UploadHandler. Default method is GET.</para>
    /// </summary>
    /// <param name="url">The target URL with which this UnityWebRequest will communicate. Also accessible via the url property.</param>
    /// <param name="uri">The target URI to which form data will be transmitted.</param>
    /// <param name="method">HTTP GET, POST, etc. methods.</param>
    /// <param name="downloadHandler">Replies from the server.</param>
    /// <param name="uploadHandler">Upload data to the server.</param>
    public UnityWebRequest(Uri uri)
    {
      this.m_Ptr = UnityWebRequest.Create();
      this.InternalSetDefaults();
      this.uri = uri;
    }

    /// <summary>
    ///   <para>Creates a UnityWebRequest with the default options and no attached DownloadHandler or UploadHandler. Default method is GET.</para>
    /// </summary>
    /// <param name="url">The target URL with which this UnityWebRequest will communicate. Also accessible via the url property.</param>
    /// <param name="uri">The target URI to which form data will be transmitted.</param>
    /// <param name="method">HTTP GET, POST, etc. methods.</param>
    /// <param name="downloadHandler">Replies from the server.</param>
    /// <param name="uploadHandler">Upload data to the server.</param>
    public UnityWebRequest(string url, string method)
    {
      this.m_Ptr = UnityWebRequest.Create();
      this.InternalSetDefaults();
      this.url = url;
      this.method = method;
    }

    /// <summary>
    ///   <para>Creates a UnityWebRequest with the default options and no attached DownloadHandler or UploadHandler. Default method is GET.</para>
    /// </summary>
    /// <param name="url">The target URL with which this UnityWebRequest will communicate. Also accessible via the url property.</param>
    /// <param name="uri">The target URI to which form data will be transmitted.</param>
    /// <param name="method">HTTP GET, POST, etc. methods.</param>
    /// <param name="downloadHandler">Replies from the server.</param>
    /// <param name="uploadHandler">Upload data to the server.</param>
    public UnityWebRequest(Uri uri, string method)
    {
      this.m_Ptr = UnityWebRequest.Create();
      this.InternalSetDefaults();
      this.uri = uri;
      this.method = method;
    }

    /// <summary>
    ///   <para>Creates a UnityWebRequest with the default options and no attached DownloadHandler or UploadHandler. Default method is GET.</para>
    /// </summary>
    /// <param name="url">The target URL with which this UnityWebRequest will communicate. Also accessible via the url property.</param>
    /// <param name="uri">The target URI to which form data will be transmitted.</param>
    /// <param name="method">HTTP GET, POST, etc. methods.</param>
    /// <param name="downloadHandler">Replies from the server.</param>
    /// <param name="uploadHandler">Upload data to the server.</param>
    public UnityWebRequest(
      string url,
      string method,
      DownloadHandler downloadHandler,
      UploadHandler uploadHandler)
    {
      this.m_Ptr = UnityWebRequest.Create();
      this.InternalSetDefaults();
      this.url = url;
      this.method = method;
      this.downloadHandler = downloadHandler;
      this.uploadHandler = uploadHandler;
    }

    /// <summary>
    ///   <para>Creates a UnityWebRequest with the default options and no attached DownloadHandler or UploadHandler. Default method is GET.</para>
    /// </summary>
    /// <param name="url">The target URL with which this UnityWebRequest will communicate. Also accessible via the url property.</param>
    /// <param name="uri">The target URI to which form data will be transmitted.</param>
    /// <param name="method">HTTP GET, POST, etc. methods.</param>
    /// <param name="downloadHandler">Replies from the server.</param>
    /// <param name="uploadHandler">Upload data to the server.</param>
    public UnityWebRequest(
      Uri uri,
      string method,
      DownloadHandler downloadHandler,
      UploadHandler uploadHandler)
    {
      this.m_Ptr = UnityWebRequest.Create();
      this.InternalSetDefaults();
      this.uri = uri;
      this.method = method;
      this.downloadHandler = downloadHandler;
      this.uploadHandler = uploadHandler;
    }

    ~UnityWebRequest()
    {
      this.DisposeHandlers();
      this.InternalDestroy();
    }

    /// <summary>
    ///   <para>Signals that this UnityWebRequest is no longer being used, and should clean up any resources it is using.</para>
    /// </summary>
    public void Dispose()
    {
      this.DisposeHandlers();
      this.InternalDestroy();
      GC.SuppressFinalize((object) this);
    }

    private void DisposeHandlers()
    {
      if (this.disposeDownloadHandlerOnDispose)
        this.downloadHandler?.Dispose();
      if (this.disposeUploadHandlerOnDispose)
        this.uploadHandler?.Dispose();
      if (!this.disposeCertificateHandlerOnDispose)
        return;
      this.certificateHandler?.Dispose();
    }

    [NativeThrows]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern UnityWebRequestAsyncOperation BeginWebRequest();

    /// <summary>
    ///   <para>Begin communicating with the remote server.</para>
    /// </summary>
    /// <returns>
    ///   <para>An AsyncOperation indicating the progress/completion state of the UnityWebRequest. Yield this object to wait until the UnityWebRequest is done.</para>
    /// </returns>
    [Obsolete("Use SendWebRequest.  It returns a UnityWebRequestAsyncOperation which contains a reference to the WebRequest object.", false)]
    public UnityEngine.AsyncOperation Send() => (UnityEngine.AsyncOperation) this.SendWebRequest();

    /// <summary>
    ///   <para>Begin communicating with the remote server.</para>
    /// </summary>
    public UnityWebRequestAsyncOperation SendWebRequest()
    {
      UnityWebRequestAsyncOperation requestAsyncOperation = this.BeginWebRequest();
      if (requestAsyncOperation != null)
        requestAsyncOperation.webRequest = this;
      return requestAsyncOperation;
    }

    /// <summary>
    ///   <para>If in progress, halts the UnityWebRequest as soon as possible.</para>
    /// </summary>
    [NativeMethod(IsThreadSafe = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Abort();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetMethod(
      UnityWebRequest.UnityWebRequestMethod methodType);

    internal void InternalSetMethod(UnityWebRequest.UnityWebRequestMethod methodType)
    {
      if (!this.isModifiable)
        throw new InvalidOperationException("UnityWebRequest has already been sent and its request method can no longer be altered");
      UnityWebRequest.UnityWebRequestError err = this.SetMethod(methodType);
      if (err != 0)
        throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetCustomMethod(string customMethodName);

    internal void InternalSetCustomMethod(string customMethodName)
    {
      if (!this.isModifiable)
        throw new InvalidOperationException("UnityWebRequest has already been sent and its request method can no longer be altered");
      UnityWebRequest.UnityWebRequestError err = this.SetCustomMethod(customMethodName);
      if (err != 0)
        throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern UnityWebRequest.UnityWebRequestMethod GetMethod();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string GetCustomMethod();

    /// <summary>
    ///   <para>Defines the HTTP verb used by this UnityWebRequest, such as GET or POST.</para>
    /// </summary>
    public string method
    {
      get
      {
        switch (this.GetMethod())
        {
          case UnityWebRequest.UnityWebRequestMethod.Get:
            return "GET";
          case UnityWebRequest.UnityWebRequestMethod.Post:
            return "POST";
          case UnityWebRequest.UnityWebRequestMethod.Put:
            return "PUT";
          case UnityWebRequest.UnityWebRequestMethod.Head:
            return "HEAD";
          default:
            return this.GetCustomMethod();
        }
      }
      set
      {
        if (string.IsNullOrEmpty(value))
          throw new ArgumentException("Cannot set a UnityWebRequest's method to an empty or null string");
        switch (value.ToUpper())
        {
          case "GET":
            this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Get);
            break;
          case "POST":
            this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Post);
            break;
          case "PUT":
            this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Put);
            break;
          case "HEAD":
            this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Head);
            break;
          default:
            this.InternalSetCustomMethod(value.ToUpper());
            break;
        }
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError GetError();

    /// <summary>
    ///   <para>A human-readable string describing any system errors encountered by this UnityWebRequest object while handling HTTP requests or responses. (Read Only)</para>
    /// </summary>
    public string error
    {
      get
      {
        switch (this.result)
        {
          case UnityWebRequest.Result.InProgress:
          case UnityWebRequest.Result.Success:
            return (string) null;
          case UnityWebRequest.Result.ProtocolError:
            return string.Format("HTTP/1.1 {0} {1}", (object) this.responseCode, (object) UnityWebRequest.GetHTTPStatusString(this.responseCode));
          default:
            return UnityWebRequest.GetWebErrorString(this.GetError());
        }
      }
    }

    private extern bool use100Continue { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Determines whether this UnityWebRequest will include Expect: 100-Continue in its outgoing request headers. (Default: true).</para>
    /// </summary>
    public bool useHttpContinue
    {
      get => this.use100Continue;
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent and its 100-Continue setting cannot be altered");
        this.use100Continue = value;
      }
    }

    /// <summary>
    ///   <para>Defines the target URL for the UnityWebRequest to communicate with.</para>
    /// </summary>
    public string url
    {
      get => this.GetUrl();
      set
      {
        string localUrl = "http://localhost/";
        this.InternalSetUrl(WebRequestUtils.MakeInitialUrl(value, localUrl));
      }
    }

    /// <summary>
    ///   <para>Defines the target URI for the UnityWebRequest to communicate with.</para>
    /// </summary>
    public Uri uri
    {
      get => new Uri(this.GetUrl());
      set
      {
        if (!value.IsAbsoluteUri)
          throw new ArgumentException("URI must be absolute");
        this.InternalSetUrl(WebRequestUtils.MakeUriString(value, value.OriginalString, false));
        this.m_Uri = value;
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern string GetUrl();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetUrl(string url);

    private void InternalSetUrl(string url)
    {
      if (!this.isModifiable)
        throw new InvalidOperationException("UnityWebRequest has already been sent and its URL cannot be altered");
      UnityWebRequest.UnityWebRequestError err = this.SetUrl(url);
      if (err != 0)
        throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
    }

    /// <summary>
    ///   <para>The numeric HTTP response code returned by the server, such as 200, 404 or 500. (Read Only)</para>
    /// </summary>
    public extern long responseCode { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern float GetUploadProgress();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool IsExecuting();

    /// <summary>
    ///   <para>Returns a floating-point value between 0.0 and 1.0, indicating the progress of uploading body data to the server.</para>
    /// </summary>
    public float uploadProgress => !this.IsExecuting() && !this.isDone ? -1f : this.GetUploadProgress();

    /// <summary>
    ///   <para>Returns true while a UnityWebRequest’s configuration properties can be altered. (Read Only)</para>
    /// </summary>
    public extern bool isModifiable { [NativeMethod("IsModifiable"), MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true after the UnityWebRequest has finished communicating with the remote server. (Read Only)</para>
    /// </summary>
    public bool isDone => this.result != 0;

    /// <summary>
    ///   <para>Returns true after this UnityWebRequest encounters a system error. (Read Only)</para>
    /// </summary>
    [Obsolete("UnityWebRequest.isNetworkError is deprecated. Use (UnityWebRequest.result == UnityWebRequest.Result.ConnectionError) instead.", false)]
    public bool isNetworkError => this.result == UnityWebRequest.Result.ConnectionError;

    /// <summary>
    ///   <para>Returns true after this UnityWebRequest receives an HTTP response code indicating an error. (Read Only)</para>
    /// </summary>
    [Obsolete("UnityWebRequest.isHttpError is deprecated. Use (UnityWebRequest.result == UnityWebRequest.Result.ProtocolError) instead.", false)]
    public bool isHttpError => this.result == UnityWebRequest.Result.ProtocolError;

    /// <summary>
    ///   <para>The result of this UnityWebRequest.</para>
    /// </summary>
    public extern UnityWebRequest.Result result { [NativeMethod("GetResult"), MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern float GetDownloadProgress();

    /// <summary>
    ///   <para>Returns a floating-point value between 0.0 and 1.0, indicating the progress of downloading body data from the server. (Read Only)</para>
    /// </summary>
    public float downloadProgress => !this.IsExecuting() && !this.isDone ? -1f : this.GetDownloadProgress();

    /// <summary>
    ///   <para>Returns the number of bytes of body data the system has uploaded to the remote server. (Read Only)</para>
    /// </summary>
    public extern ulong uploadedBytes { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the number of bytes of body data the system has downloaded from the remote server. (Read Only)</para>
    /// </summary>
    public extern ulong downloadedBytes { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int GetRedirectLimit();

    [NativeThrows]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetRedirectLimitFromScripting(int limit);

    /// <summary>
    ///   <para>Indicates the number of redirects which this UnityWebRequest will follow before halting with a “Redirect Limit Exceeded” system error.</para>
    /// </summary>
    public int redirectLimit
    {
      get => this.GetRedirectLimit();
      set => this.SetRedirectLimitFromScripting(value);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool GetChunked();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetChunked(bool chunked);

    /// <summary>
    ///   <para>**Deprecated.**. HTTP2 and many HTTP1.1 servers don't support this; we recommend leaving it set to false (default).</para>
    /// </summary>
    [Obsolete("HTTP/2 and many HTTP/1.1 servers don't support this; we recommend leaving it set to false (default).", false)]
    public bool chunkedTransfer
    {
      get => this.GetChunked();
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent and its chunked transfer encoding setting cannot be altered");
        UnityWebRequest.UnityWebRequestError err = this.SetChunked(value);
        if (err != 0)
          throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
      }
    }

    /// <summary>
    ///   <para>Retrieves the value of a custom request header.</para>
    /// </summary>
    /// <param name="name">Name of the custom request header. Case-insensitive.</param>
    /// <returns>
    ///   <para>The value of the custom request header. If no custom header with a matching name has been set, returns an empty string.</para>
    /// </returns>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetRequestHeader(string name);

    [NativeMethod("SetRequestHeader")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern UnityWebRequest.UnityWebRequestError InternalSetRequestHeader(
      string name,
      string value);

    /// <summary>
    ///   <para>Set a HTTP request header to a custom value.</para>
    /// </summary>
    /// <param name="name">The key of the header to be set. Case-sensitive.</param>
    /// <param name="value">The header's intended value.</param>
    public void SetRequestHeader(string name, string value)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException("Cannot set a Request Header with a null or empty name");
      if (value == null)
        throw new ArgumentException("Cannot set a Request header with a null");
      if (!this.isModifiable)
        throw new InvalidOperationException("UnityWebRequest has already been sent and its request headers cannot be altered");
      UnityWebRequest.UnityWebRequestError err = this.InternalSetRequestHeader(name, value);
      if (err != 0)
        throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
    }

    /// <summary>
    ///   <para>Retrieves the value of a response header from the latest HTTP response received.</para>
    /// </summary>
    /// <param name="name">The name of the HTTP header to retrieve. Case-insensitive.</param>
    /// <returns>
    ///   <para>The value of the HTTP header from the latest HTTP response. If no header with a matching name has been received, or no responses have been received, returns null.</para>
    /// </returns>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string GetResponseHeader(string name);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string[] GetResponseHeaderKeys();

    /// <summary>
    ///   <para>Retrieves a dictionary containing all the response headers received by this UnityWebRequest in the latest HTTP response.</para>
    /// </summary>
    /// <returns>
    ///   <para>A dictionary containing all the response headers received in the latest HTTP response. If no responses have been received, returns null.</para>
    /// </returns>
    public Dictionary<string, string> GetResponseHeaders()
    {
      string[] responseHeaderKeys = this.GetResponseHeaderKeys();
      if (responseHeaderKeys == null || responseHeaderKeys.Length == 0)
        return (Dictionary<string, string>) null;
      Dictionary<string, string> responseHeaders = new Dictionary<string, string>(responseHeaderKeys.Length, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      for (int index = 0; index < responseHeaderKeys.Length; ++index)
      {
        string responseHeader = this.GetResponseHeader(responseHeaderKeys[index]);
        responseHeaders.Add(responseHeaderKeys[index], responseHeader);
      }
      return responseHeaders;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetUploadHandler(UploadHandler uh);

    /// <summary>
    ///   <para>Holds a reference to the UploadHandler object which manages body data to be uploaded to the remote server.</para>
    /// </summary>
    public UploadHandler uploadHandler
    {
      get => this.m_UploadHandler;
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the upload handler");
        UnityWebRequest.UnityWebRequestError err = this.SetUploadHandler(value);
        if (err != 0)
          throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
        this.m_UploadHandler = value;
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetDownloadHandler(DownloadHandler dh);

    /// <summary>
    ///   <para>Holds a reference to a DownloadHandler object, which manages body data received from the remote server by this UnityWebRequest.</para>
    /// </summary>
    public DownloadHandler downloadHandler
    {
      get => this.m_DownloadHandler;
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the download handler");
        UnityWebRequest.UnityWebRequestError err = this.SetDownloadHandler(value);
        if (err != 0)
          throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
        this.m_DownloadHandler = value;
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetCertificateHandler(CertificateHandler ch);

    /// <summary>
    ///   <para>Holds a reference to a CertificateHandler object, which manages certificate validation for this UnityWebRequest.</para>
    /// </summary>
    public CertificateHandler certificateHandler
    {
      get => this.m_CertificateHandler;
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the certificate handler");
        UnityWebRequest.UnityWebRequestError err = this.SetCertificateHandler(value);
        if (err != 0)
          throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
        this.m_CertificateHandler = value;
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int GetTimeoutMsec();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetTimeoutMsec(int timeout);

    /// <summary>
    ///   <para>Sets UnityWebRequest to attempt to abort after the number of seconds in timeout have passed.</para>
    /// </summary>
    public int timeout
    {
      get => this.GetTimeoutMsec() / 1000;
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the timeout");
        value = Math.Max(value, 0);
        UnityWebRequest.UnityWebRequestError err = this.SetTimeoutMsec(value * 1000);
        if (err != 0)
          throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(err));
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool GetSuppressErrorsToConsole();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern UnityWebRequest.UnityWebRequestError SetSuppressErrorsToConsole(bool suppress);

    internal bool suppressErrorsToConsole
    {
      get => this.GetSuppressErrorsToConsole();
      set
      {
        if (!this.isModifiable)
          throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the timeout");
        UnityWebRequest.UnityWebRequestError console = this.SetSuppressErrorsToConsole(value);
        if (console != 0)
          throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(console));
      }
    }

    [Obsolete("UnityWebRequest.isError has been renamed to isNetworkError for clarity. (UnityUpgradable) -> isNetworkError", false)]
    public bool isError => this.isNetworkError;

    /// <summary>
    ///   <para>Create a UnityWebRequest for HTTP GET.</para>
    /// </summary>
    /// <param name="uri">The URI of the resource to retrieve via HTTP GET.</param>
    /// <returns>
    ///   <para>An object that retrieves data from the uri.</para>
    /// </returns>
    public static UnityWebRequest Get(string uri) => new UnityWebRequest(uri, "GET", (DownloadHandler) new DownloadHandlerBuffer(), (UploadHandler) null);

    /// <summary>
    ///   <para>Create a UnityWebRequest for HTTP GET.</para>
    /// </summary>
    /// <param name="uri">The URI of the resource to retrieve via HTTP GET.</param>
    /// <returns>
    ///   <para>An object that retrieves data from the uri.</para>
    /// </returns>
    public static UnityWebRequest Get(Uri uri) => new UnityWebRequest(uri, "GET", (DownloadHandler) new DownloadHandlerBuffer(), (UploadHandler) null);

    /// <summary>
    ///   <para>Creates a UnityWebRequest configured for HTTP DELETE.</para>
    /// </summary>
    /// <param name="uri">The URI to which a DELETE request should be sent.</param>
    /// <returns>
    ///   <para>A UnityWebRequest configured to send an HTTP DELETE request.</para>
    /// </returns>
    public static UnityWebRequest Delete(string uri) => new UnityWebRequest(uri, "DELETE");

    public static UnityWebRequest Delete(Uri uri) => new UnityWebRequest(uri, "DELETE");

    /// <summary>
    ///   <para>Creates a UnityWebRequest configured to send a HTTP HEAD request.</para>
    /// </summary>
    /// <param name="uri">The URI to which to send a HTTP HEAD request.</param>
    /// <returns>
    ///   <para>A UnityWebRequest configured to transmit a HTTP HEAD request.</para>
    /// </returns>
    public static UnityWebRequest Head(string uri) => new UnityWebRequest(uri, "HEAD");

    public static UnityWebRequest Head(Uri uri) => new UnityWebRequest(uri, "HEAD");

    /// <summary>
    ///   <para>Creates a UnityWebRequest intended to download an image via HTTP GET and create a Texture based on the retrieved data.</para>
    /// </summary>
    /// <param name="uri">The URI of the image to download.</param>
    /// <param name="nonReadable">If true, the texture's raw data will not be accessible to script. This can conserve memory. Default: false.</param>
    /// <returns>
    ///   <para>A UnityWebRequest properly configured to download an image and convert it to a Texture.</para>
    /// </returns>
    [Obsolete("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestTexture.GetTexture(*)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static UnityWebRequest GetTexture(string uri) => throw new NotSupportedException("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead.");

    /// <summary>
    ///   <para>Creates a UnityWebRequest intended to download an image via HTTP GET and create a Texture based on the retrieved data.</para>
    /// </summary>
    /// <param name="uri">The URI of the image to download.</param>
    /// <param name="nonReadable">If true, the texture's raw data will not be accessible to script. This can conserve memory. Default: false.</param>
    /// <returns>
    ///   <para>A UnityWebRequest properly configured to download an image and convert it to a Texture.</para>
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestTexture.GetTexture(*)", true)]
    public static UnityWebRequest GetTexture(string uri, bool nonReadable) => throw new NotSupportedException("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead.");

    /// <summary>
    ///   <para>OBSOLETE. Use UnityWebRequestMultimedia.GetAudioClip().</para>
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="audioType"></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("UnityWebRequest.GetAudioClip is obsolete. Use UnityWebRequestMultimedia.GetAudioClip instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestMultimedia.GetAudioClip(*)", true)]
    public static UnityWebRequest GetAudioClip(string uri, AudioType audioType) => (UnityWebRequest) null;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
    public static UnityWebRequest GetAssetBundle(string uri) => (UnityWebRequest) null;

    /// <summary>
    ///   <para>Deprecated. Replaced by UnityWebRequestAssetBundle.GetAssetBundle.</para>
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="crc"></param>
    /// <param name="version"></param>
    /// <param name="hash"></param>
    /// <param name="cachedAssetBundle"></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
    public static UnityWebRequest GetAssetBundle(string uri, uint crc) => (UnityWebRequest) null;

    /// <summary>
    ///   <para>Deprecated. Replaced by UnityWebRequestAssetBundle.GetAssetBundle.</para>
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="crc"></param>
    /// <param name="version"></param>
    /// <param name="hash"></param>
    /// <param name="cachedAssetBundle"></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
    public static UnityWebRequest GetAssetBundle(string uri, uint version, uint crc) => (UnityWebRequest) null;

    /// <summary>
    ///   <para>Deprecated. Replaced by UnityWebRequestAssetBundle.GetAssetBundle.</para>
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="crc"></param>
    /// <param name="version"></param>
    /// <param name="hash"></param>
    /// <param name="cachedAssetBundle"></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
    public static UnityWebRequest GetAssetBundle(string uri, Hash128 hash, uint crc) => (UnityWebRequest) null;

    /// <summary>
    ///   <para>Deprecated. Replaced by UnityWebRequestAssetBundle.GetAssetBundle.</para>
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="crc"></param>
    /// <param name="version"></param>
    /// <param name="hash"></param>
    /// <param name="cachedAssetBundle"></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
    public static UnityWebRequest GetAssetBundle(
      string uri,
      CachedAssetBundle cachedAssetBundle,
      uint crc)
    {
      return (UnityWebRequest) null;
    }

    /// <summary>
    ///   <para>Creates a UnityWebRequest configured to upload raw data to a remote server via HTTP PUT.</para>
    /// </summary>
    /// <param name="uri">The URI to which the data will be sent.</param>
    /// <param name="bodyData">The data to transmit to the remote server.
    /// 
    /// If a string, the string will be converted to raw bytes via &lt;a href="https:msdn.microsoft.comen-uslibrarysystem.text.encoding.utf8"&gt;System.Text.Encoding.UTF8&lt;a&gt;.</param>
    /// <returns>
    ///   <para>A UnityWebRequest configured to transmit bodyData to uri via HTTP PUT.</para>
    /// </returns>
    public static UnityWebRequest Put(string uri, byte[] bodyData) => new UnityWebRequest(uri, "PUT", (DownloadHandler) new DownloadHandlerBuffer(), (UploadHandler) new UploadHandlerRaw(bodyData));

    public static UnityWebRequest Put(Uri uri, byte[] bodyData) => new UnityWebRequest(uri, "PUT", (DownloadHandler) new DownloadHandlerBuffer(), (UploadHandler) new UploadHandlerRaw(bodyData));

    /// <summary>
    ///   <para>Creates a UnityWebRequest configured to upload raw data to a remote server via HTTP PUT.</para>
    /// </summary>
    /// <param name="uri">The URI to which the data will be sent.</param>
    /// <param name="bodyData">The data to transmit to the remote server.
    /// 
    /// If a string, the string will be converted to raw bytes via &lt;a href="https:msdn.microsoft.comen-uslibrarysystem.text.encoding.utf8"&gt;System.Text.Encoding.UTF8&lt;a&gt;.</param>
    /// <returns>
    ///   <para>A UnityWebRequest configured to transmit bodyData to uri via HTTP PUT.</para>
    /// </returns>
    public static UnityWebRequest Put(string uri, string bodyData) => new UnityWebRequest(uri, "PUT", (DownloadHandler) new DownloadHandlerBuffer(), (UploadHandler) new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyData)));

    public static UnityWebRequest Put(Uri uri, string bodyData) => new UnityWebRequest(uri, "PUT", (DownloadHandler) new DownloadHandlerBuffer(), (UploadHandler) new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyData)));

    /// <summary>
    ///   <para>Creates a UnityWebRequest configured to send form data to a server via HTTP POST.</para>
    /// </summary>
    /// <param name="uri">The target URI to which form data will be transmitted.</param>
    /// <param name="postData">Form body data. Will be URLEncoded prior to transmission.</param>
    /// <returns>
    ///   <para>A UnityWebRequest configured to send form data to uri via POST.</para>
    /// </returns>
    public static UnityWebRequest Post(string uri, string postData)
    {
      UnityWebRequest request = new UnityWebRequest(uri, "POST");
      UnityWebRequest.SetupPost(request, postData);
      return request;
    }

    public static UnityWebRequest Post(Uri uri, string postData)
    {
      UnityWebRequest request = new UnityWebRequest(uri, "POST");
      UnityWebRequest.SetupPost(request, postData);
      return request;
    }

    private static void SetupPost(UnityWebRequest request, string postData)
    {
      request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      if (string.IsNullOrEmpty(postData))
        return;
      byte[] bytes = Encoding.UTF8.GetBytes(WWWTranscoder.DataEncode(postData, Encoding.UTF8));
      request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bytes);
      request.uploadHandler.contentType = "application/x-www-form-urlencoded";
    }

    /// <summary>
    ///   <para>Create a UnityWebRequest configured to send form data to a server via HTTP POST.</para>
    /// </summary>
    /// <param name="uri">The target URI to which form data will be transmitted.</param>
    /// <param name="formData">Form fields or files encapsulated in a WWWForm object, for formatting and transmission to the remote server.</param>
    /// <returns>
    ///   <para>A UnityWebRequest configured to send form data to uri via POST.</para>
    /// </returns>
    public static UnityWebRequest Post(string uri, WWWForm formData)
    {
      UnityWebRequest request = new UnityWebRequest(uri, "POST");
      UnityWebRequest.SetupPost(request, formData);
      return request;
    }

    public static UnityWebRequest Post(Uri uri, WWWForm formData)
    {
      UnityWebRequest request = new UnityWebRequest(uri, "POST");
      UnityWebRequest.SetupPost(request, formData);
      return request;
    }

    private static void SetupPost(UnityWebRequest request, WWWForm formData)
    {
      request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      if (formData == null)
        return;
      byte[] data = formData.data;
      if (data.Length == 0)
        data = (byte[]) null;
      if (data != null)
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
      foreach (KeyValuePair<string, string> header in formData.headers)
        request.SetRequestHeader(header.Key, header.Value);
    }

    public static UnityWebRequest Post(
      string uri,
      List<IMultipartFormSection> multipartFormSections)
    {
      byte[] boundary = UnityWebRequest.GenerateBoundary();
      return UnityWebRequest.Post(uri, multipartFormSections, boundary);
    }

    public static UnityWebRequest Post(Uri uri, List<IMultipartFormSection> multipartFormSections)
    {
      byte[] boundary = UnityWebRequest.GenerateBoundary();
      return UnityWebRequest.Post(uri, multipartFormSections, boundary);
    }

    public static UnityWebRequest Post(
      string uri,
      List<IMultipartFormSection> multipartFormSections,
      byte[] boundary)
    {
      UnityWebRequest request = new UnityWebRequest(uri, "POST");
      UnityWebRequest.SetupPost(request, multipartFormSections, boundary);
      return request;
    }

    public static UnityWebRequest Post(
      Uri uri,
      List<IMultipartFormSection> multipartFormSections,
      byte[] boundary)
    {
      UnityWebRequest request = new UnityWebRequest(uri, "POST");
      UnityWebRequest.SetupPost(request, multipartFormSections, boundary);
      return request;
    }

    private static void SetupPost(
      UnityWebRequest request,
      List<IMultipartFormSection> multipartFormSections,
      byte[] boundary)
    {
      request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      byte[] data = (byte[]) null;
      if (multipartFormSections != null && multipartFormSections.Count != 0)
        data = UnityWebRequest.SerializeFormSections(multipartFormSections, boundary);
      if (data == null)
        return;
      UploadHandler uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
      uploadHandler.contentType = "multipart/form-data; boundary=" + Encoding.UTF8.GetString(boundary, 0, boundary.Length);
      request.uploadHandler = uploadHandler;
    }

    public static UnityWebRequest Post(string uri, Dictionary<string, string> formFields)
    {
      UnityWebRequest request = new UnityWebRequest(uri, "POST");
      UnityWebRequest.SetupPost(request, formFields);
      return request;
    }

    public static UnityWebRequest Post(Uri uri, Dictionary<string, string> formFields)
    {
      UnityWebRequest request = new UnityWebRequest(uri, "POST");
      UnityWebRequest.SetupPost(request, formFields);
      return request;
    }

    private static void SetupPost(UnityWebRequest request, Dictionary<string, string> formFields)
    {
      request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
      byte[] data = (byte[]) null;
      if (formFields != null && formFields.Count != 0)
        data = UnityWebRequest.SerializeSimpleForm(formFields);
      if (data == null)
        return;
      UploadHandler uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
      uploadHandler.contentType = "application/x-www-form-urlencoded";
      request.uploadHandler = uploadHandler;
    }

    /// <summary>
    ///   <para>Escapes characters in a string to ensure they are URL-friendly.</para>
    /// </summary>
    /// <param name="s">A string with characters to be escaped.</param>
    /// <param name="e">The text encoding to use.</param>
    public static string EscapeURL(string s) => UnityWebRequest.EscapeURL(s, Encoding.UTF8);

    /// <summary>
    ///   <para>Escapes characters in a string to ensure they are URL-friendly.</para>
    /// </summary>
    /// <param name="s">A string with characters to be escaped.</param>
    /// <param name="e">The text encoding to use.</param>
    public static string EscapeURL(string s, Encoding e)
    {
      switch (s)
      {
        case null:
          return (string) null;
        case "":
          return "";
        default:
          if (e == null)
            return (string) null;
          byte[] bytes = WWWTranscoder.URLEncode(e.GetBytes(s));
          return e.GetString(bytes);
      }
    }

    /// <summary>
    ///   <para>Converts URL-friendly escape sequences back to normal text.</para>
    /// </summary>
    /// <param name="s">A string containing escaped characters.</param>
    /// <param name="e">The text encoding to use.</param>
    public static string UnEscapeURL(string s) => UnityWebRequest.UnEscapeURL(s, Encoding.UTF8);

    /// <summary>
    ///   <para>Converts URL-friendly escape sequences back to normal text.</para>
    /// </summary>
    /// <param name="s">A string containing escaped characters.</param>
    /// <param name="e">The text encoding to use.</param>
    public static string UnEscapeURL(string s, Encoding e)
    {
      if (s == null)
        return (string) null;
      if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
        return s;
      byte[] bytes = WWWTranscoder.URLDecode(e.GetBytes(s));
      return e.GetString(bytes);
    }

    public static byte[] SerializeFormSections(
      List<IMultipartFormSection> multipartFormSections,
      byte[] boundary)
    {
      if (multipartFormSections == null || multipartFormSections.Count == 0)
        return (byte[]) null;
      byte[] bytes1 = Encoding.UTF8.GetBytes("\r\n");
      byte[] bytes2 = WWWForm.DefaultEncoding.GetBytes("--");
      int capacity = 0;
      foreach (IMultipartFormSection multipartFormSection in multipartFormSections)
        capacity += 64 + multipartFormSection.sectionData.Length;
      List<byte> byteList = new List<byte>(capacity);
      foreach (IMultipartFormSection multipartFormSection in multipartFormSections)
      {
        string str1 = "form-data";
        string sectionName = multipartFormSection.sectionName;
        string fileName = multipartFormSection.fileName;
        string str2 = "Content-Disposition: " + str1;
        if (!string.IsNullOrEmpty(sectionName))
          str2 = str2 + "; name=\"" + sectionName + "\"";
        if (!string.IsNullOrEmpty(fileName))
          str2 = str2 + "; filename=\"" + fileName + "\"";
        string s = str2 + "\r\n";
        string contentType = multipartFormSection.contentType;
        if (!string.IsNullOrEmpty(contentType))
          s = s + "Content-Type: " + contentType + "\r\n";
        byteList.AddRange((IEnumerable<byte>) bytes1);
        byteList.AddRange((IEnumerable<byte>) bytes2);
        byteList.AddRange((IEnumerable<byte>) boundary);
        byteList.AddRange((IEnumerable<byte>) bytes1);
        byteList.AddRange((IEnumerable<byte>) Encoding.UTF8.GetBytes(s));
        byteList.AddRange((IEnumerable<byte>) bytes1);
        byteList.AddRange((IEnumerable<byte>) multipartFormSection.sectionData);
      }
      byteList.AddRange((IEnumerable<byte>) bytes1);
      byteList.AddRange((IEnumerable<byte>) bytes2);
      byteList.AddRange((IEnumerable<byte>) boundary);
      byteList.AddRange((IEnumerable<byte>) bytes2);
      byteList.AddRange((IEnumerable<byte>) bytes1);
      return byteList.ToArray();
    }

    /// <summary>
    ///   <para>Generate a random 40-byte array for use as a multipart form boundary.</para>
    /// </summary>
    /// <returns>
    ///   <para>40 random bytes, guaranteed to contain only printable ASCII values.</para>
    /// </returns>
    public static byte[] GenerateBoundary()
    {
      byte[] boundary = new byte[40];
      for (int index = 0; index < 40; ++index)
      {
        int num = UnityEngine.Random.Range(48, 110);
        if (num > 57)
          num += 7;
        if (num > 90)
          num += 6;
        boundary[index] = (byte) num;
      }
      return boundary;
    }

    public static byte[] SerializeSimpleForm(Dictionary<string, string> formFields)
    {
      string s = "";
      foreach (KeyValuePair<string, string> formField in formFields)
      {
        if (s.Length > 0)
          s += "&";
        s = s + WWWTranscoder.DataEncode(formField.Key) + "=" + WWWTranscoder.DataEncode(formField.Value);
      }
      return Encoding.UTF8.GetBytes(s);
    }

    internal enum UnityWebRequestMethod
    {
      Get,
      Post,
      Put,
      Head,
      Custom,
    }

    internal enum UnityWebRequestError
    {
      OK,
      Unknown,
      SDKError,
      UnsupportedProtocol,
      MalformattedUrl,
      CannotResolveProxy,
      CannotResolveHost,
      CannotConnectToHost,
      AccessDenied,
      GenericHttpError,
      WriteError,
      ReadError,
      OutOfMemory,
      Timeout,
      HTTPPostError,
      SSLCannotConnect,
      Aborted,
      TooManyRedirects,
      ReceivedNoData,
      SSLNotSupported,
      FailedToSendData,
      FailedToReceiveData,
      SSLCertificateError,
      SSLCipherNotAvailable,
      SSLCACertError,
      UnrecognizedContentEncoding,
      LoginFailed,
      SSLShutdownFailed,
      NoInternetConnection,
    }

    /// <summary>
    ///   <para>Defines codes describing the possible outcomes of a UnityWebRequest.</para>
    /// </summary>
    public enum Result
    {
      /// <summary>
      ///   <para>The request hasn't finished yet.</para>
      /// </summary>
      InProgress,
      /// <summary>
      ///   <para>The request succeeded.</para>
      /// </summary>
      Success,
      /// <summary>
      ///   <para>Failed to communicate with the server. For example, the request couldn't connect or it could not establish a secure channel.</para>
      /// </summary>
      ConnectionError,
      /// <summary>
      ///   <para>The server returned an error response. The request succeeded in communicating with the server, but received an error as defined by the connection protocol.</para>
      /// </summary>
      ProtocolError,
      /// <summary>
      ///   <para>Error processing data. The request succeeded in communicating with the server, but encountered an error when processing the received data. For example, the data was corrupted or not in the correct format.</para>
      /// </summary>
      DataProcessingError,
    }
  }
}
