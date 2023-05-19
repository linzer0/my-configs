// Decompiled with JetBrains decompiler
// Type: UnityEngine.Component
// Assembly: UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7B2A272E-5121-4E2F-91BB-C4FB4F28EFF8
// Assembly location: E:\Programs\Gamedev\Editor\2021.3.17f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll
// XML documentation location: E:\Programs\Gamedev\Editor\2021.3.17f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.xml

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Base class for everything attached to a GameObject.</para>
  /// </summary>
  [NativeClass("Unity::Component")]
  [NativeHeader("Runtime/Export/Scripting/Component.bindings.h")]
  [RequiredByNativeCode]
  public class Component : Object
  {
    /// <summary>
    ///   <para>The Transform attached to this GameObject.</para>
    /// </summary>
    public extern Transform transform { [FreeFunction("GetTransform", HasExplicitThis = true, ThrowsException = true), MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The game object this component is attached to. A component is always attached to a game object.</para>
    /// </summary>
    public extern GameObject gameObject { [FreeFunction("GetGameObject", HasExplicitThis = true), MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the component of type if the GameObject has one attached.</para>
    /// </summary>
    /// <param name="type">The type of Component to retrieve.</param>
    /// <returns>
    ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
    /// </returns>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public Component GetComponent(System.Type type) => this.gameObject.GetComponent(type);

    [FreeFunction(HasExplicitThis = true, ThrowsException = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void GetComponentFastPath(System.Type type, IntPtr oneFurtherThanResultValue);

    [SecuritySafeCritical]
    public unsafe T GetComponent<T>()
    {
      CastHelper<T> castHelper = new CastHelper<T>();
      this.GetComponentFastPath(typeof (T), new IntPtr((void*) &castHelper.onePointerFurtherThanT));
      return castHelper.t;
    }

    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public bool TryGetComponent(System.Type type, out Component component) => this.gameObject.TryGetComponent(type, out component);

    [SecuritySafeCritical]
    public bool TryGetComponent<T>(out T component) => this.gameObject.TryGetComponent<T>(out component);

    /// <summary>
    ///   <para>To improve the performance of your code, consider using GetComponent with a type instead of a string.</para>
    /// </summary>
    /// <param name="type">The name of the type of Component to get.</param>
    /// <returns>
    ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
    /// </returns>
    [FreeFunction(HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Component GetComponent(string type);

    /// <summary>
    ///   <para>Returns the Component of type in the GameObject or any of its children using depth first search.</para>
    /// </summary>
    /// <param name="t">The type of Component to retrieve.</param>
    /// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
    /// <returns>
    ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
    /// </returns>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public Component GetComponentInChildren(System.Type t, bool includeInactive) => this.gameObject.GetComponentInChildren(t, includeInactive);

    /// <summary>
    ///   <para>Returns the Component of type in the GameObject or any of its children using depth first search.</para>
    /// </summary>
    /// <param name="t">The type of Component to retrieve.</param>
    /// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
    /// <returns>
    ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
    /// </returns>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public Component GetComponentInChildren(System.Type t) => this.GetComponentInChildren(t, false);

    public T GetComponentInChildren<T>([UnityEngine.Internal.DefaultValue("false")] bool includeInactive) => (T) this.GetComponentInChildren(typeof (T), includeInactive);

    [ExcludeFromDocs]
    public T GetComponentInChildren<T>() => (T) this.GetComponentInChildren(typeof (T), false);

    /// <summary>
    ///   <para>Returns all components of Type type in the GameObject or any of its children using depth first search. Works recursively.</para>
    /// </summary>
    /// <param name="t">The type of Component to retrieve.</param>
    /// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set. includeInactive decides which children of the GameObject will be searched.  The GameObject that you call GetComponentsInChildren on is always searched regardless. Default is false.</param>
    public Component[] GetComponentsInChildren(System.Type t, bool includeInactive) => this.gameObject.GetComponentsInChildren(t, includeInactive);

    [ExcludeFromDocs]
    public Component[] GetComponentsInChildren(System.Type t) => this.gameObject.GetComponentsInChildren(t, false);

    public T[] GetComponentsInChildren<T>(bool includeInactive) => this.gameObject.GetComponentsInChildren<T>(includeInactive);

    public void GetComponentsInChildren<T>(bool includeInactive, List<T> result) => this.gameObject.GetComponentsInChildren<T>(includeInactive, result);

    public T[] GetComponentsInChildren<T>() => this.GetComponentsInChildren<T>(false);

    public void GetComponentsInChildren<T>(List<T> results) => this.GetComponentsInChildren<T>(false, results);

    /// <summary>
    ///   <para>Returns the Component of type in the GameObject or any of its parents.</para>
    /// </summary>
    /// <param name="t">The type of Component to retrieve.</param>
    /// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
    /// <returns>
    ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
    /// </returns>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public Component GetComponentInParent(System.Type t, bool includeInactive) => this.gameObject.GetComponentInParent(t, includeInactive);

    /// <summary>
    ///   <para>Returns the Component of type in the GameObject or any of its parents.</para>
    /// </summary>
    /// <param name="t">The type of Component to retrieve.</param>
    /// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
    /// <returns>
    ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
    /// </returns>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public Component GetComponentInParent(System.Type t) => this.gameObject.GetComponentInParent(t, false);

    public T GetComponentInParent<T>([UnityEngine.Internal.DefaultValue("false")] bool includeInactive) => (T) this.GetComponentInParent(typeof (T), includeInactive);

    public T GetComponentInParent<T>() => (T) this.GetComponentInParent(typeof (T), false);

    /// <summary>
    ///   <para>Returns all components of Type type in the GameObject or any of its parents.</para>
    /// </summary>
    /// <param name="t">The type of Component to retrieve.</param>
    /// <param name="includeInactive">Should inactive Components be included in the found set?</param>
    public Component[] GetComponentsInParent(System.Type t, [UnityEngine.Internal.DefaultValue("false")] bool includeInactive) => this.gameObject.GetComponentsInParent(t, includeInactive);

    [ExcludeFromDocs]
    public Component[] GetComponentsInParent(System.Type t) => this.GetComponentsInParent(t, false);

    public T[] GetComponentsInParent<T>(bool includeInactive) => this.gameObject.GetComponentsInParent<T>(includeInactive);

    public void GetComponentsInParent<T>(bool includeInactive, List<T> results) => this.gameObject.GetComponentsInParent<T>(includeInactive, results);

    public T[] GetComponentsInParent<T>() => this.GetComponentsInParent<T>(false);

    /// <summary>
    ///   <para>Returns all components of Type type in the GameObject.</para>
    /// </summary>
    /// <param name="type">The type of Component to retrieve.</param>
    public Component[] GetComponents(System.Type type) => this.gameObject.GetComponents(type);

    [FreeFunction(HasExplicitThis = true, ThrowsException = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetComponentsForListInternal(System.Type searchType, object resultList);

    public void GetComponents(System.Type type, List<Component> results) => this.GetComponentsForListInternal(type, (object) results);

    public void GetComponents<T>(List<T> results) => this.GetComponentsForListInternal(typeof (T), (object) results);

    /// <summary>
    ///   <para>The tag of this game object.</para>
    /// </summary>
    public string tag
    {
      get => this.gameObject.tag;
      set => this.gameObject.tag = value;
    }

    public T[] GetComponents<T>() => this.gameObject.GetComponents<T>();

    /// <summary>
    ///   <para>Checks the GameObject's tag against the defined tag.</para>
    /// </summary>
    /// <param name="tag">The tag to compare.</param>
    /// <returns>
    ///   <para>Returns true if GameObject has same tag. Returns false otherwise.</para>
    /// </returns>
    public bool CompareTag(string tag) => this.gameObject.CompareTag(tag);

    [FreeFunction(HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern Component GetCoupledComponent();

    [FreeFunction(HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool IsCoupledComponent();

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
    /// </summary>
    /// <param name="methodName">Name of method to call.</param>
    /// <param name="value">Optional parameter value for the method.</param>
    /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
    [FreeFunction(HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SendMessageUpwards(
      string methodName,
      [UnityEngine.Internal.DefaultValue("null")] object value,
      [UnityEngine.Internal.DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
    /// </summary>
    /// <param name="methodName">Name of method to call.</param>
    /// <param name="value">Optional parameter value for the method.</param>
    /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
    [ExcludeFromDocs]
    public void SendMessageUpwards(string methodName, object value) => this.SendMessageUpwards(methodName, value, SendMessageOptions.RequireReceiver);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
    /// </summary>
    /// <param name="methodName">Name of method to call.</param>
    /// <param name="value">Optional parameter value for the method.</param>
    /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
    [ExcludeFromDocs]
    public void SendMessageUpwards(string methodName) => this.SendMessageUpwards(methodName, (object) null, SendMessageOptions.RequireReceiver);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
    /// </summary>
    /// <param name="methodName">Name of method to call.</param>
    /// <param name="value">Optional parameter value for the method.</param>
    /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
    public void SendMessageUpwards(string methodName, SendMessageOptions options) => this.SendMessageUpwards(methodName, (object) null, options);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
    /// </summary>
    /// <param name="methodName">Name of the method to call.</param>
    /// <param name="value">Optional parameter for the method.</param>
    /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
    public void SendMessage(string methodName, object value) => this.SendMessage(methodName, value, SendMessageOptions.RequireReceiver);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
    /// </summary>
    /// <param name="methodName">Name of the method to call.</param>
    /// <param name="value">Optional parameter for the method.</param>
    /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
    public void SendMessage(string methodName) => this.SendMessage(methodName, (object) null, SendMessageOptions.RequireReceiver);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
    /// </summary>
    /// <param name="methodName">Name of the method to call.</param>
    /// <param name="value">Optional parameter for the method.</param>
    /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
    [FreeFunction("SendMessage", HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SendMessage(string methodName, object value, SendMessageOptions options);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
    /// </summary>
    /// <param name="methodName">Name of the method to call.</param>
    /// <param name="value">Optional parameter for the method.</param>
    /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
    public void SendMessage(string methodName, SendMessageOptions options) => this.SendMessage(methodName, (object) null, options);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
    /// </summary>
    /// <param name="methodName">Name of the method to call.</param>
    /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
    /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
    [FreeFunction("BroadcastMessage", HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void BroadcastMessage(
      string methodName,
      [UnityEngine.Internal.DefaultValue("null")] object parameter,
      [UnityEngine.Internal.DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
    /// </summary>
    /// <param name="methodName">Name of the method to call.</param>
    /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
    /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
    [ExcludeFromDocs]
    public void BroadcastMessage(string methodName, object parameter) => this.BroadcastMessage(methodName, parameter, SendMessageOptions.RequireReceiver);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
    /// </summary>
    /// <param name="methodName">Name of the method to call.</param>
    /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
    /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
    [ExcludeFromDocs]
    public void BroadcastMessage(string methodName) => this.BroadcastMessage(methodName, (object) null, SendMessageOptions.RequireReceiver);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
    /// </summary>
    /// <param name="methodName">Name of the method to call.</param>
    /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
    /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
    public void BroadcastMessage(string methodName, SendMessageOptions options) => this.BroadcastMessage(methodName, (object) null, options);

    /// <summary>
    ///   <para>The Rigidbody attached to this GameObject. (Null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property rigidbody has been deprecated. Use GetComponent<Rigidbody>() instead. (UnityUpgradable)", true)]
    public Component rigidbody => throw new NotSupportedException("rigidbody property has been deprecated");

    /// <summary>
    ///   <para>The Rigidbody2D that is attached to the Component's GameObject.</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property rigidbody2D has been deprecated. Use GetComponent<Rigidbody2D>() instead. (UnityUpgradable)", true)]
    public Component rigidbody2D => throw new NotSupportedException("rigidbody2D property has been deprecated");

    /// <summary>
    ///   <para>The Camera attached to this GameObject. (Null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property camera has been deprecated. Use GetComponent<Camera>() instead. (UnityUpgradable)", true)]
    public Component camera => throw new NotSupportedException("camera property has been deprecated");

    /// <summary>
    ///   <para>The Light attached to this GameObject. (Null if there is none attached).</para>
    /// </summary>
    [Obsolete("Property light has been deprecated. Use GetComponent<Light>() instead. (UnityUpgradable)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Component light => throw new NotSupportedException("light property has been deprecated");

    /// <summary>
    ///   <para>The Animation attached to this GameObject. (Null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property animation has been deprecated. Use GetComponent<Animation>() instead. (UnityUpgradable)", true)]
    public Component animation => throw new NotSupportedException("animation property has been deprecated");

    /// <summary>
    ///   <para>The ConstantForce attached to this GameObject. (Null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property constantForce has been deprecated. Use GetComponent<ConstantForce>() instead. (UnityUpgradable)", true)]
    public Component constantForce => throw new NotSupportedException("constantForce property has been deprecated");

    /// <summary>
    ///   <para>The Renderer attached to this GameObject. (Null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property renderer has been deprecated. Use GetComponent<Renderer>() instead. (UnityUpgradable)", true)]
    public Component renderer => throw new NotSupportedException("renderer property has been deprecated");

    /// <summary>
    ///   <para>The AudioSource attached to this GameObject. (Null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property audio has been deprecated. Use GetComponent<AudioSource>() instead. (UnityUpgradable)", true)]
    public Component audio => throw new NotSupportedException("audio property has been deprecated");

    /// <summary>
    ///   <para>The NetworkView attached to this GameObject (Read Only). (null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property networkView has been deprecated. Use GetComponent<NetworkView>() instead. (UnityUpgradable)", true)]
    public Component networkView => throw new NotSupportedException("networkView property has been deprecated");

    /// <summary>
    ///   <para>The Collider attached to this GameObject. (Null if there is none attached).</para>
    /// </summary>
    [Obsolete("Property collider has been deprecated. Use GetComponent<Collider>() instead. (UnityUpgradable)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Component collider => throw new NotSupportedException("collider property has been deprecated");

    /// <summary>
    ///   <para>The Collider2D component attached to the object.</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property collider2D has been deprecated. Use GetComponent<Collider2D>() instead. (UnityUpgradable)", true)]
    public Component collider2D => throw new NotSupportedException("collider2D property has been deprecated");

    /// <summary>
    ///   <para>The HingeJoint attached to this GameObject. (Null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property hingeJoint has been deprecated. Use GetComponent<HingeJoint>() instead. (UnityUpgradable)", true)]
    public Component hingeJoint => throw new NotSupportedException("hingeJoint property has been deprecated");

    /// <summary>
    ///   <para>The ParticleSystem attached to this GameObject. (Null if there is none attached).</para>
    /// </summary>
    [Obsolete("Property particleSystem has been deprecated. Use GetComponent<ParticleSystem>() instead. (UnityUpgradable)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Component particleSystem => throw new NotSupportedException("particleSystem property has been deprecated");
  }
}
