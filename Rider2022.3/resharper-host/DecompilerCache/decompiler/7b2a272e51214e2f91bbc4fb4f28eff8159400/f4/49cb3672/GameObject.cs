// Decompiled with JetBrains decompiler
// Type: UnityEngine.GameObject
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
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Base class for all entities in Unity Scenes.</para>
  /// </summary>
  [NativeHeader("Runtime/Export/Scripting/GameObject.bindings.h")]
  [ExcludeFromPreset]
  [UsedByNativeCode]
  public sealed class GameObject : Object
  {
    /// <summary>
    ///   <para>Creates a game object with a primitive mesh renderer and appropriate collider.</para>
    /// </summary>
    /// <param name="type">The type of primitive object to create.</param>
    [FreeFunction("GameObjectBindings::CreatePrimitive")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject CreatePrimitive(PrimitiveType type);

    [SecuritySafeCritical]
    public unsafe T GetComponent<T>()
    {
      CastHelper<T> castHelper = new CastHelper<T>();
      this.GetComponentFastPath(typeof (T), new IntPtr((void*) &castHelper.onePointerFurtherThanT));
      return castHelper.t;
    }

    /// <summary>
    ///   <para>Returns the component of Type type if the game object has one attached, null if it doesn't.</para>
    /// </summary>
    /// <param name="type">The type of Component to retrieve.</param>
    [FreeFunction(Name = "GameObjectBindings::GetComponentFromType", HasExplicitThis = true, ThrowsException = true)]
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Component GetComponent(System.Type type);

    [FreeFunction(Name = "GameObjectBindings::GetComponentFastPath", HasExplicitThis = true, ThrowsException = true)]
    [NativeWritableSelf]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void GetComponentFastPath(System.Type type, IntPtr oneFurtherThanResultValue);

    [FreeFunction(Name = "Scripting::GetScriptingWrapperOfComponentOfGameObject", HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern Component GetComponentByName(string type);

    /// <summary>
    ///   <para>Returns the component with name type if the GameObject has one attached, null if it doesn't.</para>
    /// </summary>
    /// <param name="type">The type of Component to retrieve.</param>
    public Component GetComponent(string type) => this.GetComponentByName(type);

    /// <summary>
    ///   <para>Returns the component of Type type in the GameObject or any of its children using depth first search.</para>
    /// </summary>
    /// <param name="type">The type of Component to retrieve.</param>
    /// <param name="includeInactive"></param>
    /// <returns>
    ///   <para>A component of the matching type, if found.</para>
    /// </returns>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    [FreeFunction(Name = "GameObjectBindings::GetComponentInChildren", HasExplicitThis = true, ThrowsException = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Component GetComponentInChildren(System.Type type, bool includeInactive);

    /// <summary>
    ///   <para>Returns the component of Type type in the GameObject or any of its children using depth first search.</para>
    /// </summary>
    /// <param name="type">The type of Component to retrieve.</param>
    /// <param name="includeInactive"></param>
    /// <returns>
    ///   <para>A component of the matching type, if found.</para>
    /// </returns>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public Component GetComponentInChildren(System.Type type) => this.GetComponentInChildren(type, false);

    [ExcludeFromDocs]
    public T GetComponentInChildren<T>() => this.GetComponentInChildren<T>(false);

    public T GetComponentInChildren<T>([UnityEngine.Internal.DefaultValue("false")] bool includeInactive) => (T) this.GetComponentInChildren(typeof (T), includeInactive);

    /// <summary>
    ///   <para>Retrieves the component of Type type in the GameObject or any of its parents.</para>
    /// </summary>
    /// <param name="type">Type of component to find.</param>
    /// <param name="includeInactive"></param>
    /// <returns>
    ///   <para>Returns a component if a component matching the type is found. Returns null otherwise.</para>
    /// </returns>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    [FreeFunction(Name = "GameObjectBindings::GetComponentInParent", HasExplicitThis = true, ThrowsException = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Component GetComponentInParent(System.Type type, bool includeInactive);

    /// <summary>
    ///   <para>Retrieves the component of Type type in the GameObject or any of its parents.</para>
    /// </summary>
    /// <param name="type">Type of component to find.</param>
    /// <param name="includeInactive"></param>
    /// <returns>
    ///   <para>Returns a component if a component matching the type is found. Returns null otherwise.</para>
    /// </returns>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public Component GetComponentInParent(System.Type type) => this.GetComponentInParent(type, false);

    [ExcludeFromDocs]
    public T GetComponentInParent<T>() => this.GetComponentInParent<T>(false);

    public T GetComponentInParent<T>([UnityEngine.Internal.DefaultValue("false")] bool includeInactive) => (T) this.GetComponentInParent(typeof (T), includeInactive);

    [FreeFunction(Name = "GameObjectBindings::GetComponentsInternal", HasExplicitThis = true, ThrowsException = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Array GetComponentsInternal(
      System.Type type,
      bool useSearchTypeAsArrayReturnType,
      bool recursive,
      bool includeInactive,
      bool reverse,
      object resultList);

    /// <summary>
    ///   <para>Returns all components of Type type in the GameObject.</para>
    /// </summary>
    /// <param name="type">The type of component to retrieve.</param>
    public Component[] GetComponents(System.Type type) => (Component[]) this.GetComponentsInternal(type, false, false, true, false, (object) null);

    public T[] GetComponents<T>() => (T[]) this.GetComponentsInternal(typeof (T), true, false, true, false, (object) null);

    public void GetComponents(System.Type type, List<Component> results) => this.GetComponentsInternal(type, false, false, true, false, (object) results);

    public void GetComponents<T>(List<T> results) => this.GetComponentsInternal(typeof (T), true, false, true, false, (object) results);

    /// <summary>
    ///   <para>Returns all components of Type type in the GameObject or any of its children children using depth first search. Works recursively.</para>
    /// </summary>
    /// <param name="type">The type of Component to retrieve.</param>
    /// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
    [ExcludeFromDocs]
    public Component[] GetComponentsInChildren(System.Type type)
    {
      bool includeInactive = false;
      return this.GetComponentsInChildren(type, includeInactive);
    }

    /// <summary>
    ///   <para>Returns all components of Type type in the GameObject or any of its children children using depth first search. Works recursively.</para>
    /// </summary>
    /// <param name="type">The type of Component to retrieve.</param>
    /// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
    public Component[] GetComponentsInChildren(System.Type type, [UnityEngine.Internal.DefaultValue("false")] bool includeInactive) => (Component[]) this.GetComponentsInternal(type, false, true, includeInactive, false, (object) null);

    public T[] GetComponentsInChildren<T>(bool includeInactive) => (T[]) this.GetComponentsInternal(typeof (T), true, true, includeInactive, false, (object) null);

    public void GetComponentsInChildren<T>(bool includeInactive, List<T> results) => this.GetComponentsInternal(typeof (T), true, true, includeInactive, false, (object) results);

    public T[] GetComponentsInChildren<T>() => this.GetComponentsInChildren<T>(false);

    public void GetComponentsInChildren<T>(List<T> results) => this.GetComponentsInChildren<T>(false, results);

    [ExcludeFromDocs]
    public Component[] GetComponentsInParent(System.Type type)
    {
      bool includeInactive = false;
      return this.GetComponentsInParent(type, includeInactive);
    }

    /// <summary>
    ///   <para>Returns all components of Type type in the GameObject or any of its parents.</para>
    /// </summary>
    /// <param name="type">The type of Component to retrieve.</param>
    /// <param name="includeInactive">Should inactive Components be included in the found set?</param>
    public Component[] GetComponentsInParent(System.Type type, [UnityEngine.Internal.DefaultValue("false")] bool includeInactive) => (Component[]) this.GetComponentsInternal(type, false, true, includeInactive, true, (object) null);

    public void GetComponentsInParent<T>(bool includeInactive, List<T> results) => this.GetComponentsInternal(typeof (T), true, true, includeInactive, true, (object) results);

    public T[] GetComponentsInParent<T>(bool includeInactive) => (T[]) this.GetComponentsInternal(typeof (T), true, true, includeInactive, true, (object) null);

    public T[] GetComponentsInParent<T>() => this.GetComponentsInParent<T>(false);

    [SecuritySafeCritical]
    public unsafe bool TryGetComponent<T>(out T component)
    {
      CastHelper<T> castHelper = new CastHelper<T>();
      this.TryGetComponentFastPath(typeof (T), new IntPtr((void*) &castHelper.onePointerFurtherThanT));
      component = castHelper.t;
      return (object) castHelper.t != null;
    }

    public bool TryGetComponent(System.Type type, out Component component)
    {
      component = this.TryGetComponentInternal(type);
      return (Object) component != (Object) null;
    }

    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    [FreeFunction(Name = "GameObjectBindings::TryGetComponentFromType", HasExplicitThis = true, ThrowsException = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern Component TryGetComponentInternal(System.Type type);

    [NativeWritableSelf]
    [FreeFunction(Name = "GameObjectBindings::TryGetComponentFastPath", HasExplicitThis = true, ThrowsException = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void TryGetComponentFastPath(System.Type type, IntPtr oneFurtherThanResultValue);

    /// <summary>
    ///   <para>Returns one active GameObject tagged tag. Returns null if no GameObject was found.</para>
    /// </summary>
    /// <param name="tag">The tag to search for.</param>
    public static GameObject FindWithTag(string tag) => GameObject.FindGameObjectWithTag(tag);

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="options"></param>
    public void SendMessageUpwards(string methodName, SendMessageOptions options) => this.SendMessageUpwards(methodName, (object) null, options);

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="options"></param>
    public void SendMessage(string methodName, SendMessageOptions options) => this.SendMessage(methodName, (object) null, options);

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="options"></param>
    public void BroadcastMessage(string methodName, SendMessageOptions options) => this.BroadcastMessage(methodName, (object) null, options);

    [FreeFunction(Name = "MonoAddComponent", HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern Component AddComponentInternal(string className);

    [FreeFunction(Name = "MonoAddComponentWithType", HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Component Internal_AddComponentWithType(System.Type componentType);

    /// <summary>
    ///   <para>Adds a component class of type componentType to the game object. C# Users can use a generic version.</para>
    /// </summary>
    /// <param name="componentType"></param>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public Component AddComponent(System.Type componentType) => this.Internal_AddComponentWithType(componentType);

    public T AddComponent<T>() where T : Component => this.AddComponent(typeof (T)) as T;

    /// <summary>
    ///   <para>The Transform attached to this GameObject.</para>
    /// </summary>
    public extern Transform transform { [FreeFunction("GameObjectBindings::GetTransform", HasExplicitThis = true), MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The layer the GameObject is in.</para>
    /// </summary>
    public extern int layer { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("GameObject.active is obsolete. Use GameObject.SetActive(), GameObject.activeSelf or GameObject.activeInHierarchy.")]
    public extern bool active { [NativeMethod(Name = "IsActive"), MethodImpl(MethodImplOptions.InternalCall)] get; [NativeMethod(Name = "SetSelfActive"), MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>ActivatesDeactivates the GameObject, depending on the given true or false/ value.</para>
    /// </summary>
    /// <param name="value">Activate or deactivate the object, where true activates the GameObject and false deactivates the GameObject.</param>
    [NativeMethod(Name = "SetSelfActive")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetActive(bool value);

    /// <summary>
    ///   <para>The local active state of this GameObject. (Read Only)</para>
    /// </summary>
    public extern bool activeSelf { [NativeMethod(Name = "IsSelfActive"), MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Defines whether the GameObject is active in the Scene.</para>
    /// </summary>
    public extern bool activeInHierarchy { [NativeMethod(Name = "IsActive"), MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("gameObject.SetActiveRecursively() is obsolete. Use GameObject.SetActive(), which is now inherited by children.")]
    [NativeMethod(Name = "SetActiveRecursivelyDeprecated")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetActiveRecursively(bool state);

    /// <summary>
    ///   <para>Gets and sets the GameObject's StaticEditorFlags.</para>
    /// </summary>
    public extern bool isStatic { [NativeMethod(Name = "GetIsStaticDeprecated"), MethodImpl(MethodImplOptions.InternalCall)] get; [NativeMethod(Name = "SetIsStaticDeprecated"), MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal extern bool isStaticBatchable { [NativeMethod(Name = "IsStaticBatchable"), MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The tag of this game object.</para>
    /// </summary>
    public extern string tag { [FreeFunction("GameObjectBindings::GetTag", HasExplicitThis = true), MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction("GameObjectBindings::SetTag", HasExplicitThis = true), MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is this game object tagged with tag ?</para>
    /// </summary>
    /// <param name="tag">The tag to compare.</param>
    [FreeFunction(Name = "GameObjectBindings::CompareTag", HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool CompareTag(string tag);

    [FreeFunction(Name = "GameObjectBindings::FindGameObjectWithTag", ThrowsException = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject FindGameObjectWithTag(string tag);

    /// <summary>
    ///   <para>Returns an array of active GameObjects tagged tag. Returns empty array if no GameObject was found.</para>
    /// </summary>
    /// <param name="tag">The name of the tag to search GameObjects for.</param>
    [FreeFunction(Name = "GameObjectBindings::FindGameObjectsWithTag", ThrowsException = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject[] FindGameObjectsWithTag(string tag);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
    /// </summary>
    /// <param name="methodName">The name of the method to call.</param>
    /// <param name="value">An optional parameter value to pass to the called method.</param>
    /// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
    [FreeFunction(Name = "Scripting::SendScriptingMessageUpwards", HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SendMessageUpwards(
      string methodName,
      [UnityEngine.Internal.DefaultValue("null")] object value,
      [UnityEngine.Internal.DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
    /// </summary>
    /// <param name="methodName">The name of the method to call.</param>
    /// <param name="value">An optional parameter value to pass to the called method.</param>
    /// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
    [ExcludeFromDocs]
    public void SendMessageUpwards(string methodName, object value)
    {
      SendMessageOptions options = SendMessageOptions.RequireReceiver;
      this.SendMessageUpwards(methodName, value, options);
    }

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
    /// </summary>
    /// <param name="methodName">The name of the method to call.</param>
    /// <param name="value">An optional parameter value to pass to the called method.</param>
    /// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
    [ExcludeFromDocs]
    public void SendMessageUpwards(string methodName)
    {
      SendMessageOptions options = SendMessageOptions.RequireReceiver;
      object obj = (object) null;
      this.SendMessageUpwards(methodName, obj, options);
    }

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
    /// </summary>
    /// <param name="methodName">The name of the method to call.</param>
    /// <param name="value">An optional parameter value to pass to the called method.</param>
    /// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
    [FreeFunction(Name = "Scripting::SendScriptingMessage", HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SendMessage(string methodName, [UnityEngine.Internal.DefaultValue("null")] object value, [UnityEngine.Internal.DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
    /// </summary>
    /// <param name="methodName">The name of the method to call.</param>
    /// <param name="value">An optional parameter value to pass to the called method.</param>
    /// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
    [ExcludeFromDocs]
    public void SendMessage(string methodName, object value)
    {
      SendMessageOptions options = SendMessageOptions.RequireReceiver;
      this.SendMessage(methodName, value, options);
    }

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
    /// </summary>
    /// <param name="methodName">The name of the method to call.</param>
    /// <param name="value">An optional parameter value to pass to the called method.</param>
    /// <param name="options">Should an error be raised if the method doesn't exist on the target object?</param>
    [ExcludeFromDocs]
    public void SendMessage(string methodName)
    {
      SendMessageOptions options = SendMessageOptions.RequireReceiver;
      object obj = (object) null;
      this.SendMessage(methodName, obj, options);
    }

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="parameter"></param>
    /// <param name="options"></param>
    [FreeFunction(Name = "Scripting::BroadcastScriptingMessage", HasExplicitThis = true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void BroadcastMessage(
      string methodName,
      [UnityEngine.Internal.DefaultValue("null")] object parameter,
      [UnityEngine.Internal.DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="parameter"></param>
    /// <param name="options"></param>
    [ExcludeFromDocs]
    public void BroadcastMessage(string methodName, object parameter)
    {
      SendMessageOptions options = SendMessageOptions.RequireReceiver;
      this.BroadcastMessage(methodName, parameter, options);
    }

    /// <summary>
    ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="parameter"></param>
    /// <param name="options"></param>
    [ExcludeFromDocs]
    public void BroadcastMessage(string methodName)
    {
      SendMessageOptions options = SendMessageOptions.RequireReceiver;
      object parameter = (object) null;
      this.BroadcastMessage(methodName, parameter, options);
    }

    /// <summary>
    ///   <para>Creates a new game object, named name.</para>
    /// </summary>
    /// <param name="name">The name that the GameObject is created with.</param>
    /// <param name="components">A list of Components to add to the GameObject on creation.</param>
    public GameObject(string name) => GameObject.Internal_CreateGameObject(this, name);

    /// <summary>
    ///   <para>Creates a new game object, named name.</para>
    /// </summary>
    /// <param name="name">The name that the GameObject is created with.</param>
    /// <param name="components">A list of Components to add to the GameObject on creation.</param>
    public GameObject() => GameObject.Internal_CreateGameObject(this, (string) null);

    /// <summary>
    ///   <para>Creates a new game object, named name.</para>
    /// </summary>
    /// <param name="name">The name that the GameObject is created with.</param>
    /// <param name="components">A list of Components to add to the GameObject on creation.</param>
    public GameObject(string name, params System.Type[] components)
    {
      GameObject.Internal_CreateGameObject(this, name);
      foreach (System.Type component in components)
        this.AddComponent(component);
    }

    [FreeFunction(Name = "GameObjectBindings::Internal_CreateGameObject")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateGameObject([Writable] GameObject self, string name);

    /// <summary>
    ///   <para>Finds a GameObject by name and returns it.</para>
    /// </summary>
    /// <param name="name"></param>
    [FreeFunction(Name = "GameObjectBindings::Find")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject Find(string name);

    /// <summary>
    ///   <para>Scene that the GameObject is part of.</para>
    /// </summary>
    public Scene scene
    {
      [FreeFunction("GameObjectBindings::GetScene", HasExplicitThis = true)] get
      {
        Scene ret;
        this.get_scene_Injected(out ret);
        return ret;
      }
    }

    /// <summary>
    ///   <para>Scene culling mask Unity uses to determine which scene to render the GameObject in.</para>
    /// </summary>
    public extern ulong sceneCullingMask { [FreeFunction(Name = "GameObjectBindings::GetSceneCullingMask", HasExplicitThis = true), MethodImpl(MethodImplOptions.InternalCall)] get; }

    [FreeFunction(Name = "GameObjectBindings::CalculateBounds", HasExplicitThis = true)]
    internal Bounds CalculateBounds()
    {
      Bounds ret;
      this.CalculateBounds_Injected(out ret);
      return ret;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern int IsMarkedVisible();

    public GameObject gameObject => this;

    [Obsolete("GameObject.SampleAnimation(AnimationClip, float) has been deprecated. Use AnimationClip.SampleAnimation(GameObject, float) instead (UnityUpgradable).", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SampleAnimation(Object clip, float time) => throw new NotSupportedException("GameObject.SampleAnimation is deprecated");

    /// <summary>
    ///   <para>Adds a component class named className to the game object.</para>
    /// </summary>
    /// <param name="className"></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("GameObject.AddComponent with string argument has been deprecated. Use GameObject.AddComponent<T>() instead. (UnityUpgradable).", true)]
    public Component AddComponent(string className) => throw new NotSupportedException("AddComponent(string) is deprecated");

    /// <summary>
    ///   <para>The Rigidbody attached to this GameObject (Read Only). (Null if there is none attached).</para>
    /// </summary>
    [Obsolete("Property rigidbody has been deprecated. Use GetComponent<Rigidbody>() instead. (UnityUpgradable)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Component rigidbody => throw new NotSupportedException("rigidbody property has been deprecated");

    /// <summary>
    ///   <para>The Rigidbody2D component attached to this GameObject. (Read Only)</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property rigidbody2D has been deprecated. Use GetComponent<Rigidbody2D>() instead. (UnityUpgradable)", true)]
    public Component rigidbody2D => throw new NotSupportedException("rigidbody2D property has been deprecated");

    /// <summary>
    ///   <para>The Camera attached to this GameObject (Read Only). (Null if there is none attached).</para>
    /// </summary>
    [Obsolete("Property camera has been deprecated. Use GetComponent<Camera>() instead. (UnityUpgradable)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Component camera => throw new NotSupportedException("camera property has been deprecated");

    /// <summary>
    ///   <para>The Light attached to this GameObject (Read Only). (Null if there is none attached).</para>
    /// </summary>
    [Obsolete("Property light has been deprecated. Use GetComponent<Light>() instead. (UnityUpgradable)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Component light => throw new NotSupportedException("light property has been deprecated");

    /// <summary>
    ///   <para>The Animation attached to this GameObject (Read Only). (Null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property animation has been deprecated. Use GetComponent<Animation>() instead. (UnityUpgradable)", true)]
    public Component animation => throw new NotSupportedException("animation property has been deprecated");

    /// <summary>
    ///   <para>The ConstantForce attached to this GameObject (Read Only). (Null if there is none attached).</para>
    /// </summary>
    [Obsolete("Property constantForce has been deprecated. Use GetComponent<ConstantForce>() instead. (UnityUpgradable)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Component constantForce => throw new NotSupportedException("constantForce property has been deprecated");

    /// <summary>
    ///   <para>The Renderer attached to this GameObject (Read Only). (Null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property renderer has been deprecated. Use GetComponent<Renderer>() instead. (UnityUpgradable)", true)]
    public Component renderer => throw new NotSupportedException("renderer property has been deprecated");

    /// <summary>
    ///   <para>The AudioSource attached to this GameObject (Read Only). (Null if there is none attached).</para>
    /// </summary>
    [Obsolete("Property audio has been deprecated. Use GetComponent<AudioSource>() instead. (UnityUpgradable)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Component audio => throw new NotSupportedException("audio property has been deprecated");

    /// <summary>
    ///   <para>The NetworkView attached to this GameObject (Read Only). (Null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property networkView has been deprecated. Use GetComponent<NetworkView>() instead. (UnityUpgradable)", true)]
    public Component networkView => throw new NotSupportedException("networkView property has been deprecated");

    /// <summary>
    ///   <para>The Collider attached to this GameObject (Read Only). (Null if there is none attached).</para>
    /// </summary>
    [Obsolete("Property collider has been deprecated. Use GetComponent<Collider>() instead. (UnityUpgradable)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Component collider => throw new NotSupportedException("collider property has been deprecated");

    /// <summary>
    ///   <para>The Collider2D component attached to this object.</para>
    /// </summary>
    [Obsolete("Property collider2D has been deprecated. Use GetComponent<Collider2D>() instead. (UnityUpgradable)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Component collider2D => throw new NotSupportedException("collider2D property has been deprecated");

    /// <summary>
    ///   <para>The HingeJoint attached to this GameObject (Read Only). (Null if there is none attached).</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property hingeJoint has been deprecated. Use GetComponent<HingeJoint>() instead. (UnityUpgradable)", true)]
    public Component hingeJoint => throw new NotSupportedException("hingeJoint property has been deprecated");

    /// <summary>
    ///   <para>The ParticleSystem attached to this GameObject (Read Only). (Null if there is none attached).</para>
    /// </summary>
    [Obsolete("Property particleSystem has been deprecated. Use GetComponent<ParticleSystem>() instead. (UnityUpgradable)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Component particleSystem => throw new NotSupportedException("particleSystem property has been deprecated");

    [Obsolete("gameObject.PlayAnimation is not supported anymore. Use animation.Play()", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void PlayAnimation(Object animation) => throw new NotSupportedException("gameObject.PlayAnimation is not supported anymore. Use animation.Play();");

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("gameObject.StopAnimation is not supported anymore. Use animation.Stop()", true)]
    public void StopAnimation() => throw new NotSupportedException("gameObject.StopAnimation(); is not supported anymore. Use animation.Stop();");

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_scene_Injected(out Scene ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void CalculateBounds_Injected(out Bounds ret);
  }
}
