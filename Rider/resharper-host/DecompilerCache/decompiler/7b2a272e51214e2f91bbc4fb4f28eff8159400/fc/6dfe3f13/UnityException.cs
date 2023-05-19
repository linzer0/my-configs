// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnityException
// Assembly: UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7B2A272E-5121-4E2F-91BB-C4FB4F28EFF8
// Assembly location: E:\Programs\Gamedev\Editor\2021.3.17f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll
// XML documentation location: E:\Programs\Gamedev\Editor\2021.3.17f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.xml

using System;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [RequiredByNativeCode]
  [Serializable]
  public class UnityException : SystemException
  {
    private const int Result = -2147467261;
    private string unityStackTrace;

    public UnityException()
      : base("A Unity Runtime error occurred!")
    {
      this.HResult = -2147467261;
    }

    public UnityException(string message)
      : base(message)
    {
      this.HResult = -2147467261;
    }

    public UnityException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.HResult = -2147467261;
    }

    protected UnityException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
