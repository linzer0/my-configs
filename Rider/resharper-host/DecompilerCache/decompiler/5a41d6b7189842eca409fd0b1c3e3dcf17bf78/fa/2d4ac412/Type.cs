// Decompiled with JetBrains decompiler
// Type: System.Type
// Assembly: netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 5A41D6B7-1898-42EC-A409-FD0B1C3E3DCF
// Assembly location: E:\Programs\Gamedev\Editor\2021.3.17f1\Editor\Data\NetStandard\ref\2.1.0\netstandard.dll

using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
  public abstract class Type : MemberInfo, IReflect
  {
    public static readonly char Delimiter;
    public static readonly Type[] EmptyTypes;
    public static readonly MemberFilter FilterAttribute;
    public static readonly MemberFilter FilterName;
    public static readonly MemberFilter FilterNameIgnoreCase;
    public static readonly object Missing;

    public abstract Assembly Assembly { get; }

    public abstract string AssemblyQualifiedName { get; }

    public TypeAttributes Attributes { get; }

    public abstract Type BaseType { get; }

    public virtual bool ContainsGenericParameters { get; }

    public virtual MethodBase DeclaringMethod { get; }

    public override Type DeclaringType { get; }

    public static Binder DefaultBinder { get; }

    public abstract string FullName { get; }

    public virtual GenericParameterAttributes GenericParameterAttributes { get; }

    public virtual int GenericParameterPosition { get; }

    public virtual Type[] GenericTypeArguments { get; }

    public abstract Guid GUID { get; }

    public bool HasElementType { get; }

    public bool IsAbstract { get; }

    public bool IsAnsiClass { get; }

    public bool IsArray { get; }

    public bool IsAutoClass { get; }

    public bool IsAutoLayout { get; }

    public bool IsByRef { get; }

    public virtual bool IsByRefLike { get; }

    public bool IsClass { get; }

    public bool IsCOMObject { get; }

    public virtual bool IsConstructedGenericType { get; }

    public bool IsContextful { get; }

    public virtual bool IsEnum { get; }

    public bool IsExplicitLayout { get; }

    public virtual bool IsGenericMethodParameter { get; }

    public virtual bool IsGenericParameter { get; }

    public virtual bool IsGenericType { get; }

    public virtual bool IsGenericTypeDefinition { get; }

    public virtual bool IsGenericTypeParameter { get; }

    public bool IsImport { get; }

    public bool IsInterface { get; }

    public bool IsLayoutSequential { get; }

    public bool IsMarshalByRef { get; }

    public bool IsNested { get; }

    public bool IsNestedAssembly { get; }

    public bool IsNestedFamANDAssem { get; }

    public bool IsNestedFamily { get; }

    public bool IsNestedFamORAssem { get; }

    public bool IsNestedPrivate { get; }

    public bool IsNestedPublic { get; }

    public bool IsNotPublic { get; }

    public bool IsPointer { get; }

    public bool IsPrimitive { get; }

    public bool IsPublic { get; }

    public bool IsSealed { get; }

    public virtual bool IsSecurityCritical { get; }

    public virtual bool IsSecuritySafeCritical { get; }

    public virtual bool IsSecurityTransparent { get; }

    public virtual bool IsSerializable { get; }

    public virtual bool IsSignatureType { get; }

    public bool IsSpecialName { get; }

    public virtual bool IsSZArray { get; }

    public virtual bool IsTypeDefinition { get; }

    public bool IsUnicodeClass { get; }

    public bool IsValueType { get; }

    public virtual bool IsVariableBoundArray { get; }

    public bool IsVisible { get; }

    public override MemberTypes MemberType { get; }

    public new abstract Module Module { get; }

    public abstract string Namespace { get; }

    public override Type ReflectedType { get; }

    public virtual StructLayoutAttribute StructLayoutAttribute { get; }

    public virtual RuntimeTypeHandle TypeHandle { get; }

    public ConstructorInfo TypeInitializer { get; }

    public abstract Type UnderlyingSystemType { get; }

    public override bool Equals(object o);

    public virtual bool Equals(Type o);

    public virtual Type[] FindInterfaces(TypeFilter filter, object filterCriteria);

    public virtual MemberInfo[] FindMembers(
      MemberTypes memberType,
      BindingFlags bindingAttr,
      MemberFilter filter,
      object filterCriteria);

    public virtual int GetArrayRank();

    protected abstract TypeAttributes GetAttributeFlagsImpl();

    public ConstructorInfo GetConstructor(
      BindingFlags bindingAttr,
      Binder binder,
      CallingConventions callConvention,
      Type[] types,
      ParameterModifier[] modifiers);

    public ConstructorInfo GetConstructor(
      BindingFlags bindingAttr,
      Binder binder,
      Type[] types,
      ParameterModifier[] modifiers);

    public ConstructorInfo GetConstructor(Type[] types);

    protected abstract ConstructorInfo GetConstructorImpl(
      BindingFlags bindingAttr,
      Binder binder,
      CallingConventions callConvention,
      Type[] types,
      ParameterModifier[] modifiers);

    public ConstructorInfo[] GetConstructors();

    public abstract ConstructorInfo[] GetConstructors(BindingFlags bindingAttr);

    public virtual MemberInfo[] GetDefaultMembers();

    public abstract Type GetElementType();

    public virtual string GetEnumName(object value);

    public virtual string[] GetEnumNames();

    public virtual Type GetEnumUnderlyingType();

    public virtual Array GetEnumValues();

    public EventInfo GetEvent(string name);

    public abstract EventInfo GetEvent(string name, BindingFlags bindingAttr);

    public virtual EventInfo[] GetEvents();

    public abstract EventInfo[] GetEvents(BindingFlags bindingAttr);

    public FieldInfo GetField(string name);

    public abstract FieldInfo GetField(string name, BindingFlags bindingAttr);

    public FieldInfo[] GetFields();

    public abstract FieldInfo[] GetFields(BindingFlags bindingAttr);

    public virtual Type[] GetGenericArguments();

    public virtual Type[] GetGenericParameterConstraints();

    public virtual Type GetGenericTypeDefinition();

    public override int GetHashCode();

    public Type GetInterface(string name);

    public abstract Type GetInterface(string name, bool ignoreCase);

    public virtual InterfaceMapping GetInterfaceMap(Type interfaceType);

    public abstract Type[] GetInterfaces();

    public MemberInfo[] GetMember(string name);

    public virtual MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

    public virtual MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr);

    public MemberInfo[] GetMembers();

    public abstract MemberInfo[] GetMembers(BindingFlags bindingAttr);

    public MethodInfo GetMethod(string name);

    public MethodInfo GetMethod(
      string name,
      int genericParameterCount,
      BindingFlags bindingAttr,
      Binder binder,
      CallingConventions callConvention,
      Type[] types,
      ParameterModifier[] modifiers);

    public MethodInfo GetMethod(
      string name,
      int genericParameterCount,
      BindingFlags bindingAttr,
      Binder binder,
      Type[] types,
      ParameterModifier[] modifiers);

    public MethodInfo GetMethod(string name, int genericParameterCount, Type[] types);

    public MethodInfo GetMethod(
      string name,
      int genericParameterCount,
      Type[] types,
      ParameterModifier[] modifiers);

    public MethodInfo GetMethod(string name, BindingFlags bindingAttr);

    public MethodInfo GetMethod(
      string name,
      BindingFlags bindingAttr,
      Binder binder,
      CallingConventions callConvention,
      Type[] types,
      ParameterModifier[] modifiers);

    public MethodInfo GetMethod(
      string name,
      BindingFlags bindingAttr,
      Binder binder,
      Type[] types,
      ParameterModifier[] modifiers);

    public MethodInfo GetMethod(string name, Type[] types);

    public MethodInfo GetMethod(string name, Type[] types, ParameterModifier[] modifiers);

    protected virtual MethodInfo GetMethodImpl(
      string name,
      int genericParameterCount,
      BindingFlags bindingAttr,
      Binder binder,
      CallingConventions callConvention,
      Type[] types,
      ParameterModifier[] modifiers);

    protected abstract MethodInfo GetMethodImpl(
      string name,
      BindingFlags bindingAttr,
      Binder binder,
      CallingConventions callConvention,
      Type[] types,
      ParameterModifier[] modifiers);

    public MethodInfo[] GetMethods();

    public abstract MethodInfo[] GetMethods(BindingFlags bindingAttr);

    public Type GetNestedType(string name);

    public abstract Type GetNestedType(string name, BindingFlags bindingAttr);

    public Type[] GetNestedTypes();

    public abstract Type[] GetNestedTypes(BindingFlags bindingAttr);

    public PropertyInfo[] GetProperties();

    public abstract PropertyInfo[] GetProperties(BindingFlags bindingAttr);

    public PropertyInfo GetProperty(string name);

    public PropertyInfo GetProperty(string name, BindingFlags bindingAttr);

    public PropertyInfo GetProperty(
      string name,
      BindingFlags bindingAttr,
      Binder binder,
      Type returnType,
      Type[] types,
      ParameterModifier[] modifiers);

    public PropertyInfo GetProperty(string name, Type returnType);

    public PropertyInfo GetProperty(string name, Type returnType, Type[] types);

    public PropertyInfo GetProperty(
      string name,
      Type returnType,
      Type[] types,
      ParameterModifier[] modifiers);

    public PropertyInfo GetProperty(string name, Type[] types);

    protected abstract PropertyInfo GetPropertyImpl(
      string name,
      BindingFlags bindingAttr,
      Binder binder,
      Type returnType,
      Type[] types,
      ParameterModifier[] modifiers);

    public new Type GetType();

    public static Type GetType(string typeName);

    public static Type GetType(string typeName, bool throwOnError);

    public static Type GetType(string typeName, bool throwOnError, bool ignoreCase);

    public static Type GetType(
      string typeName,
      Func<AssemblyName, Assembly> assemblyResolver,
      Func<Assembly, string, bool, Type> typeResolver);

    public static Type GetType(
      string typeName,
      Func<AssemblyName, Assembly> assemblyResolver,
      Func<Assembly, string, bool, Type> typeResolver,
      bool throwOnError);

    public static Type GetType(
      string typeName,
      Func<AssemblyName, Assembly> assemblyResolver,
      Func<Assembly, string, bool, Type> typeResolver,
      bool throwOnError,
      bool ignoreCase);

    public static Type[] GetTypeArray(object[] args);

    public static TypeCode GetTypeCode(Type type);

    protected virtual TypeCode GetTypeCodeImpl();

    public static Type GetTypeFromCLSID(Guid clsid);

    public static Type GetTypeFromCLSID(Guid clsid, bool throwOnError);

    public static Type GetTypeFromCLSID(Guid clsid, string server);

    public static Type GetTypeFromCLSID(Guid clsid, string server, bool throwOnError);

    public static Type GetTypeFromHandle(RuntimeTypeHandle handle);

    public static Type GetTypeFromProgID(string progID);

    public static Type GetTypeFromProgID(string progID, bool throwOnError);

    public static Type GetTypeFromProgID(string progID, string server);

    public static Type GetTypeFromProgID(string progID, string server, bool throwOnError);

    public static RuntimeTypeHandle GetTypeHandle(object o);

    protected abstract bool HasElementTypeImpl();

    public object InvokeMember(
      string name,
      BindingFlags invokeAttr,
      Binder binder,
      object target,
      object[] args);

    public object InvokeMember(
      string name,
      BindingFlags invokeAttr,
      Binder binder,
      object target,
      object[] args,
      CultureInfo culture);

    public abstract object InvokeMember(
      string name,
      BindingFlags invokeAttr,
      Binder binder,
      object target,
      object[] args,
      ParameterModifier[] modifiers,
      CultureInfo culture,
      string[] namedParameters);

    protected abstract bool IsArrayImpl();

    public virtual bool IsAssignableFrom(Type c);

    protected abstract bool IsByRefImpl();

    protected abstract bool IsCOMObjectImpl();

    protected virtual bool IsContextfulImpl();

    public virtual bool IsEnumDefined(object value);

    public virtual bool IsEquivalentTo(Type other);

    public virtual bool IsInstanceOfType(object o);

    protected virtual bool IsMarshalByRefImpl();

    protected abstract bool IsPointerImpl();

    protected abstract bool IsPrimitiveImpl();

    public virtual bool IsSubclassOf(Type c);

    protected virtual bool IsValueTypeImpl();

    public virtual Type MakeArrayType();

    public virtual Type MakeArrayType(int rank);

    public virtual Type MakeByRefType();

    public static Type MakeGenericMethodParameter(int position);

    public virtual Type MakeGenericType(params Type[] typeArguments);

    public virtual Type MakePointerType();

    public static bool operator ==(Type left, Type right);

    public static bool operator !=(Type left, Type right);

    public static Type ReflectionOnlyGetType(
      string typeName,
      bool throwIfNotFound,
      bool ignoreCase);

    public override string ToString();
  }
}
