# Add API Key Authentication before accessing Controller

1. Add Authentication to DI

```
services.AddAuthentication(options =>
{
    options.AddScheme<ApiKeyAuthenticationHandler>("ApiKey", "API Key");
});
```

2. Add Attribute to controller

`[Authorize(AuthenticationSchemes = "ApiKey")]`

3. Add API Key to appsettings.json file

`"ApiKey": "abc"`

			