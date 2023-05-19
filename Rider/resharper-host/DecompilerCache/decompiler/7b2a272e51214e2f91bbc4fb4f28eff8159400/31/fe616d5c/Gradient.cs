// Decompiled with JetBrains decompiler
// Type: UnityEngine.Gradient
// Assembly: UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7B2A272E-5121-4E2F-91BB-C4FB4F28EFF8
// Assembly location: E:\Programs\Gamedev\Editor\2021.3.17f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll
// XML documentation location: E:\Programs\Gamedev\Editor\2021.3.17f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.xml

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Gradient used for animating colors.</para>
  /// </summary>
  [RequiredByNativeCode]
  [NativeHeader("Runtime/Export/Math/Gradient.bindings.h")]
  [StructLayout(LayoutKind.Sequential)]
  public class Gradient : IEquatable<Gradient>
  {
    internal IntPtr m_Ptr;

    [FreeFunction(Name = "Gradient_Bindings::Init", IsThreadSafe = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr Init();

    [FreeFunction(Name = "Gradient_Bindings::Cleanup", IsThreadSafe = true, HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Cleanup();

    [FreeFunction("Gradient_Bindings::Internal_Equals", IsThreadSafe = true, HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool Internal_Equals(IntPtr other);

    /// <summary>
    ///   <para>Create a new Gradient object.</para>
    /// </summary>
    [RequiredByNativeCode]
    public Gradient() => this.m_Ptr = Gradient.Init();

    ~Gradient() => this.Cleanup();

    /// <summary>
    ///   <para>Calculate color at a given time.</para>
    /// </summary>
    /// <param name="time">Time of the key (0 - 1).</param>
    [FreeFunction(Name = "Gradient_Bindings::Evaluate", IsThreadSafe = true, HasExplicitThis = true)]
    public Color Evaluate(float time)
    {
      Color ret;
      this.Evaluate_Injected(time, out ret);
      return ret;
    }

    /// <summary>
    ///   <para>All color keys defined in the gradient.</para>
    /// </summary>
    public extern GradientColorKey[] colorKeys { [FreeFunction("Gradient_Bindings::GetColorKeys", IsThreadSafe = true, HasExplicitThis = true), MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction("Gradient_Bindings::SetColorKeys", IsThreadSafe = true, HasExplicitThis = true), MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>All alpha keys defined in the gradient.</para>
    /// </summary>
    public extern GradientAlphaKey[] alphaKeys { [FreeFunction("Gradient_Bindings::GetAlphaKeys", IsThreadSafe = true, HasExplicitThis = true), MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction("Gradient_Bindings::SetAlphaKeys", IsThreadSafe = true, HasExplicitThis = true), MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Control how the gradient is evaluated.</para>
    /// </summary>
    [NativeProperty(IsThreadSafe = true)]
    public extern GradientMode mode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [NativeProperty(IsThreadSafe = true)]
    internal Color constantColor
    {
      get
      {
        Color ret;
        this.get_constantColor_Injected(out ret);
        return ret;
      }
      set => this.set_constantColor_Injected(ref value);
    }

    /// <summary>
    ///   <para>Setup Gradient with an array of color keys and alpha keys.</para>
    /// </summary>
    /// <param name="colorKeys">Color keys of the gradient (maximum 8 color keys).</param>
    /// <param name="alphaKeys">Alpha keys of the gradient (maximum 8 alpha keys).</param>
    [FreeFunction(Name = "Gradient_Bindings::SetKeys", IsThreadSafe = true, HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys);

    public override bool Equals(object o)
    {
      if (o == null)
        return false;
      if (this == o)
        return true;
      return o.GetType() == this.GetType() && this.Equals((Gradient) o);
    }

    public bool Equals(Gradient other)
    {
      if (other == null)
        return false;
      return this == other || this.m_Ptr.Equals((object) other.m_Ptr) || this.Internal_Equals(other.m_Ptr);
    }

    public override int GetHashCode() => this.m_Ptr.GetHashCode();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Evaluate_Injected(float time, out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_constantColor_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_constantColor_Injected(ref Color value);
  }
}
