# Lazy Property Helper Nuget  

# Usage  

Follow the steps from the [Nuget Package Url](https://www.nuget.org/packages/LazyPropertyHelper/)  

# Development  

## Run the test  

```bash
dotnet test
```

## Nuget Publish  

[Docs](https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-visual-studio)  

### Step 1: Package project  

```bash
PROJECT_FILE=LazyPropertyHelper/LazyPropertyHelper.csproj && \
  rm LazyPropertyHelper/bin/Release/*.nupkg && \
  dotnet clean $PROJECT_FILE -c Release && \
  dotnet build $PROJECT_FILE -c Release && \
  dotnet pack $PROJECT_FILE -c Release
```

### Step 2: Publish nuget  

```bash
dotnet nuget push LazyPropertyHelper/bin/Release/LazyPropertyHelper.*.nupkg \
  -k `sudo security find-generic-password -w -gs NUGET_PUSH_KEY` \
  -s https://api.nuget.org/v3/index.json
```

*The above command assumes there's a `NUGET_PUSH_KEY` stored in the Keychain*  
