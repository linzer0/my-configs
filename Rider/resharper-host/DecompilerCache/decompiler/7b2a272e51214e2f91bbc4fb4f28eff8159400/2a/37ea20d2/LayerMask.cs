// Decompiled with JetBrains decompiler
// Type: UnityEngine.LayerMask
// Assembly: UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7B2A272E-5121-4E2F-91BB-C4FB4F28EFF8
// Assembly location: E:\Programs\Gamedev\Editor\2021.3.17f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll
// XML documentation location: E:\Programs\Gamedev\Editor\2021.3.17f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.xml

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Specifies Layers to use in a Physics.Raycast.</para>
  /// </summary>
  [RequiredByNativeCode(Optional = true, GenerateProxy = true)]
  [NativeHeader("Runtime/BaseClasses/BitField.h")]
  [NativeHeader("Runtime/BaseClasses/TagManager.h")]
  [NativeClass("BitField", "struct BitField;")]
  public struct LayerMask
  {
    [NativeName("m_Bits")]
    private int m_Mask;

    public static implicit operator int(LayerMask mask) => mask.m_Mask;

    public static implicit operator LayerMask(int intVal)
    {
      LayerMask layerMask;
      layerMask.m_Mask = intVal;
      return layerMask;
    }

    /// <summary>
    ///   <para>Converts a layer mask value to an integer value.</para>
    /// </summary>
    public int value
    {
      get => this.m_Mask;
      set => this.m_Mask = value;
    }

    /// <summary>
    ///   <para>Given a layer number, returns the name of the layer as defined in either a Builtin or a User Layer in the.</para>
    /// </summary>
    /// <param name="layer"></param>
    [NativeMethod("LayerToString")]
    [StaticAccessor("GetTagManager()", StaticAccessorType.Dot)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string LayerToName(int layer);

    /// <summary>
    ///   <para>Given a layer name, returns the layer index as defined by either a Builtin or a User Layer in the.</para>
    /// </summary>
    /// <param name="layerName"></param>
    [StaticAccessor("GetTagManager()", StaticAccessorType.Dot)]
    [NativeMethod("StringToLayer")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int NameToLayer(string layerName);

    /// <summary>
    ///   <para>Given a set of layer names as defined by either a Builtin or a User Layer in the, returns the equivalent layer mask for all of them.</para>
    /// </summary>
    /// <param name="layerNames">List of layer names to convert to a layer mask.</param>
    /// <returns>
    ///   <para>The layer mask created from the layerNames.</para>
    /// </returns>
    public static int GetMask(params string[] layerNames)
    {
      if (layerNames == null)
        throw new ArgumentNullException(nameof (layerNames));
      int mask = 0;
      foreach (string layerName in layerNames)
      {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer != -1)
          mask |= 1 << layer;
      }
      return mask;
    }
  }
}
