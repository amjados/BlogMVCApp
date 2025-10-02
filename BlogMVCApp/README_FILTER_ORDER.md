# 🎛️ Filter Order & Global Application Configuration Guide

## 🎯 **Multiple Approaches to Control Filter Order and Application**

Your BlogMVCApp now supports **4 different approaches** to control filter order and global application:

## 📋 **Approach 1: Global Filters in Program.cs (Immediate)**

### **✅ Apply Filters Globally to ALL Actions**

```csharp
// In Program.cs
builder.Services.AddControllersWithViews(options =>
{
    // Global filters with explicit order
    options.Filters.Add<OrderedLogActionAttribute>(order: 100);
    options.Filters.Add<OrderedRateLimitAttribute>(order: 200);
    options.Filters.Add<ValidateModelAttribute>(order: 300);
    options.Filters.Add<OrderedCacheResponseAttribute>(order: 500);
});
```

**Benefits:**
- ✅ **Immediate application** to all actions
- ✅ **Centralized control** in one place
- ✅ **Explicit ordering** with order parameter
- ✅ **No configuration files needed**

## 📋 **Approach 2: Configuration-Based Global Filters**

### **✅ Control via appsettings.json**

```json
{
  "FilterOrder": {
    "GlobalFilters": [
      {
        "FilterType": "LogAction",
        "Order": 100,
        "Enabled": true,
        "ExcludeActions": ["HealthCheck"]
      },
      {
        "FilterType": "ConfigurableRateLimit",
        "Order": 200,
        "Enabled": true,
        "Parameters": { "actionKey": "default" }
      }
    ]
  }
}
```

**Benefits:**
- ✅ **Runtime configuration** changes
- ✅ **Environment-specific** filter application
- ✅ **Selective inclusion/exclusion** of actions
- ✅ **Non-developer configuration** management

## 📋 **Approach 3: Controller-Level Configuration**

### **✅ Apply Filters per Controller**

```json
{
  "FilterOrder": {
    "ControllerFilters": {
      "Home": [
        {
          "FilterType": "ConfigurableRateLimit",
          "Order": 200,
          "Enabled": true,
          "ExcludeActions": ["Index", "Privacy"]
        }
      ],
      "Admin": [
        {
          "FilterType": "CustomAuthorize",
          "Order": 50,
          "Enabled": true,
          "Parameters": { "roles": "Admin" }
        }
      ]
    }
  }
}
```

**Benefits:**
- ✅ **Controller-specific** logic
- ✅ **Granular control** per controller
- ✅ **Inheritance and override** patterns

## 📋 **Approach 4: Action-Level Configuration**

### **✅ Fine-Grained Action Control**

```json
{
  "FilterOrder": {
    "ActionFilters": {
      "Home.Login": [
        {
          "FilterType": "ConfigurableRateLimit",
          "Order": 150,
          "Enabled": true
        },
        {
          "FilterType": "ValidateModel",
          "Order": 250,
          "Enabled": true
        }
      ],
      "Home.CreatePost": [
        {
          "FilterType": "CustomAuthorize",
          "Order": 100,
          "Enabled": true,
          "Parameters": { "roles": "User,Admin" }
        },
        {
          "FilterType": "ConfigurableRateLimit",
          "Order": 200,
          "Enabled": true
        }
      ]
    }
  }
}
```

**Benefits:**
- ✅ **Action-specific** filter chains
- ✅ **Maximum flexibility**
- ✅ **Override global/controller settings**

## 🔄 **Filter Execution Order Chain**

### **📊 Combined Order Resolution:**

1. **Global Filters** (from Program.cs or config)
2. **Controller Filters** (from config)
3. **Action Filters** (from config)
4. **Attribute Filters** (from controller/action attributes)

### **🎯 Order Calculation:**
```
Final Order = Math.Min(
    ConfiguredOrder,
    AttributeOrder,
    DefaultTypeOrder
)
```

## 🛠️ **Implementation Examples**

### **Example 1: Global Application via Program.cs**

```csharp
// Program.cs - Apply to ALL actions
builder.Services.AddControllersWithViews(options =>
{
    // Order matters - lower numbers execute first
    options.Filters.Add<OrderedLogActionAttribute>(order: 100);      // Logging first
    options.Filters.Add<OrderedRateLimitAttribute>(order: 200);      // Then rate limiting
    options.Filters.Add<ValidateModelAttribute>(order: 300);         // Then validation
    options.Filters.Add<OrderedCacheResponseAttribute>(order: 500);  // Finally caching
});
```

### **Example 2: Hybrid Approach (Recommended)**

```csharp
// Program.cs - Global basics
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<OrderedLogActionAttribute>(order: 100);  // Log everything
});

// appsettings.json - Specific configurations
{
  "FilterOrder": {
    "ActionFilters": {
      "Home.Login": [
        { "FilterType": "ConfigurableRateLimit", "Order": 150 },
        { "FilterType": "ValidateModel", "Order": 250 }
      ]
    }
  }
}

// Controller - Manual overrides for special cases
[OrderedRateLimit(5, 1, order: 175)]  // Override config for this specific action
public IActionResult SpecialAction() { ... }
```

### **Example 3: Configuration-Only Approach**

```csharp
// Program.cs - Minimal setup
builder.Services.AddControllersWithViews();

// Everything controlled via configuration
{
  "FilterOrder": {
    "DefaultOrder": {
      "AuthorizationOrder": 100,
      "RateLimitOrder": 200,
      "ValidationOrder": 300,
      "LoggingOrder": 400,
      "CachingOrder": 500
    },
    "GlobalFilters": [
      { "FilterType": "LogAction", "Order": 100, "Enabled": true }
    ],
    "ControllerFilters": {
      "Home": [
        { "FilterType": "ConfigurableRateLimit", "Order": 200 }
      ]
    }
  }
}
```

## 🎛️ **Configuration Structure Reference**

### **Complete Configuration Schema:**

```json
{
  "FilterOrder": {
    "DefaultOrder": {
      "AuthorizationOrder": 100,
      "RateLimitOrder": 200,
      "ValidationOrder": 300,
      "LoggingOrder": 400,
      "CachingOrder": 500,
      "ExceptionOrder": 600
    },
    "GlobalFilters": [
      {
        "FilterType": "LogAction",
        "Order": 100,
        "Enabled": true,
        "Parameters": {
          "message": "Global logging"
        },
        "ExcludeActions": ["HealthCheck", "Heartbeat"],
        "IncludeActions": []
      }
    ],
    "ControllerFilters": {
      "ControllerName": [
        {
          "FilterType": "FilterTypeName",
          "Order": 200,
          "Enabled": true,
          "Parameters": {},
          "ExcludeActions": [],
          "IncludeActions": []
        }
      ]
    },
    "ActionFilters": {
      "Controller.Action": [
        {
          "FilterType": "FilterTypeName",
          "Order": 300,
          "Enabled": true,
          "Parameters": {}
        }
      ]
    }
  }
}
```

## 🎯 **Supported Filter Types in Configuration**

- **`LogAction`** - Action logging with custom messages
- **`ConfigurableRateLimit`** - Rate limiting with action keys
- **`ConfigurableCacheResponse`** - Response caching with action keys
- **`ValidateModel`** - Model validation
- **`CustomAuthorize`** - Custom authorization with roles
- **`CustomExceptionFilter`** - Exception handling

## 🚀 **Usage Recommendations**

### **🏭 For Production Applications:**

1. **Global Logging** via Program.cs
2. **Security filters** via configuration for flexibility
3. **Performance filters** (rate limiting, caching) via configuration
4. **Business logic filters** via attributes on actions

### **🧪 For Demo/Development:**

1. **Simple attributes** on actions for clarity
2. **Configuration for testing** different scenarios
3. **Mix of both** to demonstrate capabilities

## 💡 **Best Practices Summary**

✅ **Use Program.cs for:** Universal filters (logging, security)  
✅ **Use Configuration for:** Environment-specific settings  
✅ **Use Attributes for:** Action-specific logic  
✅ **Use Hybrid for:** Maximum flexibility  

Your BlogMVCApp now supports **all approaches** - choose what works best for your specific needs! 🎯