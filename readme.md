# Localizy
Strongly typed localization. Based on the foundations of Fubu-Localizations but with an easy setup and good feature set.

## Getting Started

Defining a translation is done by creating a public static field. The generic paramter type should be the parent class type. 

```csharp
public class L 
{
    public static StringToken Default = new StringToken<L>("Default Value"); 	
}
```

Then in your Application_Start or static Main method, you need to initialize the Localization provider. This should only be called once. LocalizationManager is a static class helper. This uses the generic parameter to determine the assembly in which your localizations are defined. You can pass the assembly in when not using the generic version of the `Init` method.

```csharp
var provider = LocalizationManager.Init<L>();
```

You can then use the provider to resolve the translations. It can be resolved in 1 of 3 ways.

1. By Key  
This returns null if the key could not be found. The key is defaulted to the class hierachy.
2. By StringToken
3. Using the StringToken directly

```csharp
// 1.
provider.TryGetText("L.Default");
// 2.
provider.GetText(L.Default);
// 3.
L.Default
```

## Localization Sources
You can define multiple localization sources for the transated values. These need to implement `ILocalizationStorageProvider`.

```csharp
public interface ILocalizationStorageProvider
{
    string Name { get; }
    IEnumerable<LocalString> Provide(CultureInfo culture);
}
```

These can then be passed to the `Init` method. Multiple can be defined and they will get overlayed in the order they are passed in.  
For instance you may have google localizations and custom localizations. 

```csharp
var provider = LocalizationManager
    .Init<L>(new XmlDirectoryStorageProvider("Google", @"c:\mylocalizations"), 
             new CustomLocalizationProvider());
```

The above example is using the default `XmlDirectoryStorageProvider` which loads translations formatted as xml. For instance the google translations could be stored in XML files like so. The files must start with the culture and end in .locale.config. eg. de.local.config. This can be overridden.

```xml
<localizations>
  <string key="L.Default">The Google Default</string>
</localizations>
```

## Current Culture
The current culture used to resolve the translations is determined (by default) to be the Current Thread Culture but this can be overriden in a couple different ways.

1 - CurrentCultureFactory

```csharp
provider.CurrentCultureFactory = () => {
   //return my fancy way to resolve the current culture  
};
```

2 - Passing it in

```csharp
provider.TryGetText("L.Default", new Culture("de"));
// or
provider.GetText(L.Default, new Culture("de"));
// or 
L.Default.ToString(new Culture("de"));
```
