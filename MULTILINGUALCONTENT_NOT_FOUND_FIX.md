# MultilingualContent Not Found - Complete Fix

## 🔴 Your Error

```
The type or namespace name 'MultilingualContent' could not be found
(are you missing a using directive or an assembly reference?)
```

This means the class `MultilingualContent` doesn't exist or Visual Studio can't find it.

---

## 🔍 Step 1: Check If File Exists

### Check Your Project Structure

You need this file to exist:

```
SomaShare/
├── Models/
│   ├── ApplicationUser.cs
│   ├── Textbook.cs
│   ├── Offer.cs
│   ├── Transaction.cs
│   ├── Review.cs
│   ├── WantedAd.cs
│   ├── MultilingualContent.cs  ← ⭐ THIS FILE MUST EXIST
│   ├── ChangePasswordViewModel.cs
│   ├── SearchFiltersViewModel.cs
│   ├── DashboardViewModel.cs
│   └── SecuritySettingsViewModel.cs
```

### How to Check in Visual Studio

1. Open Solution Explorer (Ctrl + Alt + L)
2. Expand "Models" folder
3. Look for `MultilingualContent.cs`

**If it's NOT there:** → Create it (see Step 2)

---

## 🆘 Step 2: Create the Missing File

If `MultilingualContent.cs` doesn't exist, create it NOW:

### In Visual Studio:

1. Right-click on **Models** folder
2. Select **Add** → **New Item**
3. Choose **Class**
4. Name it: `MultilingualContent.cs`
5. Click **Add**

### Copy-Paste This Code:

```csharp
using System.ComponentModel.DataAnnotations;

namespace SomaShare.Models
{
    public class MultilingualContent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        [Required]
        [StringLength(50)]
        public string Language { get; set; }

        [Required]
        [StringLength(1000)]
        public string Value { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }
    }

    public class LanguagePreference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string PreferredLanguage { get; set; } = "en";

        public DateTime SetDate { get; set; } = DateTime.UtcNow;

        public ApplicationUser User { get; set; }
    }
}
```

---

## ✅ Step 3: Add Using Statement Where Needed

Find where you're using `MultilingualContent` and add the using statement.

### In ApplicationDbContext.cs:

```csharp
// ADD THIS AT THE TOP:
using SomaShare.Models;

// Then you can use:
public DbSet<MultilingualContent> MultilingualContents { get; set; }
public DbSet<LanguagePreference> LanguagePreferences { get; set; }
```

### In MultilingualService.cs:

```csharp
// ADD THESE AT THE TOP:
using SomaShare.Models;
using SomaShare.Data;

// Then you can use:
var content = await _context.MultilingualContents
    .FirstOrDefaultAsync(c => c.Key == key && c.Language == language);
```

### In Controllers:

```csharp
// ADD THIS AT THE TOP:
using SomaShare.Models;
using SomaShare.Services;
using SomaShare.Data;

// Then you can use:
var filters = new SearchFiltersViewModel();
```

---

## 📋 Step 4: Complete Using Statements Reference

Here are ALL the using statements you need at the top of each file:

### ApplicationDbContext.cs
```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SomaShare.Models;  // ← ADD THIS
using System.Reflection.Emit;

namespace SomaShare.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // ...
    }
}
```

### MultilingualService.cs
```csharp
using Microsoft.EntityFrameworkCore;
using SomaShare.Data;  // ← ADD THIS
using SomaShare.Models;  // ← ADD THIS

namespace SomaShare.Services
{
    public class MultilingualService : IMultilingualService
    {
        // ...
    }
}
```

### SearchService.cs
```csharp
using Microsoft.EntityFrameworkCore;
using SomaShare.Data;  // ← ADD THIS
using SomaShare.Models;  // ← ADD THIS

namespace SomaShare.Services
{
    public class SearchService : ISearchService
    {
        // ...
    }
}
```

### DashboardService.cs
```csharp
using Microsoft.EntityFrameworkCore;
using SomaShare.Data;  // ← ADD THIS
using SomaShare.Models;  // ← ADD THIS

namespace SomaShare.Services
{
    public class DashboardService : IDashboardService
    {
        // ...
    }
}
```

### CommunityTrustService.cs
```csharp
using Microsoft.EntityFrameworkCore;
using SomaShare.Data;  // ← ADD THIS
using SomaShare.Models;  // ← ADD THIS

namespace SomaShare.Services
{
    public class CommunityTrustService : ICommunityTrustService
    {
        // ...
    }
}
```

### PasswordSecurityService.cs
```csharp
using SomaShare.Models;  // ← ADD THIS

namespace SomaShare.Services
{
    public class PasswordSecurityService : IPasswordSecurityService
    {
        // ...
    }
}
```

### SearchController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Data;  // ← ADD THIS
using SomaShare.Models;  // ← ADD THIS
using SomaShare.Services;  // ← ADD THIS

namespace SomaShare.Controllers
{
    public class SearchController : Controller
    {
        // ...
    }
}
```

### DashboardController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;  // ← ADD THIS
using SomaShare.Services;  // ← ADD THIS

namespace SomaShare.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // ...
    }
}
```

### LanguageController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;  // ← ADD THIS
using SomaShare.Services;  // ← ADD THIS

namespace SomaShare.Controllers
{
    public class LanguageController : Controller
    {
        // ...
    }
}
```

### AccountSecurityController.cs
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;  // ← ADD THIS
using SomaShare.Services;  // ← ADD THIS

namespace SomaShare.Controllers
{
    [Authorize]
    public class AccountSecurityController : Controller
    {
        // ...
    }
}
```

---

## 🔧 Step 5: Visual Studio Auto-Fix

### Method 1: Use IntelliSense

1. Click on the red squiggly under `MultilingualContent`
2. Press `Ctrl + .` (Quick Actions)
3. Click "using SomaShare.Models;"
4. Visual Studio adds it automatically

### Method 2: Manual Add

1. Put cursor on `MultilingualContent`
2. Right-click → **Quick Actions and Refactorings**
3. Click **using SomaShare.Models;**

### Method 3: Type It Yourself

Just type at the very top of the file:
```csharp
using SomaShare.Models;
```

---

## ✅ Step 6: Verify File Structure

Make sure your `Models` folder contains these files:

```
Models/
├── ApplicationUser.cs ✅
├── Textbook.cs ✅
├── Offer.cs ✅
├── Transaction.cs ✅
├── Review.cs ✅
├── WantedAd.cs ✅
├── MultilingualContent.cs ✅ (THIS IS THE NEW ONE)
├── ChangePasswordViewModel.cs ✅
├── SearchFiltersViewModel.cs ✅
├── DashboardViewModel.cs ✅
├── CommunityTrustScoreViewModel.cs ✅
└── SecuritySettingsViewModel.cs ✅
```

---

## 🚀 Step 7: Build Again

```powershell
dotnet build
```

If you still see the error, check:
1. File path is correct
2. Namespace is exactly `SomaShare.Models`
3. Using statement is at the TOP of the file
4. No typos in class name or file name

---

## 🆘 If Still Not Working

### Nuclear Option: Clean and Rebuild

```powershell
# In Package Manager Console:
dotnet clean

# Delete folders
Remove-Item -Path "bin" -Recurse -Force
Remove-Item -Path "obj" -Recurse -Force

# Rebuild
dotnet build
```

---

## Checklist: Before Migration

- [ ] `MultilingualContent.cs` file EXISTS in Models folder
- [ ] `using SomaShare.Models;` is at top of ApplicationDbContext.cs
- [ ] `public DbSet<MultilingualContent> MultilingualContents { get; set; }` exists in DbContext
- [ ] `public DbSet<LanguagePreference> LanguagePreferences { get; set; }` exists in DbContext
- [ ] All namespaces are exactly `namespace SomaShare.Models`
- [ ] All classes are `public class`
- [ ] No syntax errors (missing semicolons, braces)
- [ ] `dotnet build` returns "Build succeeded"

---

## Summary

The error means:
1. File `MultilingualContent.cs` doesn't exist, OR
2. Using statement is missing, OR
3. Wrong namespace/spelling

**Solutions in order:**
1. ✅ Create file if missing
2. ✅ Add `using SomaShare.Models;`
3. ✅ Check spelling/namespace
4. ✅ Run `dotnet build`

**Once this is fixed:**
```powershell
Add-Migration AddMultilingualSupport
Add-Migration AddSecurityFields
Update-Database
```

---

## Need More Help?

Tell me:
1. Does the file `Models/MultilingualContent.cs` exist? (Yes/No)
2. What error do you see in Package Manager Console?
3. Which file is causing the error? (filename)

I can help debug!
