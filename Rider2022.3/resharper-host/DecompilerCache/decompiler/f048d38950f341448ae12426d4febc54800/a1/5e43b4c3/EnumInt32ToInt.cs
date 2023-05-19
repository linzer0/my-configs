// Decompiled with JetBrains decompiler
// Type: EnumInt32ToInt
// Assembly: enum2int, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F048D389-50F3-4144-8AE1-2426D4FEBC54
// Assembly location: G:\Work\Projects\td-pvp\CastleFight\Assets\Orbox\EnumInt32ToInt\Plugins\enum2int.dll

public class EnumInt32ToInt
{
  public static int Convert<TEnum>(TEnum value) where TEnum : struct => (int) value;
}
